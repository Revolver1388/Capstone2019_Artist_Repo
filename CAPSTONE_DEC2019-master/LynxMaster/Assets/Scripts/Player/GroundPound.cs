// Sebastian Borkowski

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPound : MonoBehaviour
{
   
    // Simply change the name on line 29 and add the enemies to the tag and all "should" work.

    // Note: must add desired enemies to GroundedEnemy tag in order for the GroundPound to work!

    public float DropForce; // Force of the downward force.
    public float GroundPoundRadius; // ground pound radius of attack.
    public float ForwardForce; // Force of the forward movment in the air.

    public bool isPouncing;

    public GameObject Player; // player object.
    public GameObject[] Enemies; // (Enemy must have a RigidBody!!)
    
    bool IsGrounded = false; // bool for ground.


    void Start()
    {
        if (Enemies == null)
        {
            Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        }
    }


    void Update()
    {

        if (!IsGrounded)
        {
            if (Input.GetButtonDown("Dive")) // Ground Pound
            {
                gameObject.GetComponent<Rigidbody>().AddForce(transform.up * -DropForce, ForceMode.Impulse); // Force Down
                gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * ForwardForce, ForceMode.Impulse); // Force Forward
                Debug.Log("Pounce !");
                isPouncing = true;
            }
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            if (isPouncing)
            {
                foreach (GameObject Enemy in Enemies)
                {
                    if ((Vector3.Distance(Enemy.transform.position, Player.transform.position) < GroundPoundRadius)) // Radius check
                    {
                        Debug.Log("Hit!");

                        // Insert Damage script here.
                    }
                }
            }

            IsGrounded = true;
            isPouncing = false;
            
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            IsGrounded = false;
        }
    } 
}
