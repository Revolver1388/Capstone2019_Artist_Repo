using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractComponent : MonoBehaviour {

    //important, put in the inspector
    public Transform visionCone;

    //put the gameobject with the main script (enemy, collectable, platform) here...
    public GameObject interactable;

    //...and as long as the script derives from interact, this will work
    Interact interactScript;


    private void Awake()
    {
        if(!visionCone)
        {
            GameObject coneObject = GameObject.FindWithTag("VisionCone");
            visionCone = coneObject.transform; 
        }


        if(!interactable)
        interactable = this.gameObject; 

        interactScript = interactable.GetComponent<Interact>();

        if (interactScript == null)
        {
            Debug.LogWarning("The main script does not derive from Interact"); //you should not see this
        }

    }


    //For when the object is within the player's vision cone;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == visionCone)
        {
            if (interactScript != null)
            {
                Debug.Log("Object " + this.gameObject.name + " can be interacted with");
                interactScript.InteractWithMe();
            }
        }
    }

    //For when the object is out of the the player's vison cone
    private void OnTriggerExit(Collider other)
    {
        if (other.transform == visionCone)
        {

            if (interactScript != null)
            {
                Debug.Log("Object " + this.gameObject.name + " can no longer be interacted with");
                interactScript.DontInteractWithMe();
            }
        }
    }

}
