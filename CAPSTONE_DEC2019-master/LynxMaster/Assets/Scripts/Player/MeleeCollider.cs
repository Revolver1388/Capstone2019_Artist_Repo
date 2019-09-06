// Sebastian Borkowski

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCollider : MonoBehaviour
{
    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy") // Collison check with enemies.
        {
            Debug.Log("Melee Hit"); //When available will call enemy hit script.
        }
    }
}
