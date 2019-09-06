
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShoot : MonoBehaviour
{
    public GameObject projectile;
    public GameObject projSpawn;

    public int pooledProjectiles = 20;
    List<GameObject> projectiles = new List<GameObject>();

    public int speed = 3;

	// Use this for initialization
	void Start ()
    {
        for (int i = 0; i < pooledProjectiles; i++)
        {
            GameObject obj = (GameObject)Instantiate(projectile);
            obj.SetActive(false);
            projectiles.Add(obj);
        }
	}

    public void ShootProjectile()
    {
        for (int i = 0; i < projectiles.Count; i++)
        {
            if (!projectiles[i].activeInHierarchy)
            {
                projectiles[i].transform.position = projSpawn.transform.position;
                projectiles[i].transform.rotation = projSpawn.transform.rotation;
                projectiles[i].SetActive(true);
                projectiles[i].GetComponent<Rigidbody>().AddForce(transform.forward * speed, ForceMode.Impulse);
                break;
            }
        }
    }
}
