//Created by Dylan LeClair May, 17/19

//Empty gameobject childed to the player must be created and tagged as "FollowPoint"

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdIdleFollowPointTest : MonoBehaviour
{
    private float waitTimer = 1.5f;
    private Vector3 pointPosition;
    private GameObject followPoint;

    private void Awake()
    {
        followPoint = GameObject.FindGameObjectWithTag("FollowPoint");
    }

    private void Start()
    {
        pointPosition = new Vector3(Random.Range(-1.2f, 1.2f), Random.Range(1f, 1.5f), Random.Range(-1.2f, -2f));
        followPoint.transform.localPosition = pointPosition;

        StartCoroutine(ChooseNewPosition());
    }

    private IEnumerator ChooseNewPosition()
    {
        yield return new WaitForSecondsRealtime(waitTimer);

        pointPosition = new Vector3(Random.Range(-1.2f, 1.2f), Random.Range(1f, 1.5f), Random.Range(-1.2f, -2f));
        followPoint.transform.localPosition = pointPosition;

        StartCoroutine(ChooseNewPosition());
    }
}
