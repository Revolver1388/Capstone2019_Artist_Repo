// Sebastian Borkowski

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public int Health; // integer of player health.
    
    public float seconds;

    public bool Invincible = false; // boolean for Invincible.

    public void OnTriggerEnter(Collider col) // Collision check for Enemies melee collider.
    {
        if(!Invincible)
        {
            if(col.gameObject.tag == "") // Will be for the enemy melee colliders.
            {
                StartCoroutine("Timer"); // Calls Ienumerable Timer.
            }
        }
    }

    IEnumerable Timer(float seconds)
    {
        Health -= 1; // Deducts player health by one point.
        Invincible = true; // Turns player Invincible for set amount of time.

        yield return seconds;

        Invincible = false; // Turn player vulnrable after set amount of time.
    }
}
