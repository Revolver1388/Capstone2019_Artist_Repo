//created 19-08-09 - ground movement scrip based on old player movement script by AT edited by LF
//keeps ground movement but removed air funtionallity

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundMovement : MonoBehaviour
{
    //rigidbody
    public Rigidbody rb;

    //camera object
    public Camera cammy;

    //raycasts
    private RaycastHit footHit;

    //vectors
    private readonly Vector3 halves = new Vector3(0.34f, 0.385f, 0.34f);

    //priamtives
    //private
    private float horizontal;
    private float vertical;
    private bool deadJoy;
    private readonly float deadZone = 0.028f;
    private readonly float decelFactor = 0.14f;
    
    //public - to test balance etc.
    //for movement
    public float walkAccel = 60;
    public float climbAccel = 12;
    public float walkMax = 10;
    //public float airMax = 8;
    public float rotateSpeed = 120;
    private float frictionCoeff = 0.2f;

    public bool grounded;

    //for jumps
    public float jumpForce = 8; 
    
    //for animation
    public Animator anim;
    private float moveSpeed;
    private float rotate;
    private float animRotate;

    PlayerClass player;
       
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
        player = GetComponentInParent<PlayerClass>();
        anim = GetComponentInParent<PlayerClass>().GetAnimator();
     
        GameObject camObject = GameObject.FindGameObjectWithTag("MainCamera");
        cammy = camObject.GetComponent<Camera>();
    }
             
    // Update is called once per frame
    void FixedUpdate()
    {
        grounded = player.IsGrounded();

        ControlInput();
        setSpeed();//for animations
        setRotate();//for animations
    }

    public void ControlInput()
    {
        horizontal = Input.GetAxis("HorizontalJoy") + Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("VerticalJoy") + Input.GetAxis("Vertical");
        
        //if move input then move if no input stop
        if (horizontal > deadZone || horizontal < -deadZone || vertical > deadZone || vertical < -deadZone)
        {
            Movement();
        }
        else if (horizontal <= deadZone && horizontal >= -deadZone && vertical <= deadZone && vertical >= -deadZone)
        {
            Decel();
        }
               
        ApplyVelocityCutoff();
    }

    public void Movement()
    {
        //movement based on direction camera is facing
        Vector3 cammyRight = cammy.transform.TransformDirection(Vector3.right);
        Vector3 cammyFront = cammy.transform.TransformDirection(Vector3.forward);
        cammyRight.y = 0;
        cammyFront.y = 0;
        cammyRight.Normalize();
        cammyFront.Normalize();
                        
        player.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, cammyFront * vertical + cammyRight * horizontal, rotateSpeed * Time.fixedDeltaTime, 0.0f));
               
        //adds force to the player
        rb.AddForce(transform.forward * walkAccel, ForceMode.Force);
                
        Friction();
    }

    //clamps the velocity
    void ApplyVelocityCutoff()
    {
        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0;
        
        horizontalVelocity = Mathf.Min(horizontalVelocity.magnitude, walkMax) * horizontalVelocity.normalized;
       
        rb.velocity = horizontalVelocity + rb.velocity.y * Vector3.up;
    }

    void Friction()
    {
        float friction = frictionCoeff * rb.mass * Physics.gravity.magnitude;
        Vector3 groundNormal = player.GetFootHit().normal;
        Vector3 normalComponentofVelocity = Vector3.Dot(rb.velocity, groundNormal) * groundNormal;
        Vector3 parallelComponentofVelocity = rb.velocity - normalComponentofVelocity;
        rb.AddForce(-friction * parallelComponentofVelocity);
    }
    
    public void Jump()
    {
        //Debug.Log("Normal Jump");
        rb.velocity /= 2;

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        anim.SetTrigger("Jump");
       
        if (player.transform.parent != null)
        {
            player.transform.parent = null;
        }
              
        anim.SetBool("Grounded", false);
    }

    public void Decel()
    {
        if (!deadJoy)
        {
            deadJoy = true;
        }

        if (rb.velocity.magnitude > 1f && grounded)
        {
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, decelFactor);
        }
    }
       
    //for animator
    public void setSpeed()
    {
        moveSpeed = rb.velocity.magnitude / walkMax;
        anim.SetFloat("Speed", moveSpeed);
    }

    public void setRotate()
    {
        animRotate = rotate / rotateSpeed;
        anim.SetFloat("Rotate", animRotate);
    }
}
