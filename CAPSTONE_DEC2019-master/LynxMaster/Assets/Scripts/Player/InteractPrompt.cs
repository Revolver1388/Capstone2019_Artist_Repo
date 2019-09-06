using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractPrompt : MonoBehaviour
{
    //TODO: Currently seems to load text backwards and low. Next Update will fix.

    public GameObject prompt;
    
    //public GameObject followingObject;

    public float rayDistance = 10;

    public GameObject player;

    bool instance = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //poopy.x = followingObject.transform.position.x;
        //poopy.y = transform.position.y;
        //poopy.z = followingObject.transform.position.z;
        //gameObject.transform.LookAt(poopy);

        
        transform.LookAt(player.transform);

        //transform.rotation = Quaternion.LookRotation(-mainCamera.transform.position);

        // Bit shift the index of the layer (9) to get a bit mask (Change to whatever layer the player is set to)
        int layerMask = 1 << 10;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, rayDistance, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            //Debug.Log("Did Hit");
            if(instance == false)
            {
                Debug.Log("<color=red>SetPrompActive</color>");
                prompt.SetActive(true);
                instance = true;
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 2, Color.white);
            //Debug.Log("Did not Hit");
            prompt.SetActive(false);
            instance = false;
        }


    }
   
    
}
