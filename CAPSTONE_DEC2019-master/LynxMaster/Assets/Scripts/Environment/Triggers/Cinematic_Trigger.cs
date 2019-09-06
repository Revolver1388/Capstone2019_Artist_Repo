//Written by Mike Elkin 06/24/2019
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[ExecuteInEditMode]
[AddComponentMenu("Mike's Scripts/Cinematic_Trigger", 5)]

public class Cinematic_Trigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider o)
    {
        if (o.gameObject.tag == "Player")
        {
            // Enter Cinematics Function Here
            Debug.Log("Cinematic Plays");
        }
    }
}
