// Created April 30, 2019 by Alek Tepylo - updated movement script (no wall functionality)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementv2 : MonoBehaviour
{
    //rigidbody
    public Rigidbody rb;

    //camera object
    public Camera cammy;

    //raycasts
    private RaycastHit footHit;
    private RaycastHit faceHit;
    int layerMask = 1 << 2;

    //vectors
    private readonly Vector3 halves = new Vector3(0.34f, 0.385f, 0.34f);

    //priamtives
    //private
    private float horizontal;
    private float vertical;
    private bool deadJoy;
    private readonly float deadZone = 0.028f;
    private readonly float decelFactor = 0.14f;
    private readonly float velocityDivider = 1.2f;
    private readonly float groundCheckRate = 0.1f;

    //public - to test balance etc.
    //for movement
    float walkAccel = 45;
    public float climbAccel = 12;
    public float walkMax = 10;
    public float airMax = 8;
    public float rotateSpeed = 30;
    private float frictionCoeff = 0.2f;

    public bool grounded;
    public bool onWall;

    //for jumps
    public float jumpForce = 10;
    public float jumpMultiplier = 1.5f;
    public float fallMultiplier = 2.5f;
    public bool canFlutter;
    public float flutterForce = 7;

    //for grapple
    bool toggle;

    //for animation
    public Animator anim;
    private float moveSpeed;
    private float rotate;
    private float animRotate;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (!cammy)
        {
            cammy = Camera.main;
        }
        grounded = true;
        StartCoroutine(CheckGround());
        canFlutter = false;
    }


    private void Update()
    {
 
        layerMask = ~layerMask;
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + .3f, transform.position.z), transform.forward, out faceHit, .5f, layerMask) && !grounded)
        {
            if (faceHit.collider.gameObject != null)
            {
                //Debug.Log(faceHit.collider.gameObject.ToString());
                onWall = true;
                //StartCoroutine("Slide", 1);
            }
        }
        else
        {
            onWall = false;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        ControlInput();
        setSpeed();//for animations
        setRotate();//for animations
    }

    public void ControlInput()
    {
        horizontal = Input.GetAxis("HorizontalJoy") + Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("VerticalJoy") + Input.GetAxis("Vertical");
        //jump
        if (Input.GetButtonDown("Jump"))
        {
            if (grounded && !onWall)
            {
                Jump();
            }
            else if (onWall && !grounded)
            {
                WallJump();
            }
        }

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (jumpMultiplier - 1) * Time.fixedDeltaTime;
        }
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

        //caches the start rotation
        Vector3 currentRotation = transform.forward;

        //rotates the direction the character is facing to the correct direction based on camera
        //need to have two functions one for short rotations and one for sharp turns with different rotate speed (high speed for sharp turns, low speed for slow turns)
        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, cammyFront * vertical + cammyRight * horizontal, rotateSpeed * Time.fixedDeltaTime, 0.0f));

        //calculate the rotation for animator
        rotate = Vector3.SignedAngle(currentRotation, transform.forward, transform.up);
        rb.AddForce(transform.forward * walkAccel, ForceMode.Force);

        if (grounded)
        {
            Friction();
        }
    }

    //clamps the velocity
    void ApplyVelocityCutoff()
    {
        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0;
        if (grounded)
        {
            horizontalVelocity = Mathf.Min(horizontalVelocity.magnitude, walkMax) * horizontalVelocity.normalized;
        }
        else
        {
            horizontalVelocity = Mathf.Min(horizontalVelocity.magnitude, airMax) * horizontalVelocity.normalized;
        }
        rb.velocity = horizontalVelocity + rb.velocity.y * Vector3.up;
    }

    void Friction()
    {
        float friction = frictionCoeff * rb.mass * Physics.gravity.magnitude;
        Vector3 groundNormal = footHit.normal;
        Vector3 normalComponentofVelocity = Vector3.Dot(rb.velocity, groundNormal) * groundNormal;
        Vector3 parallelComponentofVelocity = rb.velocity - normalComponentofVelocity;
        rb.AddForce(-friction * parallelComponentofVelocity);
    }


    public void Jump()
    {
        if (!grounded)
        {
            StartCoroutine("DoubleJumpActivate");
        }
        if (grounded)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            anim.SetTrigger("Jump");
            grounded = false;
        }
        else if (canFlutter && !grounded && !onWall)
        {
            //zero out velocity at start of flutter jump to prevent to much height
            Vector3 tempVelocity = rb.velocity;
            tempVelocity.y = 0;
            rb.velocity = tempVelocity;

            rb.AddForce(transform.up * flutterForce, ForceMode.Impulse);
            canFlutter = false;
            anim.SetTrigger("Jump");
        }
        if (transform.parent != null)
        {
            transform.parent = null;
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

    public IEnumerator CheckGround()
    {
        if (Physics.BoxCast(transform.position, halves, Vector3.down, out footHit, Quaternion.identity, halves.y))
        {
            if (footHit.collider.gameObject != null)
            {
                GroundMe();
            }
        }
        else
        {
            grounded = false;
            anim.SetBool("Grounded", false);
        }
        yield return new WaitForSecondsRealtime(groundCheckRate);
        StartCoroutine(CheckGround());
    }

    public void GroundMe()
    {
        grounded = true;
        canFlutter = true;
        anim.SetBool("Grounded", true);
        if (footHit.collider.gameObject.tag == "MovingPlatform")
        {
            transform.parent = footHit.transform.parent;
        }
    }

    IEnumerator DoubleJumpActivate()
    {
        yield return new WaitForSeconds(0.1f);
        canFlutter = true;
    }
    //function to call when bouncing off an object (i.e. mushrooms)
    //public void AddBounceForce(Vector3 direction, float force)
    //{        
    //    rb.velocity = Vector3.zero;
    //    rb.AddForce(transform.up * force, ForceMode.Impulse);
    //}


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

    public void WallJump()
    {
        if (onWall)
        {
            // jumps off of wall
            rb.AddForce((-transform.forward * 10) + (transform.up * 5), ForceMode.Impulse);
            // sets player looking away from wall
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, -transform.forward, rotateSpeed * Time.fixedDeltaTime, 0.0f));
            transform.forward = -transform.forward;
        }
        if (transform.parent != null)
        {
            transform.parent = null;
        }
        onWall = false;
    }

    IEnumerator Slide(float t)
    {
        yield return new WaitForSeconds(t);
        float drag = 0;
        drag += 1 * Time.deltaTime;
        rb.velocity += new Vector3(0, -drag, 0);
    }
}



