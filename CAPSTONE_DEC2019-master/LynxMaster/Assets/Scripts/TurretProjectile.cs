//Written by Mike Elkin 07/10/2019
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[ExecuteInEditMode]
[AddComponentMenu("Mike's Scripts/TurretProjectile", 20)]

public class TurretProjectile : MonoBehaviour
{

    [SerializeField, Range(1, 50)]
    float _projVelocity = 1.0f;
    public float projVelocity
    {
        get { return _projVelocity; }
        private set { _projVelocity = value; }
    }

    [SerializeField, Range(1, 50)]
    float _detForce = 1.0f;
    public float detForce
    {
        get { return _detForce; }
        private set { _detForce = value; }
    }
    [SerializeField, Range(1, 50)]
    float _detRadius = 1.0f;
    public float detRadius
    {
        get { return _detRadius; }
        private set { _detRadius     = value; }
    }


    private void OnCollisionEnter(Collision c)
    {
        if(c.gameObject.tag == "Player")
        {
            c.gameObject.GetComponent<Rigidbody>().AddExplosionForce(detForce, transform.position, detRadius);
            gameObject.SetActive(false);
        }
    }





}
