//Written by Mike Elkin 08/19/2019
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Mike's Scripts/TimedPlatform", 22)]

public class TimedPlatform : MonoBehaviour
{
    //[SerializeField]
    //Rigidbody rb;
    [SerializeField, Range(1, 5)]
    float bottomPointOffset = 1.0f;
    [SerializeField, Range(0, 1)]
    float deadZone = 0.0f;
    [SerializeField]
    float gravityMod = 0.0f;
    [SerializeField, Range(0, 10)]
    float platformTimer = 0.0f;
    [SerializeField]
    float timer = 0.0f;
    [SerializeField]
    SpringJoint mySJ;
    [SerializeField]
    Vector3 antiGravForce;
    Vector3 startPoint;
    Vector3 bottomPoint;


    bool startTimer;


    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        startPoint = transform.position;
        bottomPoint = Vector3.down * bottomPointOffset;
        mySJ = GetComponent<SpringJoint>();
        timer = platformTimer;
    }

    // Update is called once per frame
    void Update()
    {
        //Float();
        Timer();
    }

    private void OnCollisionEnter(Collision c)
    { 
        if (c.gameObject.tag == "Player")
        {
            startTimer = true;
        }
    }

    void Timer()
    {
        if(startTimer && timer > 0)
        {
            timer -= Time.deltaTime;
        }
        if(timer <= 0)
        {
            mySJ.breakForce = 0.01f;
        }
    }

    void Float()
    {
        //if (antiGravForce.magnitude < Physics.gravity.magnitude)
        //{
        //    gravityMod = (startPoint - transform.position).magnitude / bottomPointOffset;
        //    antiGravForce = rb.mass * -Physics.gravity;
        //    rb.AddForce(antiGravForce, ForceMode.Acceleration);
        //}
        //rb.AddForce(-Physics.gravity, ForceMode.Acceleration);
    }



}
