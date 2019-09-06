using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWonder : MonoBehaviour
{
    #region Variables

    public bool isRolling;

    public Transform center;
    [SerializeField]
    private float speed = 6;
    [SerializeField]
    private int rollTimerInSec = 3;
    [SerializeField]
    private float spherecastDistance = 1.5f;
    [SerializeField]
    private float rollingSpeed = 10;
    [SerializeField]
    private int playerLayer = 10;
    [SerializeField]
    private float wonderDistance = 5;
    [SerializeField]
    private float rotationSpeed = 1;

    private Transform armadilloMovement;
    private Animator anim;
    private Vector3 newForward;
    private WaitForSeconds framRate = new WaitForSeconds(1/60);
    private RaycastHit hitRotation;

    private int rotationSpeedMultiplier;
    private int f;
    private bool attack;
    #endregion

    // Use this for initialization
    void Start ()
    {
        armadilloMovement = transform.parent;
        anim = GetComponent<Animator>();
        speed = speed / 100;
        FindNewForward();
        anim.SetTrigger("PlayArmadilloRollUp");
    }

    void OnEnable()
    {
        attack = false;
        FindNewForward();

        if (anim != null)
        {
            anim.SetTrigger("PlayArmadilloRollUp");
        }
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator RollAround()
    {
        RaycastHit hit;
 
        //updates balls x rotation rotation and newforwards position 60 times per sec
        //for the time given
        isRolling = true;

        for (int i = rollTimerInSec; i > 0; i--)
        {
            for (f = 0; f < 60; f++)
            {             
                if (Physics.SphereCast(
                    armadilloMovement.position, 
                    1.5f, newForward, 
                    out hit, 
                    spherecastDistance))
                {
                    f = 60;
                    i = 0;
                }
                else
                {
                    Roll();
                }
                yield return framRate;
            }
        }

        //Makes sure balls x rotation is facing the y axis up  
        while (transform.rotation.x < -0.01f || transform.rotation.x > 0.01f)
        {
            Roll();
            yield return framRate;
        }

        Quaternion temp = transform.rotation;
        temp.x = 0;
        temp.z = 0;
        transform.rotation = temp;
        isRolling = false;
        anim.SetTrigger("PlayLookAround");
    }

    IEnumerator Rotate()
    {
        //randomizes spin speed to randomize next direction 
        rotationSpeedMultiplier = Mathf.RoundToInt(Random.Range(10.0f, 20.0f));

        if (Mathf.RoundToInt(Random.Range(0, 1000)) % 2 == 0)
        {
            speed = -1 * speed;
        }

        center.LookAt(transform.position);
        Vector3 newCenterForward;
        newCenterForward = center.forward;
        newCenterForward.x = newCenterForward.x * speed;
        newCenterForward.z = newCenterForward.z * speed;

        //Debug.DrawRay(center.position, newCenterForward * 100, Color.red, 5);
        //if (Physics.)
        if (Physics.Raycast(center.position, newCenterForward, wonderDistance))
        {
            //rotates the object along y axis 
            for (int i = 1; i > 0; i--)
            {
                for (f = 60; f > 0; f--)
                {
                    RotateEnemy();
                    yield return framRate;
                }
                if (attack)
                {
                    i = 0;
                }
            }
        }
        else
        {
            Vector3 lookPos = center.position - transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            for (f = 200; f > 0; f--)
            {
                RotateEnemy(rotation);
                yield return framRate;
            }
        }

        //reset speed to a positive value
        if (speed < 0)
        {
            speed = speed * -1;
        }

        FindNewForward();
        if (attack == false)
        {
            anim.SetTrigger("PlayArmadilloRollUp");
        }
        else
        {
            GetComponent<RandomWonder>().enabled = false;
            GetComponent<ArmadilloAttack>().enabled = true;
        }
    }

    void FindNewForward()
    {
        newForward = transform.forward;
        newForward.x = newForward.x * speed;
        newForward.z = newForward.z * speed;
    }

    void FindNewForward(float speed)
    {
        newForward = transform.forward;
        newForward.x = newForward.x * speed;
        newForward.z = newForward.z * speed;
    }

    void RotateEnemy(Quaternion pRotation)
    {
        if (speed < 0)
        {
            FindNewForward(-speed);
        }
        else
        {
            FindNewForward();
        }
        //longer raycast to find enemy
        if (Physics.Raycast(armadilloMovement.position, newForward, out hitRotation, 20))
        {
            if (hitRotation.collider.gameObject.layer == playerLayer)
            {
                attack = true;
                f = 0;
            }
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, pRotation, Time.deltaTime * rotationSpeed);
    }

    void RotateEnemy()
    {
        if (speed < 0)
        {
            FindNewForward(-speed);
        }
        else
        {
            FindNewForward();
        }
        //shorter raycast for walls and such
        if (Physics.Raycast(armadilloMovement.position, newForward, out hitRotation, spherecastDistance * 4f))
        {
            if (hitRotation.collider.gameObject.layer == playerLayer)
            {
                attack = true;
                f = 0;
            }
            f++;
        }
        //longer raycast to find enemy
        if (Physics.Raycast(armadilloMovement.position, newForward, out hitRotation, 20))
        {
            //Debug.DrawRay(armadilloMovement.position, newForward * 500, Color.red, spherecastDistance * 10);

            if (hitRotation.collider.gameObject.layer == playerLayer)
            {
                attack = true;
                f = 0;
            }
        }

        armadilloMovement.transform.RotateAround(transform.position, Vector3.up, speed * rotationSpeedMultiplier);
    }

    void Roll()
    {

        //rolls the ball
        transform.Rotate(Vector3.right, rollingSpeed);
        //move ball forward        
        armadilloMovement.position = transform.position + newForward;
    }
}
