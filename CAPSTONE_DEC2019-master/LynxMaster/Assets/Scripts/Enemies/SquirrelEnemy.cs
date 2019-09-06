// Squirrel Enemy
// Created by Brianna Stone 5/18/2019

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SquirrelEnemy : MonoBehaviour
{
    public int maxHP = 3;
    private int currentHP;

    public int unitSphere;
    int throwRange = 12;
    int lungeRange = 4;
    int lookSpeed = 10;

    Vector3 startPos;
    Vector3 newPos;
    NavMeshAgent agent;

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();

        startPos = transform.position;

        currentHP = maxHP;

        NextPoint();
    }

    // Update is called once per frame
    void Update()
    {
        // Reset enemy path after each path has finished
        if (!agent.pathPending && agent.remainingDistance < 0.2f)
        {
            NextPoint();
        }

        if (Vector3.Distance(transform.position, player.transform.position) <= throwRange && Vector3.Distance(transform.position, player.transform.position) > lungeRange)
        {
            agent.destination = player.transform.position;
            agent.stoppingDistance = throwRange - 2;

            var rotation = Quaternion.LookRotation(player.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, lookSpeed * Time.deltaTime);

            Throw();
        }

        if (Vector3.Distance(transform.position, player.transform.position) > throwRange)
        {          
            NextPoint();
        }

        if (Vector3.Distance(transform.position, player.transform.position) <= lungeRange)
        {
            var rotation = Quaternion.LookRotation(player.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, lookSpeed * Time.deltaTime);

            Lunge();
        }
    }

    public void NextPoint()
    {
        newPos = startPos + Random.insideUnitSphere * unitSphere;
        agent.destination = newPos;
    }

    public void Throw()
    {
        GetComponent<ProjectileShoot>().ShootProjectile();
    }

    public void Lunge()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 6 * Time.deltaTime);
    }

    public void KillEnemy()
    {
        currentHP--;

        if (currentHP <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            ///Damage player
        }
    }
}
