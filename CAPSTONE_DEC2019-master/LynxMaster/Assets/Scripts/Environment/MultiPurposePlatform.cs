//Written by Mike Elkin 07/10/2019
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Mike's Scripts/MultiPurposePlatform", 17)]
//[ExecuteInEditMode]
public class MultiPurposePlatform : MonoBehaviour
{
    //Platform Types
    enum PlatformType { Linear = 1, Radial, Spinning }
    [SerializeField, Header("Movement Type")]
    PlatformType type;

    [SerializeField, Header("Enable Live Editing")]
    bool liveEdit = false;// Toggles Ability to Edit Variables While Engine Is Running
    [SerializeField, Header("Platform Wait Time"), Range(0,5)]
    float movDelay = 0;
    // Player Reference
    GameObject Player;// Used For Attaching To Platform
    
    // Target Position Variables
    [Range(-100, 100), SerializeField, Header("End Position / Rotation Point")]
    float xDistance = 0.0f;// Desired Position X Mod Variable

    [Range(-100, 100), SerializeField]
    float yDistance = 0.0f;// Desired Position Y Mod Variable

    [Range(-100, 100), SerializeField]
    float zDistance = 0.0f;// Desired Position Z Mod Variable

    [SerializeField, Header("Linear or Radial Movement Repeatable") ]
    bool repeatable = false;

    [SerializeField, Range(1, 4), Header("Slerp Direction Selection")]
    int slerpDirection = 1;

    [SerializeField, Range(1, 4), Header("Spin Direction")]
    int axisSpinDirection = 1;

    [SerializeField, Range(1, 4), Header("Rotate Around Direction")]
    int rotateAroundDirection = 1;

    // Movement variables   
    Vector3 startPosition;//Return Point for linear movement
    Vector3 desiredPosition;//Final Destination for linear movement
    Vector3 target;//Target Destination for linear movement 
    Vector3 desiredAxis;// Holds Chosen Axis Of Spin
    Vector3 desiredAxisAround;
    float numeratorOfMovement = 0.0f;// Numerator of LERP & SLERP Movement
    [SerializeField, Range(1, 1000), Header("Linear & Radial Move Mod")]
    float denominatorOfMovement = 1000.0f;// Denominator of LERP & SLERP Movement
    bool goingForward = true;// Movement Direction Bool

    
    //Radial Movement Variables
    Vector3 centerPoint;// Point Between Start Position and Desrired Position
    Vector3 startRelativeToCenter;// Start Position Relative To Center
    Vector3 desiredRelativeToCenter;// Desired Position Relative To Center
    
    // Spinning Platfrom exclusive Variables
    [SerializeField, Range(1, 100), Header("Spinning Type Speed Variables")]
    float speedAroundDesiredPosition = 1.0f;// Rotation Speed Around Desired Position
    [SerializeField, Range(1, 100)]
    float speedAroundOwnAxis = 1.0f;// Rotation Around Own Y Axis
    // Use this for initialization
    void Start()
    {
        Startup();
    }
    // Update is called once per frame
    void Update()
    {
        if (liveEdit)
        {
            ResetPosition();            
        }
        else
        {
            PlatformRun();
        }
    }
    private void OnTriggerEnter(Collider o)
    {
        if (o.gameObject.tag == "Player")
        {
            Player.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider o)
    {
        if (o.gameObject.tag == "Player")
        {
            Player.transform.parent = null;
        }
    }

    void LinearMovement()
    {
        if (repeatable)
        {            
            RepeatableLinearMovement();
        }
        else
        {
            SingleLinearMovement();
        }
    }
    void MoveWithRadius()
    {
        if (repeatable)
        {
            RepeatableRadialMovement();
        }
        else
        {
            SingleRadialMovement();
        }
    }
    void GetCenter()
    {
        switch (slerpDirection)
        {
            case 1:
                centerPoint = (startPosition + desiredPosition) * .5f;
                centerPoint -= Vector3.up;
                startRelativeToCenter = startPosition - centerPoint;
                desiredRelativeToCenter = desiredPosition - centerPoint;

                break;
            case 2:
                centerPoint = (startPosition + desiredPosition) * .5f;
                centerPoint -= Vector3.right;
                startRelativeToCenter = startPosition - centerPoint;
                desiredRelativeToCenter = desiredPosition - centerPoint;
                break;
            case 3:
                centerPoint = (startPosition + desiredPosition) * .5f;
                centerPoint -= Vector3.down;
                startRelativeToCenter = startPosition - centerPoint;
                desiredRelativeToCenter = desiredPosition - centerPoint;
                break;
            case 4:
                centerPoint = (startPosition + desiredPosition) * .5f;
                centerPoint -= Vector3.left;
                startRelativeToCenter = startPosition - centerPoint;
                desiredRelativeToCenter = desiredPosition - centerPoint;
                break;

        }
    }
    void SingleRadialMovement()
    {
        if (numeratorOfMovement <= denominatorOfMovement)
        {
            transform.position = Vector3.Slerp(startRelativeToCenter, desiredRelativeToCenter, numeratorOfMovement / denominatorOfMovement);
            transform.position += centerPoint;
            numeratorOfMovement++;
        }
    }
    void RepeatableRadialMovement()
    {
        if (numeratorOfMovement <= denominatorOfMovement && goingForward)
        {
            transform.position = Vector3.Slerp(startRelativeToCenter, desiredRelativeToCenter, numeratorOfMovement / denominatorOfMovement);
            transform.position += centerPoint;
            numeratorOfMovement++;
            if(numeratorOfMovement > denominatorOfMovement)
            {
                StartCoroutine(WaitForMovement(movDelay));                                
            }
        }
        if (numeratorOfMovement >= 0 && !goingForward)
        {
            transform.position = Vector3.Slerp(startRelativeToCenter, desiredRelativeToCenter, numeratorOfMovement / denominatorOfMovement);
            transform.position += centerPoint;
            numeratorOfMovement--;
            if (numeratorOfMovement < 0)
            {
                StartCoroutine(WaitForMovement(movDelay));                                
            }
        }
    }
    void SingleLinearMovement()
    {
        if (numeratorOfMovement <= denominatorOfMovement)
        {
            transform.position = Vector3.Lerp(startPosition, desiredPosition, numeratorOfMovement / denominatorOfMovement);
            numeratorOfMovement++;
        }

    }
    void RepeatableLinearMovement()
    {
        if (numeratorOfMovement <= denominatorOfMovement && goingForward)
        {
            transform.position = Vector3.Lerp(startPosition, desiredPosition, numeratorOfMovement / denominatorOfMovement);
            numeratorOfMovement++;
            if(numeratorOfMovement > denominatorOfMovement)
            {
                StartCoroutine(WaitForMovement(movDelay));                                
            }
        }
        if (numeratorOfMovement >= 0 && !goingForward)
        {
            transform.position = Vector3.Lerp(startPosition, desiredPosition, numeratorOfMovement / denominatorOfMovement);
            numeratorOfMovement--;
            if (numeratorOfMovement < 0)
            {
                StartCoroutine(WaitForMovement(movDelay));                                
            }
        }
    }
    void SpinPlatform()
    {
        switch(axisSpinDirection)
        {
            case 1:
                desiredAxis = Vector3.up;
                break;
            case 2:
                desiredAxis = Vector3.left;
                break;
            case 3:
                desiredAxis = Vector3.down;
                break;
            case 4:
                desiredAxis = Vector3.right;
                break;
        }
        switch(rotateAroundDirection)
        {
            case 1:
                desiredAxisAround = Vector3.up;
                break;
            case 2:
                desiredAxisAround = Vector3.left;
                break;
            case 3:
                desiredAxisAround = Vector3.down;
                break;
            case 4:
                desiredAxisAround = Vector3.right;
                break;

        }
        transform.Rotate(desiredAxis, 5.0f * Time.deltaTime * speedAroundOwnAxis);
        transform.RotateAround(desiredPosition, desiredAxisAround, 5.0f * Time.deltaTime * speedAroundDesiredPosition);
    }
    void ResetPosition()
    {
        switch (type)
        {
            case PlatformType.Linear:                               
                desiredPosition.Set(transform.position.x + xDistance, transform.position.y + yDistance, transform.position.z + zDistance);
                transform.position = startPosition;
                numeratorOfMovement = 0.0f;
                break;
            case PlatformType.Radial:                              
                desiredPosition.Set(transform.position.x + xDistance, transform.position.y + yDistance, transform.position.z + zDistance);
                transform.position = startPosition;
                numeratorOfMovement = 0.0f;
                break;
            case PlatformType.Spinning:
                desiredPosition.Set(startPosition.x + xDistance, startPosition.y + yDistance, startPosition.z + zDistance);
                transform.position = startPosition;
                numeratorOfMovement = 0.0f;
                break;
        }
    }
    void PlatformRun()
    {
        switch (type)
        {
            case PlatformType.Linear:               
                LinearMovement();
                break;
            case PlatformType.Radial:                
                GetCenter();
                MoveWithRadius();
                break;
            case PlatformType.Spinning:
                SpinPlatform();
                break;
        }
    }
    void Startup()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        desiredPosition.x = xDistance + transform.position.x;
        desiredPosition.y = yDistance + transform.position.y;
        desiredPosition.z = zDistance + transform.position.z;
        startPosition = transform.position;
        numeratorOfMovement = 0.0f;
        goingForward = true;
    }
    IEnumerator WaitForMovement(float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        goingForward = !goingForward;
        StopCoroutine(WaitForMovement(movDelay));
    }
    void OnDrawGizmosSelected()
    {
        switch(type)
        {
            case PlatformType.Linear:
                Gizmos.color = Color.green;
                Gizmos.DrawCube(desiredPosition, transform.lossyScale);
                break;
            case PlatformType.Radial:
                Gizmos.color = Color.cyan;
                Gizmos.DrawCube(desiredPosition, transform.lossyScale);
                break;
            case PlatformType.Spinning:
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(desiredPosition, .5f);
                break;
        }        
    }
}
