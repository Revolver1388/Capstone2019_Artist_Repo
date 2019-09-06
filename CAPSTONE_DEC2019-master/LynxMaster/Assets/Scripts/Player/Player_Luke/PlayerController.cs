//created by Luke Fentress - controller script to handle player movement types
//edited by AT - 19-08-09 - added air controller functionallity

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public PlayerClass player;

    [Header("STATE MACHINE NOT ANIMATION")]
    [SerializeField]
    Animator stateMachine;



    //annoying temporary check until we slay the beast that is the grappleComponent tether bool
    bool isTethered = false;
    bool isCrouching = false;
    public bool GetIsCrouching()
    {
        return isCrouching;
    }

    private void Awake()
    {
        player.InitializePlayer();
    }

    private void Start()
    {
        StartCoroutine(CheckGround());
    }

    public void Jump()
    {
        //player.GetMoveComponent().Jump();
        //currently using if statement for this
        //however should set up a "currentMovementComponet" then just call player.currentMovementComponent.Jump()
        
        if (player.GetMovementType() == player.move)
        {
            player.GetMoveComponent().Jump();
            //set movement type to air
            player.SetMovementType("air");
            //sets grounded to false outside of 
            player.SetGrounded(false);
            stateMachine.SetTrigger("JumpTrigger");
        }
        else if(player.GetMovementType() == player.air)
        {
            player.GetAirComponent().Jump();
        }
        else if(player.GetMovementType() == player.crouch)
        {
            player.GetCrouchComponent().Jump();
            player.SetMovementType("air");
            player.SetGrounded(false);
        }
        else if(player.GetMovementType() == player.grapple)
        {
            //player.GetGrappleComponent().Jump();
        }
        
    }

    public void Grapple()
    {
        //stupid fucking tethered
        if(!isTethered)
        {
            stateMachine.SetTrigger("GrappleTrigger");
            isTethered = true; 
        }

        player.SetMovementType("grapple");
        player.GetGrappleComponent().Grapple();
    }

    public void DetatchGrapple()
    {
        player.GetGrappleComponent().DetatchGrapple();
        stateMachine.SetTrigger("FallTrigger");

        //this would be the fall state, but since we don't yet have an air controller
        //player.SetMovementType("move");
    }
    
    //man this is alot of jumping back and forth to do something simple lol
    public void Crouch()
    {
        player.SetMovementType("crouch");
        isCrouching = true;
        player.SetCrouching(isCrouching);
    }

    public void DeCrouch()
    {
        player.SetMovementType("move");
        isCrouching = false;
        player.SetCrouching(isCrouching);
    }

    #region check ground functions

    private readonly Vector3 halves = new Vector3(0.34f, 0.385f, 0.34f);
    private readonly float groundCheckRate = 0.1f;
    private RaycastHit footHit;

    public void GroundMe()
    {  
        if (Physics.BoxCast(transform.position, halves, Vector3.down, out footHit, Quaternion.identity, halves.y))
        {
            if (footHit.collider.gameObject != null && !isCrouching)
            {
                player.SetGrounded(true);
                player.SetFlutter(true);
                player.SetMovementType("move");
                player.GetAnimator().SetBool("Grounded", true);
                stateMachine.SetTrigger("GroundTrigger");
                
            }
            else if (footHit.collider.gameObject != null && isCrouching)
            {
                player.SetGrounded(true);
                player.SetFlutter(true);
                player.SetMovementType("crouch");
                player.GetAnimator().SetBool("Crouching", true);
            }
            else if (footHit.collider.gameObject.tag == "MovingPlatform")
            {
                transform.parent = footHit.transform.parent;
            }
        }
    }

    public IEnumerator CheckGround()
    {
        if (Physics.BoxCast(transform.position, halves, Vector3.down, out footHit, Quaternion.identity, halves.y))
        {
            GroundMe();
        }
        else
        {
            player.SetGrounded(false);
            player.GetAnimator().SetBool("Grounded", false);
            //Debug.Log("not on ground");
            player.SetMovementType("air");
            //this is an issue for the fall trigger. We can't put it here since it'll as of now conflict with the Grapple Trigger
        }

        yield return new WaitForSecondsRealtime(groundCheckRate);

        StartCoroutine(CheckGround());
    }



    #endregion

}
