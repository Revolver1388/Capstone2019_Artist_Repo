//Written by Mike Elkin 07/10/2019
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[ExecuteInEditMode]
[AddComponentMenu("Mike's Scripts/MultiPurposePlatform", 16)]

public class ObjectDistanceMeasure : MonoBehaviour
{
    enum ActionState { MarkArea, Measure}
    [SerializeField]
    ActionState state;
    [SerializeField]
    Transform object_1;
    [SerializeField]
    Transform object_2;
    [SerializeField]
    bool measure;
    float distance = 0;

    [SerializeField,Range(0,1000)]
    float sizeX = 0.0f;
    [SerializeField, Range(0, 1000)]
    float sizeY = 0.0f;
    [SerializeField, Range(0, 1000)]
    float sizeZ = 0.0f;

    Vector3 sizeVector;

    private void Update()
    {
        if (measure)
        {
            distance = CalcDistance();
            Debug.Log("Distance = " + distance);
        }
        if (sizeVector.x != sizeZ || sizeVector.y != sizeY || sizeVector.z != sizeZ)
        {

        }
    }

    float CalcDistance()
    {
        return Vector3.Distance(object_1.position, object_2.position);
    }

    private void OnDrawGizmos()
    {
        
        switch (state)
        {
            case ActionState.MarkArea:
                if (sizeVector.x != sizeZ || sizeVector.y != sizeY || sizeVector.z != sizeZ)
                {
                    sizeVector = new Vector3(sizeX, sizeY, sizeZ);
                }
                Gizmos.color = Color.blue;
                Gizmos.DrawWireCube(transform.position, sizeVector);
                break;
            case ActionState.Measure:
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(object_1.position, object_1.position + (Vector3.up * 10));
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(object_2.position, object_2.position + (Vector3.up * 10));
                break;

        }
    }
}
