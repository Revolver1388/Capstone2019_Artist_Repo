//Written by Mike Elkin 06/21/2019
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Mike's Scripts/Save_Trigger", 15)]

public class Save_Trigger : MonoBehaviour
{  

    private void OnTriggerEnter(Collider o)
    {
        if (o.gameObject.tag == "Player")
        {
            // Enter Save Function Here
            Debug.Log("Player Saved");
        }
    }
}
