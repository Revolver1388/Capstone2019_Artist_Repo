//Written By Michael Elkin 05/06/2019
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[ExecuteInEditMode]
[AddComponentMenu("Mike's Scripts/Falling_Obstacle",14)]
public class Falling_Obstacle : MonoBehaviour
{
    [SerializeField]
    Falling_Obstacle_Manager managerRef;

    [SerializeField]
    public Rigidbody rb;
    [SerializeField, Range(1, 10)]
    float attackForce = 10.0f;    
    [SerializeField, Range(1, 50)]
    float approachRange = 1.0f;
    [SerializeField, Range(1, 10)]
    float fallDelay = 1.0f;    
    [SerializeField]
    float playerDistance;
    [SerializeField]
    public bool functionStart;
    [SerializeField]
    float timeStart;
    [SerializeField]
    GameObject playerRef;
    [SerializeField]
    public MeshRenderer meshrend;
    [SerializeField]
    public CapsuleCollider collider;

    public Vector3 startPos;
    public Quaternion startRot;


    Collider[] targetColliders;


    bool falling;
    bool mySwitch;
    
    int moveCount = 0;
    

    private void Awake()
    {
        if (GetComponentInParent<Falling_Obstacle_Manager>())
        {
            managerRef = GetComponentInParent<Falling_Obstacle_Manager>();
        }
        else
        {
            Debug.Log("Falling_Obstacle_Manager Object is missing");
        }
        meshrend = GetComponent<MeshRenderer>();
        collider = GetComponent<CapsuleCollider>();
        startPos = transform.position;
        startRot = transform.rotation;
        playerRef = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerRef == null)
        {
            playerRef = GameObject.FindGameObjectWithTag("Player");
        }
        
        CheckForPlayer();
    }

    private void OnCollisionEnter(Collision c)
    {
        if(c.gameObject.tag == "Ground")
        {
            // beakapart animation here
            //Debug.Log("Breaks apart");
            //targetColliders = Physics.OverlapSphere(transform.position, smashRadius);
            //for (int i = 0; i < targetColliders.Length; i++)
            //{
            //    //()
            //}


            //managerRef.FlipSwitch();
            //meshrend.enabled = false;
            //collider.enabled = false;
            //rb.useGravity = false;
            //rb.velocity = Vector3.zero;
            gameObject.SetActive(false);
            
        }
        if (c.gameObject.tag == "Player")
        {
            c.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            c.gameObject.GetComponent<Rigidbody>().AddForce((transform.position - c.transform.position).normalized * attackForce, ForceMode.Impulse);
        }
    }

    void FindPlayerDistance()
    {
        playerDistance = (transform.position - playerRef.transform.position).magnitude;
    }    

    void CheckForPlayer()
    {
        FindPlayerDistance();
        if (playerDistance <= approachRange)
        {
            if(functionStart == false)
            {
                timeStart = Time.time;
                functionStart = true;
            }
            //fill in for animation
            if (mySwitch && !falling)
            {
                transform.Rotate(0, 0, 45 * Time.deltaTime);
                moveCount++;
                if (moveCount > 5)
                {
                    mySwitch = !mySwitch;
                    moveCount = 0;
                }
            }
            else if (!mySwitch && !falling)
            {
                transform.Rotate(0,0,-45 * Time.deltaTime);
                moveCount++;
                if (moveCount > 5)
                {
                    mySwitch = !mySwitch;
                    moveCount = 0;
                }
            }

            if (Time.time - timeStart >= fallDelay)// to be but at the end of an animation of a wiggle
            {
                rb.useGravity = true;
                falling = true;
                //rb.AddForce(Vector3.down * attackForce, ForceMode.Impulse);
            }
        }
    }

    



}
