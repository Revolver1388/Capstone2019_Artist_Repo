using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingWayPoint : MonoBehaviour, Interact
{
    public float circleSpeed;
    public Transform birdTarget;
    private Vector3 distanceFromTarget;

    public Vector3 relativeAxis;

    Vector3 xAxis;
    Vector3 yAxis;


    // Start is called before the first frame update
    void Start()
    {
        distanceFromTarget = transform.position - birdTarget.position;
    }

    // Update is called once per frame
    void Update()
    {


        //Vector3 newDistanceFromTarget = transform.position - birdTarget.position;

        //if(distanceFromTarget.magnitude + 1f < newDistanceFromTarget.magnitude)
        //{
        //    float flutter = circleSpeed * Time.deltaTime;
        //    transform.position = Vector3.MoveTowards(transform.position, distanceFromTarget, flutter);
        //}

        transform.Rotate(Vector3.right, Space.Self);


        Vector3 worldAxis = relativeAxis.x * birdTarget.right + relativeAxis.y * birdTarget.up + relativeAxis.z * birdTarget.forward;
        transform.RotateAround(birdTarget.position, worldAxis, circleSpeed * Time.deltaTime);
            
        //Debug.Log(newDistanceFromTarget.magnitude);



    }

    private void LateUpdate()
    {
        int rando = Random.Range(0, 1);
        float fRando = rando * 1f; 
        relativeAxis.z = fRando;
        //Debug.Log(fRando);

    }

    public void InteractWithMe()
    {
        
        throw new System.NotImplementedException();
    }

    public void DontInteractWithMe()
    {
        throw new System.NotImplementedException();
    }
}
