using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeMovement : MonoBehaviour
{
    public float speed = .5f;
    public float rotateSpeed = 3.0f;
    public float radius = 2.5f;

    public Transform player;
    public GameObject hive;

    Vector3 newPosition;

    void Start()
    {
        PositionChange();
    }

    void PositionChange()
    {
        newPosition = new Vector3(Random.Range(1f, 5.0f), Random.Range(1f, 5.0f));
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, newPosition) < 1)
            PositionChange();

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * speed);

        LookAt(newPosition);
        if (Vector3.Distance(transform.position, player.transform.position) <= radius)
        {
            transform.LookAt(player);
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime);
        }
       


    }

    void LookAt(Vector3 lookAtPosition)
    {
        float distanceX = lookAtPosition.x - transform.position.x;
        float distanceY = lookAtPosition.y - transform.position.y;
        float angle = Mathf.Atan2(distanceX, distanceY) * Mathf.Rad2Deg;

        Quaternion endRotation = Quaternion.AngleAxis(angle, Vector3.back);
        transform.rotation = Quaternion.Slerp(transform.rotation, endRotation, Time.deltaTime * rotateSpeed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void Death()
    {

    }
}


