using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TetherPoint : MonoBehaviour, Interact
{

    public Material tetherMat;


    public GrappleComponent grappleComp;

    private void Awake()
    {
        GameObject player = GameObject.FindWithTag("Player");
        grappleComp = player.GetComponent<GrappleComponent>();
        tetherMat.color = Color.gray;

       
    }

    public void DontInteractWithMe()
    {
        if (grappleComp != null)
        {
            if(grappleComp.tetherPoint == this.transform)
            {
            grappleComp.setTetherPoint(null);
            grappleComp.isStaring = false;
            }
            tetherMat.color = Color.gray;
        }

    }

    public void InteractWithMe()
    {

        if (grappleComp != null)
        {

            tetherMat.color = Color.red;
            grappleComp.setTetherPoint(transform);
            grappleComp.isStaring = true; 
           


        }

    }


}


