//Written By Michael Elkin 04/06/2019
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[ExecuteInEditMode]
[AddComponentMenu("Mike's Scripts/FloraAnimatorController", 12)]

public class FloraAnimatorController : MonoBehaviour
{
    [SerializeField]
    Animator _anim;
    public Animator anim
    {
        get { return _anim; }
        private set { _anim = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider o)
    {
        if(o.gameObject.tag == "Player")
        {
            PlayCollisionAnimation();
        }
    }
    private void OnTriggerExit(Collider o)
    {
        if (o.gameObject.tag == "Player")
        {
            StopCollisionAnimation();
        }
    }

    void StopCollisionAnimation()
    {
        anim.SetTrigger("notTouched");        
    }

    void PlayCollisionAnimation()
    {
        anim.SetTrigger("isTouched");
    }



}
