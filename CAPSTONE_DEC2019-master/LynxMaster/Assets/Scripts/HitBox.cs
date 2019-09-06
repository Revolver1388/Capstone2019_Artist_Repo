using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public PlayerMovementv2 h_PlayerScript;
    public Vector3 spawnPoint;
    public int h_Health;

    private int h_FullHealth;

    void Start()
    {
        h_FullHealth = h_Health;
    }

    void Update()
    {
        Dead();
    }

    public void TakeDamage(int dmg)
    {
        h_Health -= dmg;
    }

    public int GetHealth()
    {
        return h_Health;
    }

    void Dead()
    {
        if (h_Health < 0)
        {
            gameObject.transform.position = spawnPoint;
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            h_Health = h_FullHealth;
        }
    }

}
