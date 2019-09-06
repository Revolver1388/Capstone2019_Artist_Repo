using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmadilloAttack : MonoBehaviour
{
    //ArmadilloSharedVariables enemy;

    #region Variables

    public bool isRolling;
    [SerializeField]
    private float speed = 8;
    //public float speed;
    [SerializeField]
    private int rollTimerInSec = 2;
    [SerializeField]
    private float spherecastDistance = 0.09f;
    [SerializeField]
    private float rollingSpeed = 15;
    [SerializeField]
    private int playerLayer = 10;
    [SerializeField]
    private float impulseForce = 5;
    [SerializeField]
    private float impulseRadius = 2;
    [SerializeField]
    private float impulseHeight = 3;
    [SerializeField]
    private int rotationSpeedMultiplier = 20;
    [SerializeField]
    float raycastDistance = 20;
    [SerializeField]
    private int searchForPlayerSec = 3;
    [SerializeField]
    private int attackDamage = 1;

    private int lookCounter;
    private bool playerFound;
    private int f;

    private Transform armadilloMovement;
    private Rigidbody armadilloRB;
    private Animator anim;
    private Vector3 newForward;
    private WaitForSeconds framRate = new WaitForSeconds(1 / 60);
    #endregion

    // Use this for initialization
    void Start()
    {
        armadilloMovement = transform.parent;
        armadilloRB = gameObject.GetComponentInParent<Rigidbody>(); 
        anim = GetComponent<Animator>();
        speed = speed / 100;
        FindNewForward();
        StartCoroutine("Attack");
    }

    void OnEnable()
    {
        lookCounter = 0;
        FindNewForward();
        StartCoroutine("Attack");
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator Attack()
    {
        RaycastHit hit;
        //try
        //{
        //    Debug.Log(armadilloMovement.position);
        //}
        //catch(System.Exception e)
        //{
        //    Debug.Log("Not sure why this happens even though it still works");
        //}
        isRolling = true;
        //updates balls x rotation rotation and newforwards position 60 times per sec
        //for the time given
        if (armadilloMovement.position == null)
        {
            armadilloMovement = transform.parent;
        }

        for (int i = rollTimerInSec; i > 0; i--)
        {
            for (int f = 0; f < 60; f++)
            {
                if (Physics.SphereCast(
                    armadilloMovement.position,
                    1f, newForward,
                    out hit,
                    spherecastDistance))
                {
                    if (hit.collider.gameObject.GetComponent<HitBox>() != null)
                    {
                        hit.collider.gameObject.GetComponent<HitBox>().TakeDamage(attackDamage);
                    }
                    StartCoroutine("BounceBack");
                    yield break;
                }
                else
                {
                    Roll(1);
                }
                yield return framRate;
            }
        }

        //Makes sure balls x rotation is facing the y axis up  
        while (transform.rotation.x < -0.01f || transform.rotation.x > 0.01f)
        {
            Roll(1);
            yield return framRate;
        }

        Quaternion temp = transform.rotation;
        temp.x = 0;
        temp.z = 0;
        transform.rotation = temp;
        isRolling = false;
        anim.SetTrigger("PlayStunned");
    }



    IEnumerator BounceBack()
    {
        isRolling = true;
        //bounces armadillo after hitting something while fast rolling
        armadilloRB.AddExplosionForce
            (impulseForce, 
            armadilloMovement.position, 
            impulseRadius, 
            impulseHeight,
            ForceMode.Impulse);

        for (int f = 0; f < 60; f++)
        {
            Roll(-1);
            yield return framRate;
        }

        while (transform.rotation.x < -0.01f || transform.rotation.x > 0.01f)
        {
            Roll(1);
            yield return framRate;
        }
        Quaternion temp = transform.rotation;
        temp.x = 0;
        temp.z = 0;
        transform.rotation = temp;
        isRolling = false;
        anim.SetTrigger("PlayStunned");
    }



    IEnumerator LookForPlayer()
    {
        playerFound = false;

        if (Mathf.RoundToInt(Random.Range(0, 1000)) % 2 == 0)
        {
            speed = -1 * speed;
        }

        for (int i = searchForPlayerSec; i > 0; i--)
        {
            for (f = 60; f > 0; f--)
            {
                RotateEnemy();
                yield return framRate;
            }    
        }

        if(!playerFound)
        {
            lookCounter++;
        }

        if(lookCounter < 3)
        {
            if (speed < 0)
            {
                speed = speed * -1;
            }

            FindNewForward();
            StartCoroutine("Attack");
        }
        else
        {
            GetComponent<RandomWonder>().enabled = true;
            GetComponent<ArmadilloAttack>().enabled = false;
            yield break;
        }
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

        RaycastHit hitRotation;
        if (Physics.SphereCast(
                    armadilloMovement.position,
                    1.5f, newForward,
                    out hitRotation,
                    raycastDistance))
        {
            Debug.DrawRay(armadilloMovement.position, newForward * 500, Color.red, raycastDistance);

            if (hitRotation.collider.gameObject.layer == playerLayer)
            {
                f = 0;
                lookCounter = 0;
                playerFound = true;
            }
            else
            {
                f++;
            }
        }

        armadilloMovement.transform.RotateAround(transform.position, 
            Vector3.up, speed * rotationSpeedMultiplier);
    }

    //when using 1 goes forward -1 goes in reverse
    void Roll(int direction)
    {
        //rolls the ball
        transform.Rotate(Vector3.right, (rollingSpeed * direction));
        //move ball forward        
        armadilloMovement.position = armadilloMovement.position + (newForward * direction);
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
}
