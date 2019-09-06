//Written by Mike Elkin 06/28/2019
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[ExecuteInEditMode]
[AddComponentMenu("Mike's Scripts/Column_Platforms", 19)]

public class Column_Platforms : MonoBehaviour
{
    [SerializeField]
    Transform spawnPoint;
    [SerializeField]
    MeshRenderer myMesh;
    [SerializeField]
    BoxCollider myCollider;


    private void Awake()
    {
        myMesh = GetComponent<MeshRenderer>();
        myCollider = GetComponent<BoxCollider>();
        
    }
    private void Start()
    {
        myMesh.enabled = false;
        myCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {        
        CheckColumnHeight();
    }

    void CheckColumnHeight()
    {
        if (spawnPoint.position.y >= 1.5f)
        {
            myMesh.enabled = false;
            myCollider.enabled = false;
        }
        else
        {
            myMesh.enabled = true;
            myCollider.enabled = true;
        }
    }




}
