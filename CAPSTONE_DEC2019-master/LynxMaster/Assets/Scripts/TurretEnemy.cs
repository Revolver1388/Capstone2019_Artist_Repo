//Written by Mike Elkin 07/10/2019
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Mike's Scripts/TurretEnemy", 21)]

public class TurretEnemy : MonoBehaviour
{
    GameObject playerRef;

    [SerializeField, Range(0, 30)]
    float checkRadius = 1.0f;

    [SerializeField]
    bool hasTarget;

    [SerializeField]
    GameObject projectilePrefab;

    [SerializeField, Range(1, 10)]
    int poolSize = 1;

    int projectilePoolIndex = 0;

    GameObject[] projectilePool;    

    LayerMask playerLayer;

    Collider[] targetArray;

    private void Awake()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        projectilePool = new GameObject[poolSize];
    }
    
    // Update is called once per frame
    void Update()
    {
        CheckForTargets();
    }

    void CheckForTargets()
    {
        if (!hasTarget)
        {
            targetArray = Physics.OverlapSphere(transform.position, checkRadius, playerLayer);
            if (targetArray.Length > 0 && targetArray[0].gameObject.tag == "Player")
            {
                playerRef = targetArray[0].gameObject;
                hasTarget = true;
            }

        }
    }

    void Attack()
    {
        if(playerRef != null && Vector3.Distance(transform.position, playerRef.transform.position) <= checkRadius)
        {

        }
    }

    void CreateProjedctilePool()
    {
        for (int i = 0; i < projectilePool.Length; i++)
        {
            projectilePool[i] = Instantiate(projectilePrefab);
            projectilePool[i].SetActive(false);
        }
    }

}
