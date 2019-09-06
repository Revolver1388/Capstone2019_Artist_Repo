//Written by Mike Elkin 06/21/2019
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[AddComponentMenu("Mike's Scripts/Boundry", 4)]
public class Boundry : MonoBehaviour
{

    [SerializeField]
    bool deathTrigger;
    [SerializeField, Range(1, 10)]
    float reboundForce;
    [SerializeField]
    Transform startPosition;



    private void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag == "Player")
        {
            if (deathTrigger)
            {
                c.transform.position = startPosition.position;// Need connection to Current Spawn Point
                Debug.Log("Player Hit Rock Bottom");
            }
            else if (!c.gameObject.GetComponent<PlayerMovementv2>().grounded)
            {
                //Vector3.
                c.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.Cross(c.transform.position, transform.position) * reboundForce, ForceMode.Impulse);
            }

        }
    }
}
