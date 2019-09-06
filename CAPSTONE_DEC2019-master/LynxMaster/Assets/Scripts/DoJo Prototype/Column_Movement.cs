// Written by Mike Elkin 06-26-2019
//edited by Mike Elkin 06-28-2019
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[ExecuteInEditMode]
[AddComponentMenu("Mike's Scripts/Column_Movement", 7)]

public class Column_Movement : MonoBehaviour
{
    enum Column_Movement_Cycle { AwaitInput, MoveColumn}
    [SerializeField]
    Column_Movement_Cycle cycleState;
    
    Column_Movement_Manager column_Movement_Manager;
    [SerializeField]
    Transform spawnPoint;// Spawn Location For Collectibles.  Add In Inspector.
    
    Vector3 newLocalPosition;// New Location For Column To Move To
   
    Vector3 currentPosition;// Columns Current Location
    
    float fractionComplete = 0;// Numerator Of Lerp Movement Fraction

    [SerializeField, Range(1,1000)]
    float scaleNum = 1000;// Denominator Of Lerp Movement Fraction
    
    Vector3 startPosition;

    // Local Position adjustment offsets
    Vector3 height4_35UU;// 4.35 unity units offset
    Vector3 height7_0UU;// 7.0 unity units offset
    Vector3 height11_35UU;// 11.35 unity units offset
    Vector3 maxHeight;// 14.0 unity units offset

    bool hasNewInput;// Triggers Move Cycle for Column
    bool canSpawnHere;// Spawn Point Active Switch
    


    private void Awake()
    {
        height4_35UU = new Vector3(0, -4.35f, 0);
        height7_0UU = new Vector3(0, -7.0f, 0);
        height11_35UU = new Vector3(0, -11.35f, 0);
        maxHeight = new Vector3(0, -14.0f, 0);
        cycleState = Column_Movement_Cycle.AwaitInput;
        startPosition = this.transform.position;
        currentPosition = startPosition;
        column_Movement_Manager = GetComponentInParent<Column_Movement_Manager>();
    }


    private void Update()
    {
        switch (cycleState)
        {
            case Column_Movement_Cycle.AwaitInput:
                if (hasNewInput)
                {
                    cycleState = Column_Movement_Cycle.MoveColumn;
                }

                break;            
            case Column_Movement_Cycle.MoveColumn:
                MoveColumn();
                if (transform.position == newLocalPosition)
                {
                    hasNewInput = false;                    
                    fractionComplete = 0.0f;
                    currentPosition = transform.position;
                    cycleState = Column_Movement_Cycle.AwaitInput;                    
                }
                break;
        }
    }

    private void OnTriggerEnter(Collider o)
    {
        if (o.gameObject.tag == "Player")
        {
            o.transform.parent = transform;
        }

    }

    private void OnTriggerExit(Collider o)
    {
        if (o.gameObject.tag == "Player")
        {
            o.transform.parent = null;
        }
    }

    public void SetColumnParameters(bool newInput, int positionNum, bool spawnHere)// Set Column Movement Info
    {
        SetNewLocalPosition(positionNum);
        canSpawnHere = spawnHere;
        hasNewInput = newInput;
    }

    void SetNewLocalPosition(int positionNum)// Set Columns New Local Position
    {
        switch(positionNum)
        {
            case 1:
                newLocalPosition = startPosition;
                break;
            case 2:
                newLocalPosition = startPosition - height4_35UU;
                break;
            case 3:
                newLocalPosition = startPosition - height7_0UU;
                break;
            case 4:
                newLocalPosition = startPosition - height11_35UU;
                break;
            case 5:
                newLocalPosition = startPosition - maxHeight;
                break;
        }
    }    

    void MoveColumn()// Column Movement Function
    {             
        if(fractionComplete <= scaleNum)
        {
            transform.position = Vector3.Lerp(currentPosition, newLocalPosition, fractionComplete / scaleNum);
            fractionComplete++;
        }       
    }

    public Vector3 GetSpawnPointPos()// Access To Spawn Point
    {
        return spawnPoint.position;
    }

    public Vector3 GetNewLocalPosition()// Access To New Local Position
    {
        return newLocalPosition;
    }

    public bool GetCanSpawnHere()// Spawn ToggleS
    {
        return canSpawnHere;
    }

}
