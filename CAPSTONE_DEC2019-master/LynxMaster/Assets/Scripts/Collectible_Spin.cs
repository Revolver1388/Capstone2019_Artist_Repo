using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Mike's Scripts/Collectible_Spin", 7)]

public class Collectible_Spin : MonoBehaviour
{
    #region Variables
    [SerializeField]
    ParticleSystem myPS;// Collectible Particle System Reference  
    [SerializeField, Range(0, 90)]
    float rateOfOrbRotation = 1.0f;// set rotation speed of orb
    [SerializeField, Range(0, 90)]
    float offset = 1.0f;// sets distance of point to rotate around
    [SerializeField, Range(0, 10)]
    float moveOffset = 0.0f;// sets distance of vertical movement
    [SerializeField, Range(0, 10)]
    float moveMod = 0.0f;// sets vertical movement rate
    [SerializeField, Range(0, 360)]
    float rateOfParticleXRotation = 1.0f;//used to set rotX
    [SerializeField, Range(0, 360)]
    float rateOfParticleYRotation = 1.0f;//used to set rotY
    [SerializeField, Range(0, 360)]
    float rateOfParticleZRotation = 1.0f;//used to set rotZ
    float rotX = 0.0f;// Sets X rotation in rotationVector
    float rotY = 0.0f;// Sets Y rotation in rotationVector
    float rotZ = 0.0f;// Sets Z rotation in rotationVector
    Vector3 rotationVector;// sets rotational movement of Particle System
    float yStart = 0.0f;//used to store the starting Y world position
    bool movingUp;//used to switch direction of vertical movement
    #endregion
    void Awake()
    {
        myPS = GetComponentInChildren<ParticleSystem>();
        yStart = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        PSRotation();
        RotateAround();
    }

    private void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag == "Player")
        {            
            gameObject.SetActive(false);
        }
    }

    void PSRotation()//Rotates the Particle System
    {        
        rotX += rateOfParticleXRotation * Time.deltaTime;
        rotY += rateOfParticleYRotation * Time.deltaTime;
        rotZ += rateOfParticleZRotation * Time.deltaTime;
        rotationVector.x = rotX;
        rotationVector.y = rotY;
        rotationVector.z = rotZ;
        myPS.transform.eulerAngles = rotationVector;
    }
    
    void RotateAround()//Rotates Orb
    {           
        transform.RotateAround((transform.position + transform.forward * offset), Vector3.up, rateOfOrbRotation);
        VerticalMovement();
    }
    
    void VerticalMovement()//Vertical Orb Movement
    {
        if (movingUp)
        {
            transform.position += Vector3.up * Time.deltaTime * moveMod;
            if (transform.position.y >= yStart + moveOffset)
            {
                movingUp = false;
            }
        }
        else
        {
            transform.position += Vector3.down * Time.deltaTime * moveMod;
            if (transform.position.y < yStart - moveOffset)
            {
                movingUp = true;
            }
        }
    }

}
