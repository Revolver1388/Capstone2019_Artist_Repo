//created Aug 8, 2019 - air controller

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirMovement : MonoBehaviour
{
    //architecture
    PlayerClass player;

    //rigid body
    private Rigidbody rb;

    //camera object
    private Camera cammy;

    //wall check
    private bool onWall;
    private float wallCheckRate = 0.1f;

    //for animator
    private Animator anim;

    //for jumps
    private bool canFlutter;
    public float doubleJumpForce = 7;

    //gravity modifiers
    public float jumpMultiplier = 3f;
    public float fallMultiplier = 8f;
    public float peakTime = 0.3f; 
    public float peakHeightMultiplier = 0.8f;
    public float wallMultiplier = 0.5f;

    //air movement
    private float horizontal;
    private float vertical;
    public float airForwardSpeed = 10f;
    public float airSideSpeed = 5f;
    public float wallJumpVertical = 7;
    public float wallJumpHorizontal = 7;
    //need high airMax to allow long jump
    private float airMax = 12f;
    public float testAirMax = 7;
    private float rotateSpeed = 120f;

    private bool deadJoy;
    private readonly float deadZone = 0.028f;
    private readonly float decelFactor = 0.14f;
    private readonly float velocityDivider = 1.2f;


    private bool wallDeadZone;
    public bool doubleJumpControl;


    //rayCasts
    private RaycastHit faceHit;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
        player = GetComponentInParent<PlayerClass>();
        anim = GetComponentInParent<PlayerClass>().GetAnimator();

        GameObject camObject = GameObject.FindGameObjectWithTag("MainCamera");
        cammy = camObject.GetComponent<Camera>();

    }

    private void OnEnable()
    {
        //starts checking walls when ever the script is enabled
        StartCoroutine(WallWait());
        wallDeadZone = false;
        doubleJumpControl = false; 
    }

    private void OnDisable()
    {
        //stops checking wall on disables to prevent multiple checks running
        StopAllCoroutines();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        canFlutter = player.CanFlutter();
        ControlInput();
    }
           
    private void ControlInput()
    {
        horizontal = Input.GetAxis("HorizontalJoy") + Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("VerticalJoy") + Input.GetAxis("Vertical");

        //will slide a slower speed down wall
        if(onWall && rb.velocity.y <= 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (wallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if(rb.velocity.y < -peakTime && rb.velocity.y > peakTime)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (peakHeightMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.velocity.y < peakTime)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        //player will fall faster on way down
        else if (rb.velocity.y > peakTime && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (jumpMultiplier - 1) * Time.fixedDeltaTime;
        }

        Vector3 forward = transform.forward;
        Vector3 inputDir = transform.forward * vertical + transform.right * horizontal;

        //if move input then move if no input stop
        if (horizontal > deadZone || horizontal < -deadZone || vertical > deadZone || vertical < -deadZone)
        {
            if (!wallDeadZone)
            {
                if(Vector3.Dot(forward, inputDir) < 0) // to prevent sticking to walls (and not slidining down) when input is in the direction of the wall
                {
                    AirMovement();
                }
            }
        }       
        
        ApplyVelocityCutoff();
    }

    public void AirMovement()
    {
        //movement based on direction camera is facing
        Vector3 cammyRight = cammy.transform.TransformDirection(Vector3.right);
        Vector3 cammyFront = cammy.transform.TransformDirection(Vector3.forward);
        cammyRight.y = 0;
        cammyFront.y = 0;
        cammyRight.Normalize();
        cammyFront.Normalize();

        //rotates the direction the character is facing to the correct direction based on camera
        //air script is different than the ground movement, as the character moves slower in the air and does NOT rotate to face camera forward instead will shift from side to side

        //following two lines are what we's use if we wanted to rotate character
        //player.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, cammyFront * vertical, rotateSpeed * Time.fixedDeltaTime, 0.0f));
        //rb.AddForce(transform.forward * Mathf.Abs(vertical) * airForwardSpeed + cammyRight * horizontal * airSideSpeed, ForceMode.Force);
             

        //adds force to the player
        if (!doubleJumpControl)
        {
            rb.AddForce(cammy.transform.forward * vertical * airForwardSpeed + cammyRight * horizontal * airSideSpeed, ForceMode.Force);
        }
        else
        {
            player.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, cammyFront * vertical + cammyRight * horizontal, rotateSpeed * Time.fixedDeltaTime, 0.0f));
            rb.AddForce(transform.forward * airForwardSpeed, ForceMode.Force);
        }
    }

    void ApplyVelocityCutoff()
    {
        //if long jumping has a higher air max to allow longer jumps
        if(player.GetCrouching())
        {
            airMax = 50;
        }
        //air max of 12 is for normal jumps
        else
        {
            airMax = testAirMax;
        }

        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0;
       
        horizontalVelocity = Mathf.Min(horizontalVelocity.magnitude, airMax) * horizontalVelocity.normalized;
       
        rb.velocity = horizontalVelocity + rb.velocity.y * Vector3.up;
    }

    public void Jump()
    {       
        if(canFlutter && !onWall)
        {
            //Debug.Log("Flutter Jump");
            //zero out velocity at start of flutter jump to prevent to much height
            Vector3 tempVelocity = rb.velocity;
            tempVelocity.y = 0;
            rb.velocity = tempVelocity;

            //to allow control for a brief period after double jump
            doubleJumpControl = true;
            StartCoroutine(DoubleJumpControl());

            rb.AddForce(transform.up * doubleJumpForce, ForceMode.Impulse);
            player.SetFlutter(false);
        }
        else if(onWall)
        {            
            // jumps off of wall
            rb.AddForce((-transform.forward * wallJumpHorizontal) + (transform.up * wallJumpVertical), ForceMode.Impulse);
            // sets player looking away from wall (two ways to do it)
            //player.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, -transform.forward, rotateSpeed * Time.fixedDeltaTime, 0.0f));
            player.transform.forward = -transform.forward;
                        
            onWall = false;
            wallDeadZone = true;
            StartCoroutine(WallDeadZone());
        }

        if (player.transform.parent != null)
        {
            player.transform.parent = null;
        }
    }


    public IEnumerator CheckWall()
    {
        //top of head boxcast
        Vector3 topRaycastLocation = new Vector3(transform.position.x, transform.position.y + 0.5f * transform.localScale.y - 0.1f, transform.position.z);
        Vector3 topRaycastHalf = new Vector3(0.5f * transform.localScale.x, 0.1f, 0.5f * transform.localScale.z);

        //euler in box cast currently checks all around player would need to change if only want it in the front
        //bool topOfHead = Physics.BoxCast(topRaycastLocation, topRaycastHalf, transform.forward, out faceHit, Quaternion.Euler(0, 2 * Mathf.PI, 0), 0.5f * transform.localScale.z + 0.1f);
        //distance checks slighly in front of player, may want ot change depending on play testing
        bool topOfHead = Physics.Raycast(topRaycastLocation, transform.forward, 0.5f * transform.localScale.z + 0.1f);
       
        //toe  raycast
        Vector3 toeRaycastLocation = new Vector3(transform.position.x, transform.position.y - 0.5f * transform.localScale.y + 0.1f, transform.position.z);
        Vector3 toeRaycastHalf = new Vector3(0.5f * transform.localScale.x, 0.1f, 0.5f * transform.localScale.z);

        //bool toeCast = Physics.BoxCast(toeRaycastLocation, toeRaycastHalf, transform.forward, out faceHit, Quaternion.Euler(0, 2 * Mathf.PI, 0), 0.5f * transform.localScale.z + 0.1f);
        bool toeCast = Physics.Raycast(toeRaycastLocation, transform.forward, 0.5f * transform.localScale.z + 0.1f);
       
        //mid raycast
        Vector3 midRaycastLocation = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Vector3 midRaycastHalf = new Vector3(0.5f * transform.localScale.x, 0.1f, 0.5f * transform.localScale.z);

        //bool midCast = Physics.BoxCast(minRaycastLocation, toeRaycastHalf, transform.forward, out faceHit, Quaternion.Euler(0, 2 * Mathf.PI, 0), 0.5f * transform.localScale.z + 0.1f);
        bool midCast = Physics.Raycast(midRaycastLocation, transform.forward, 0.5f * transform.localScale.z + 0.1f);
   
        //if all three
        if (toeCast && midCast && topOfHead)
        {
            onWall = true;
        }
        else if (toeCast && !midCast && !topOfHead)
        {
            //call function to move player up on top of platform if we want
        }
        else if (toeCast && midCast && !topOfHead)
        {
            //ledge grab if we have it
        }
        else
        {
            onWall = false;
        }

        yield return new WaitForSecondsRealtime(wallCheckRate);

        StartCoroutine(CheckWall());
    }

    //wait for time after air controller is initially enabled to prevent sticking to wall initially if next to wall
    IEnumerator WallWait()
    {
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(CheckWall());
    }

    //prevents player input for a time directly after wall jumping
    IEnumerator WallDeadZone()
    {
        yield return new WaitForSeconds(0.3f);
        wallDeadZone = false;
    }

    IEnumerator DoubleJumpControl()
    {
        yield return new WaitForSeconds(0.75f);
        doubleJumpControl = false;
    }

}
