using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//updated 05/28- LF
//updated 08/06

public class GrappleComponent : MonoBehaviour
{

    [Range(45, 90)]

    public Rigidbody rb;
    public Transform tetherPoint;
    public Transform attachedTetherPoint; 
    public bool tether = false;
    public float maxSwingSpeed;
    public float forwardSpeed = 100;
    public float sidewaysSpeed = 8;
    public float jumpSpeed = 10;
    public float launchSpeed;
    public float boost = 20;
    private bool reachedZero = false;

    public bool isStaring;

    [Range(20, 90)]
    public float maxAngle;


    PlayerClass player;

    // Start is called before the first frame update

    public void Initialize()
    {
        player = GetComponentInParent<PlayerClass>();
        rb = player.rb;
    }

    public void Grapple()
    {
        if (tetherPoint == null)
        {
            Debug.Log("No tetherpoint");
            return;
        }

        tether = true;
        attachedTetherPoint = tetherPoint;


    }



    // Update is called once per frame
    void Update()
    {
        if (!tether)
            return;

           
            rb.mass = 100;
            Vector3 radial = (attachedTetherPoint.position - transform.position).normalized;
            Vector3 radialVelocityComponent = Vector3.Dot(rb.velocity, radial) * radial;
            rb.velocity -= radialVelocityComponent;
            rb.velocity = rb.velocity.normalized * Mathf.Min(rb.velocity.magnitude, maxSwingSpeed);

            Vector3 tetherDirection = attachedTetherPoint.position - transform.position;
            float tetherLength = tetherDirection.magnitude;
            float x = Vector3.Dot(tetherDirection.normalized, Vector3.up);
            float y = (tetherDirection.normalized - x * Vector3.up).magnitude;
            float theta = Mathf.Atan2(y, x);
            float tension = rb.mass * 9.8f * Mathf.Cos(theta) + rb.mass * Mathf.Pow(rb.velocity.magnitude, 2) / tetherLength;
            rb.AddForce(tension * tetherDirection.normalized);
            Debug.DrawLine(transform.position, attachedTetherPoint.position);
            float thetaDegrees = theta * Mathf.Rad2Deg;
            //Debug.Log(thetaDegrees);


            if (thetaDegrees < 5)
            {
                Debug.Log("Reached Zero");
                reachedZero = true;
            }

            if (reachedZero)
            {
                rb.AddForce(rb.velocity.normalized * boost, ForceMode.Impulse);

                if (thetaDegrees > maxAngle)
                {
                    rb.mass = 1;
                    rb.AddForce(rb.velocity.normalized * launchSpeed, ForceMode.Impulse);
                    //normalMove.enabled = true;
                    tether = false;
                    attachedTetherPoint = null;
                    reachedZero = false;
                }

            }



   
    }

    public void DetatchGrapple()
    {
        rb.mass = 1;
        rb.AddForce(rb.velocity.normalized * launchSpeed, ForceMode.Impulse);
        tether = false;

        if (tetherPoint == attachedTetherPoint && isStaring == false)
            tetherPoint = null;

        attachedTetherPoint = null;

        reachedZero = false;
    }

    public Transform setTetherPoint(Transform t)
    {
        if (t != null)
        {
            tetherPoint = t;
            return tetherPoint;
        }


        Debug.Log("No tetherpoint");
        tetherPoint = null; 
        return tetherPoint;

    }




    //private void OnDisable()
    //{
    //    tether = false;
    //    rb.mass = 1;
    //}




}


