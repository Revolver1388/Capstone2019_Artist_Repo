using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZTargetEnemySwitch : MonoBehaviour
{

    public bool ZTarget;
    public bool lockCursor;//turns off the mouse cursor if not needed
    public bool cameraFree;
    public float mousesensitivity = 10;//pretty self explanitory
    public Transform Player;//set to player character
    public float distFromPlayer = 10;//set distance from the character
    public float distFromTarget = 8;
    public float distFromEnemy = Mathf.Infinity;

    public Vector2 pitchMinMax = new Vector2(10, 55);//clamps the camera between two vectors***add a second min max to lock the x seperate from the y

    public float rotationsmoothTime = .6f;//smoothing for the rotational movement, .6f is great, 1 feels a little slow
    Vector3 rotationsmoothVelocity;
    public Vector3 currentRotation;
    public Vector3 eul;

    float yaw;
    float pitch;

    private GameObject[] targets;
    public GameObject closestEnemy = null;
    public Transform TargetT;

    void Start()
    {
        ZTarget = false;
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    private void Update()
    {

        if (Input.GetButton("LeftBumper"))
        {
            TargetT = GetTarget().transform;
            float distance = Vector3.Distance(GetTarget().transform.position, Player.transform.position);
            if (distance < distFromTarget)
            {
                ZTarget = true;
                if (GetTarget() != null)
                {
                    Player.transform.LookAt(TargetT);
                }
            }
            else if (distance >= distFromTarget)
            {
                ZTarget = true;
                distFromEnemy = Mathf.Infinity;
                targets = null;
                TargetT = null;
                transform.LookAt(Player);
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            ZTarget = false;
            distFromEnemy = Mathf.Infinity;
            targets = null;
            TargetT = null;
        }
    }
    private void LateUpdate()
    {
        if (cameraFree == true)
        {
            yaw += Input.GetAxis("MouseX") * mousesensitivity;
            pitch -= Input.GetAxis("MouseY") * mousesensitivity;
            pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

            currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationsmoothVelocity, rotationsmoothTime);
            transform.eulerAngles = currentRotation;

            eul = transform.eulerAngles;
            eul.x = 0;

            transform.eulerAngles = eul + currentRotation;
            transform.position = Player.position - transform.forward * distFromPlayer;
        }
        else if (cameraFree == false)
        {
            yaw += Input.GetAxis("MouseX") * mousesensitivity;
            pitch -= Input.GetAxis("MouseY") * mousesensitivity;
            pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);
            currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationsmoothVelocity, rotationsmoothTime);
            transform.eulerAngles = currentRotation;
            transform.position = Player.position - transform.forward * distFromPlayer;
            transform.LookAt(TargetT);
        }
        //else if (cameraFree == false)                           //places player in first person mode, pretty neat actually
        //{
        //    transform.position = Player.transform.position;
        //    transform.LookAt(TargetT);
        //}
    }

    public GameObject GetTarget()//this cycles through the enemies in the area, can be converted to be used with swingpoints
    {
        targets = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject target in targets)
        {
            Vector3 dist = target.transform.position - Player.position;
            float currentDistance = dist.sqrMagnitude;
            if (currentDistance < distFromEnemy)
            {
                closestEnemy = target;
                distFromEnemy = currentDistance;
            }
        }
        return closestEnemy;
    }
}