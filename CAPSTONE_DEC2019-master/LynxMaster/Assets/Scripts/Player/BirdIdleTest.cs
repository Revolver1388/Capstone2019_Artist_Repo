//Created by Dylan LeClair, May 17/19

//Empty gameobject childed to the player must be created and tagged as "FollowPoint"
//Player gameobject in scene must be tagged as "Player"

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdIdleTest : MonoBehaviour
{
    private GameObject idleFollowPoint;
    private GameObject player;

    private readonly float speed = 6;


    //states
    public enum BirdState { follow, fetch, comeBack };
    public BirdState currentState = BirdState.follow;

    //for fetch
    public Transform target;

    private void Start()
    {
        idleFollowPoint = GameObject.FindGameObjectWithTag("FollowPoint");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        switch (currentState)
        {
            case BirdState.follow:
                FollowState();
                break;
            case BirdState.fetch:
                FetchState();
                break;
            case BirdState.comeBack:
                ComeBackState();
                break;
        }

        //if (target != null)
        //{
        //    //Debug.Log(target.name);
        //}
    }

    private void LookDirection()
    {
        transform.LookAt(player.transform.position);
    }

    private void Follow()
    {
        transform.position = Vector3.MoveTowards(transform.position, idleFollowPoint.transform.position, speed * Time.deltaTime);
    }

    public void SetIdle()
    {
        target = idleFollowPoint.transform;
    }

    public void SetFollowState()
    {
        currentState = BirdState.follow;
    }
    public void SetFetchState()
    {
        currentState = BirdState.fetch;
    }

    public void FollowState()
    {
        LookDirection();
        Follow();
    }

    public void FetchState()
    {
        if(target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }

    public void ComeBackState()
    {

    }
}
