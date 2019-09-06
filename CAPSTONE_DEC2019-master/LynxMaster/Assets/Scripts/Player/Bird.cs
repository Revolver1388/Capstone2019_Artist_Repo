using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Bird : MonoBehaviour
{

    public GameObject player;

    //where the bird is heading for in idle state
    public GameObject birdTarget;
    public bool isIdle;

    //how often the bird switches targets in idle state
    private float targetTimer;
    public bool canPickTarget;


    //isLooking
    //gameObject[] allCollectables


    [Header("List of CheckPoints. Populate in order!!!")]
    public List<GameObject> CheckPointList;

    //this tells the bird which checkpoint to go to 
    private GameObject currentCheckPoint;

    //super important, it keeps track of the player's position in the level
    public int playerCheckCount; 


    //AI stuff
    public float birdOffsetHeight;
    NavMeshAgent AIBird;


    //Swing stuff - now obsolete
    public bool canSwing; 
    public GameObject swingPoint;
    public bool shittyWayToDoThis;


    //Collecting
    public GameObject playerRadious; 
    public bool canCollect;
    private GameObject collectable;
    //public bool isTethered;
    public Queue <Vector3> collectQueue;

    //interact
    public Vector3 destination; 


    // Start is called before the first frame update
    void Start()
    {

        destination = Vector3.zero;
        collectQueue = new Queue<Vector3>();
        shittyWayToDoThis = true; 
        isIdle = true; 
        targetTimer = 0;
        canPickTarget = true;
        canSwing = false;

        //myEvent.AddListener(move)
        //allCollectables = new GameObject[] GameObject.FindObjectWithTheTag("Collectable")


        if(!player)
        {
            Debug.Log("You forgot to add a player");
        }
        else
        {
            //playerCheckCount = player.GetComponent<playerCheckPoint>().checkPointCount; 
        }


        if(GetComponent<NavMeshAgent>())
        {
            AIBird = GetComponent<NavMeshAgent>();
        }
        else
        {
            Debug.Log("You forgot to add a navmesh agent");
           
        }
    }

    // Update is called once per frame
    void Update()
    {
       

        //this is when the bird is in the idle state. It will first pick a target around the player (automatically, no need to set it up in the editor).
        PickBirdTarget();

        //After a randomly elapsed time the bird will pick another target. 
        TargetTimerReset();

        //Then it will follow the target. 
        MoveToRotatingWayPoint();

        //Right now this tracks the isIdle state so that the bird can move to it's guidence system. Orginally the stuff below was in it, 
        //but it wasn't working as intended. 
        //MoveToNextCheckPoint();

        //On trigger with Player's BirdRangeTrigger collider, the bird automatically heads to the target object and picks it up 
        //FindStuff();

        //Temporary way for the bird to fly to a child of the player (swing target), which the player swings from. Now obsolete.
        //MoveToSwingPoint();


        if (Input.GetButtonDown("LeftBumper"))
        {
            //if(collectable.GetComponent<Collectable>().isTethered)
            //{
            //    collectable.GetComponent<Collectable>().isTethered = false;
            //}
            //else{
            //isIdle = false;
            //}

            if(destination != Vector3.zero)
            {
                moveTo();
            }
        }
     



       
    }

    private void FixedUpdate()
    {
        AIBird.speed = player.GetComponent<Rigidbody>().velocity.magnitude + 5;

    }


    void TargetTimerReset()
    {
        if(!canPickTarget)
        {
            int randomTargetTime = Random.Range(5, 10);
            targetTimer += Time.deltaTime;
            if(targetTimer > randomTargetTime)
            {
                canPickTarget = true;
                targetTimer = 0; 
            }
            
        }

    }


    void PickBirdTarget()
    {
        if(canPickTarget)
        {
            GameObject[] birdTargetArray = GameObject.FindGameObjectsWithTag("BirdTarget");
            int birdTargetChoice = Random.Range(0, birdTargetArray.Length);
            birdTarget = birdTargetArray[birdTargetChoice];
            canPickTarget = false; 


         }
    }

    void MoveToRotatingWayPoint()
    {
        if(isIdle)
        {
        birdOffsetHeight = birdTarget.transform.position.y;
        AIBird.baseOffset = birdOffsetHeight * 5;
        AIBird.SetDestination(birdTarget.transform.position);
        }



    }

          
    

    public void moveTo()
    {
        Debug.Log("Moving to place");
        isIdle = false;
        AIBird.SetDestination(destination);
        
    }


}


    
