using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BirdPlatform : MonoBehaviour, Interact
{

    public BirdIdleTest bird;
    public Transform startPosition;
    public Transform finalLocation;

    public Transform perch; //need a child transform and add it here in inspector

    public float moveSpeed;
    public float maxDistance;

    public bool canMove;
    public bool move;

    // Start is called before the first frame update
    void Start()
    {
        bird = GameObject.FindWithTag("Bird").GetComponent<BirdIdleTest>();
        startPosition.transform.position = transform.position; // set the default location
        canMove = false;
        move = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("LeftBumper"))
        {
            if (canMove)
            {
                bird.SetFetchState();
                Debug.Log("send bird");
            }
            else
            {
                if (bird.target == perch)
                {
                    bird.SetFollowState();
                    Debug.Log("bring bird");
                }
            }
        }

        if (Vector3.Distance(bird.transform.position, perch.position) <= 0.1)
        {
            move = true;
        }
        else if (Vector3.Distance(bird.transform.position, perch.position) > 0.1)
        {
            move = false;
        }

        if (move)
        {
            Move();
        }
        else if (!move)
        {
            ResetPosition();
        }
    }

    void Interact.InteractWithMe()
    {
        //Debug.Log("You can see the platform");
        //bird.destination = transform.position;
        bird.target = perch;
        canMove = true;
    }

    public void DontInteractWithMe()
    {
        //Debug.Log("You can't see the platform");
        //bird.SetIdle();
        canMove = false;     
    }

    public void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, finalLocation.transform.position, moveSpeed * Time.deltaTime);
    }

    public void ResetPosition()
    {
        transform.position = Vector3.MoveTowards(transform.position, startPosition.transform.position, moveSpeed * Time.deltaTime);
    }

}
