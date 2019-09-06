//Written by Mike Elkin 07/10/2019
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
[AddComponentMenu("Mike's Scripts/TriggerBlock", 9)]

public class TriggerBlock : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb;// Switch Rigidbody
    [SerializeField, Range(0, 50)]
    float blockMass = 1.0f;// Setting For Mass of Rigidbody
    [SerializeField, Range(0, 1)]
    float deadZone = 0.5f;// Buffer Zone for Block Movement
    [SerializeField, Range(0, 10)]
    float triggerDepth = 1.0f;
    [SerializeField]
    MultiPurposePlatform triggerActivated;//Platform to Trigger
    [SerializeField]
    Vector3 lockedPosition;// Home Position Once Triggered 
    [SerializeField]
    Vector3 currentPosition;// Position in Real Time
    [SerializeField]
    Vector3 startPosition;// Position on Awake
    [SerializeField]
    Vector3 endPosition;// Trigger Point
    Vector3 downForce;// Downward Force Vector
    Vector3 upForce;// Upward Force Vector


    bool playerOnSwitch;

    void Awake()
    {
        rb = GetComponentInChildren<Rigidbody>();
        rb.mass = blockMass;
        startPosition = transform.position;
        endPosition = startPosition + Vector3.down * triggerDepth;
        lockedPosition = endPosition + Vector3.down;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        IsTriggered();
        IsLocked();
        AddForce();
        CurrentPosition();

    }

    private void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag != "Player")
        {
            Debug.Log("Not Colliding");
            Physics.IgnoreCollision(transform.GetComponent<BoxCollider>(), c.collider);

            //if (transform.position.y <= endPosition.y && !playerOnSwitch)
            //{
            //    startPosition = endPosition;
            //    playerOnSwitch = true;
            //}
            //impactVel = c.gameObject.GetComponent<Rigidbody>().mass * Physics.gravity;
        }
        
    }
    private void OnTriggerEnter(Collider o)
    {
        if (o.gameObject.tag == "Player")
        {
            playerOnSwitch = true;
            o.transform.parent = transform;
        }
    }
    private void OnTriggerExit(Collider o)
    {
        if (o.gameObject.tag == "Player")
        {
            playerOnSwitch = false;
            o.transform.parent = null;
        }
    }
    /*
    private void OnTriggerEnter(Collider o)
    {
        if (o.gameObject.tag == "Player")
        {
            Player.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider o)
    {
        if (o.gameObject.tag == "Player")
        {
            Player.transform.parent = null;
        }
    }
     */

    void IsTriggered()
    {
        if (transform.position.y <= endPosition.y)
        {
            startPosition = lockedPosition;
            //rb.drag = 50.0f;
            //rb.mass = 100.0f;
            // Add Trigger For Asociated Platform
        }
    }

    void CurrentPosition()
    {
        currentPosition = transform.position;
    }

    void IsLocked()
    {
        if (startPosition == lockedPosition)
        {
            rb.drag = 50.0f;
            rb.mass = 100.0f;
        }
    }


    void AddForce()
    {
        //CalcUpwardForce();
        if (transform.position.y > startPosition.y + deadZone)
        {
            Debug.Log("Pushing Down");
            rb.AddForce((rb.mass * 9.8f) * Vector3.down, ForceMode.Acceleration);
        }
        if (transform.position.y < startPosition.y - deadZone)
        {
            Debug.Log("Pushing Up");
            rb.AddForce((rb.mass * -9.8f) * Vector3.up * 0.75f, ForceMode.Acceleration);
        }
    }

}
