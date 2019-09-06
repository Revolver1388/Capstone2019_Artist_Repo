// Sebastian Borkowski

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacks : MonoBehaviour
{
    // Place script in the player, Create two box colliders for the hands of the player.
    // Add Colliders to the corisponding BoxCollider.

    // When we have animations I will make the code work with the Melee Animation.

    //public Animator PlayerAnimator;
    public float DashSpeed; // Speed of the dash.
    public BoxCollider RightHand; // Collider on the right hand.
    public BoxCollider LeftHand; // Collider on the left hand.

    // Use this for initialization
    void Start()
    {

        RightHand.enabled = false; 
        LeftHand.enabled = false;

    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown("q")) // Key for the Dash.
        {
            gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * DashSpeed, ForceMode.Impulse); // Adds force forwards.
        }

        if (Input.GetKeyDown("e")) // Key for the Melee.
        {
            StartAttack(); // Enables the colliders.
            Debug.Log("Pressed");
        }
    }

    void StartAttack() // Will be used when the Melee animation plays.
    {
        RightHand.enabled = true;
        LeftHand.enabled = true;
    }

    void EndAttack() // Will be used when the Melee animation ends.
    {
        RightHand.enabled = false;
        LeftHand.enabled = false;
    }
}
