using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 

//this is an example script template. 

public class Collectable : MonoBehaviour, Interact {

    public BirdIdleTest bird;
    public Material blue;
    private Color color;

    private bool canCollect = false;
    private bool carried = false;
    
    public Transform perch;


	// Use this for initialization
	void Start () {


        // color = blue.color; 
        color = Color.gray;
        blue.color = color;

        bird = GameObject.FindWithTag("Bird").GetComponent<BirdIdleTest>();
		
	}

    void Update()
    {
        if (Input.GetButtonDown("LeftBumper"))
        {
            if (canCollect)
            {
                bird.SetFetchState();
                Debug.Log("send bird");
            }
            else if(!canCollect && bird.target == perch)
            {
                bird.SetFollowState();
                Debug.Log("bring bird");
                if(transform.parent != null)
                {
                    transform.parent = null;
                }
            }
        }

        if(Vector3.Distance(bird.transform.position, perch.position) < 0.1)
        {
            if (carried == false)
            {
                transform.parent = bird.transform;
                carried = true;
            }
            bird.SetFollowState();
        }

    }

    
    void Interact.InteractWithMe()
    {
        Debug.Log("You can see the collectable");
        //bird.destination = transform.position;
        blue.color = Color.red;
        canCollect = true;
        bird.target = perch;
    }

    public void DontInteractWithMe()
    {
        Debug.Log("You can't see the collectable");
        //bird.destination = Vector3.zero;
        //bird.canPickTarget = true; 
        blue.color = color;
        canCollect = false;
        
    }







}
