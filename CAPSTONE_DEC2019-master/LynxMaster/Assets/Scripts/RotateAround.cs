using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour {

    public GameObject player;
    public Vector3 relativeAxis;

    public GameObject projectile;

    public float RotateSpeed = 80f;

    private float spinTimer = 10f;

    Vector3 xAxis;
    Vector3 yAxis;
    

	// Use this for initialization
	void Start () {
        //ProjectileFunction();
        //xAxis = new Vector3(-relativeAxis.y, relativeAxis.x, 0);
        //xAxis.Normalize();
        //yAxis = Vector3.Cross(xAxis, relativeAxis);
        //yAxis.Normalize();
    }
	
	// Update is called once per frame
	void Update () {

       
            SpinFunction();
        
        //transform.localPosition = Mathf.Cos(RotateSpeed * Mathf.Deg2Rad * Time.time) * xAxis + Mathf.Sin(RotateSpeed * Mathf.Deg2Rad * Time.time) * yAxis;
    }

    void ProjectileFunction ()
    {
        //GameObject spawnedProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
    }

    void SpinFunction()
    {
        //spinTimer -= Time.deltaTime;

        //if (spinTimer <= 0)
        //{
        //    transform.position = new Vector3(relativeAxis.x, relativeAxis.y, relativeAxis.z);
        //    spinTimer = 10;
        //}

        Vector3 worldAxis = relativeAxis.x * player.transform.right + relativeAxis.y * player.transform.up + relativeAxis.z * player.transform.forward;
        transform.RotateAround(player.transform.position, worldAxis, RotateSpeed * Time.deltaTime);




    }
}
