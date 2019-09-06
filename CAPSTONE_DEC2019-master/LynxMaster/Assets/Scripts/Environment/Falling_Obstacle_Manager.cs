using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[ExecuteInEditMode]
[AddComponentMenu("Mike's Scripts/Falling_Obstacle_Manager",13)]

public class Falling_Obstacle_Manager : MonoBehaviour
{
    [SerializeField]
    Falling_Obstacle[] fallingObjects;

    [SerializeField, Range(1, 10)]
    int resetDelay = 1;

    [SerializeField]
    float startTime;

    [SerializeField]
    float currentTime;

    [SerializeField]
    bool getTime;

    [SerializeField]
    bool startReset;
    

    void Awake()
    {
        fallingObjects = GetComponentsInChildren<Falling_Obstacle>(true);
    }

    void Update()
    {
        ResetFunction();

    }

    void ResetObject(Falling_Obstacle temp)
    {

        temp.transform.position = temp.startPos;
        temp.transform.rotation = temp.startRot;
        temp.rb.freezeRotation = true;
        temp.functionStart = false;
        temp.meshrend.enabled = true;
        temp.collider.enabled = true;
    }

    //IEnumerable ResetDelay(int delay)
    //{
    //    Debug.Log("timer started");
    //    yield return new WaitForSecondsRealtime(delay);
    //    ResetArray();
    //}
    void ResetArray()
    {
        for (int i = 0; i < fallingObjects.Length; i++)
        {
            Debug.Log("checking objects");
            if (fallingObjects[i].meshrend.enabled == false)
            {
                Debug.Log("Resetting Object: " + i);
                ResetObject(fallingObjects[i]);
            }
        }
    }

    void ResetFunction()
    {
        if (startReset)
        {
            if (getTime)
            {
                startTime = Time.time;
                getTime = false;
            }
            if(CheckTimeElapsed() >= resetDelay)
            {
                ResetArray();
                startReset = false;
            }
        }
    }
    float CheckTimeElapsed()
    {
        currentTime = Time.time;
        return currentTime - startTime;
    }
    public void FlipSwitch()
    {
        startReset = true;
        getTime = true;
    }



}


