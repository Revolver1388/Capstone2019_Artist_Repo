//Written By Michael Elkin 05/06/2019
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//[ExecuteInEditMode]
[AddComponentMenu("Mike's Scripts/Bounce_Up",3)]
public class Bounce_Up : MonoBehaviour
{
    [SerializeField, Range(1, 300)]
    float bounceBoost = 1.0f;
    [SerializeField, Range(1, 10)]
    float bounce = 1.0f;
    [SerializeField]
    GameObject playerRef;




    private void Awake()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
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
    }


    private void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag == "Player")
        {
            c.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            c.gameObject.GetComponent<Rigidbody>().AddForce(transform.up * bounce, ForceMode.Impulse);
        }
        if(c.gameObject.tag == "Player" && Input.GetButton("Jump"))
        {
            c.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            c.gameObject.GetComponent<Rigidbody>().AddForce(transform.up * bounceBoost, ForceMode.Impulse);
        }
    }
    //private void OnTriggerEnter(Collider o)
    //{
    //    if (o.gameObject.tag == "Player" && Input.GetButton("Jump"))
    //    {
    //        playerRef.GetComponent<Rigidbody>().velocity = Vector3.zero;
    //        playerRef.GetComponent<Rigidbody>().AddForce((playerRef.transform.position - transform.position).normalized * bounceBoost, ForceMode.Force);
    //    }
    //}
}
