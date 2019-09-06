using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float timing;
    public float maxDistanceDelta;
    public bool goingTo;
    public Vector3 displacement;
    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
        StartCoroutine(Shift());
    }

    private void FixedUpdate()
    {
        if(goingTo)
        {
            transform.position = Vector3.MoveTowards(transform.position, initialPosition + displacement, maxDistanceDelta);
        }
        else if(!goingTo)
        {
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, maxDistanceDelta);
        }
    }

    public IEnumerator Shift()
    {
        goingTo = true;
        yield return new WaitForSecondsRealtime(timing);
        goingTo = false;
        yield return new WaitForSecondsRealtime(timing);
        StartCoroutine(Shift());
    }

}
