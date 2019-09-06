//Written By Michael Elkin 05/06/2019
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Mike's Scripts/Kickable_Object", 11)]
public class Kickable_Object : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField, Range(1,10)]
    float forwardForce = 1.0f;
    [SerializeField, Range(1, 10)]
    float upwardForce = 1.0f;
    [SerializeField, Range(0, 5)]
    float drag = 0.0f;
    [SerializeField, Range(0, 5)]
    float frictionMod = 0.0f;
    float friction;

    float maxDistance = 100.0f; // Used To Store Max Distance for Raycasts
    RaycastHit hit; // Used to store Raycast Hit info
    bool grounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = drag;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //IsGrounded();
        //if(grounded)
        //{
        //    AddFriction();
        //}
    }

    

    private void OnTriggerEnter(Collider o)
    {
        if(o.gameObject.tag == "Player")
        {
            //if(Vector3.Angle(o.transform.forward, transform.forward) >= 90)
            //{
            Vector3 kickVector = (o.transform.position - transform.position).normalized;
                rb.AddForce((kickVector * forwardForce) + (o.transform.up * upwardForce), ForceMode.Impulse);
            //}
            //else
            //{
            //    rb.AddForce((o.transform.forward * forwardForce) + (o.transform.up * upwardForce), ForceMode.Impulse);
            //}            
        }
    }

    void AddFriction()
    {
        friction = frictionMod * rb.mass * Physics.gravity.magnitude;
        rb.AddForce(-friction * rb.velocity.normalized);
    }

    void IsGrounded()
    {
        Ray groundedRay = new Ray(transform.position, Vector3.down);
        Physics.Raycast(groundedRay, out hit, maxDistance);
        if (hit.distance <= 1.125f)
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }

}
