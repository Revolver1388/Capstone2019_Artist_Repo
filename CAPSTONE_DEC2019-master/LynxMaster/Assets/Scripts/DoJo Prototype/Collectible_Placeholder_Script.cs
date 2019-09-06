// Written by Mike Elkin 06-26-2019
//edited by Mike Elkin 06-28-2019
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
[AddComponentMenu("Mike's Scripts/Collectible_Placeholder_Script", 6)]

public class Collectible_Placeholder_Script : MonoBehaviour
{
    DojoQuestManager DQM;// DoJoQuestManager Reference

    [SerializeField]
    ParticleSystem myPS;// Collectible Particle System Reference  
    
    Vector3 rotationVector;
    float rotX = 0.0f;// Sets X rotation in rotationVector
    float rotY = 0.0f;// Sets Y rotation in rotationVector
    float rotZ = 0.0f;// Sets Z rotation in rotationVector
    
    int rdmNum; // Sets Scale Of Change

    private void Awake()
    {
        myPS = GetComponentInChildren<ParticleSystem>();
        DQM = GameObject.FindGameObjectWithTag("DoJoQuestManager").GetComponent<DojoQuestManager>();
    }
    private void Update()
    {
        //PSRotation();
    }
    private void LateUpdate()
    {
        PSRotation();
    }

    private void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag == "Player")
        {
            //DQM.quest1CollectibleCount++;       

            switch (DQM.questsCompleted)
            {
                case 1:
                    DQM.quest1CollectibleCount++;
                    if(DQM.quest1CollectibleCount == DQM.totalQuest1Collectibles)
                    {
                        DQM.questsCompleted++;
                        DQM.triggerHit = true;
                    }
                    
                    break;

                case 2:
                    DQM.quest2CollectibleCount++;
                    if (DQM.quest2CollectibleCount == DQM.totalQuest2Collectibles)
                    {
                        DQM.questsCompleted++;
                        DQM.triggerHit = true;
                    }
                    
                    break;

                case 3:
                    DQM.quest3CollectibleCount++;
                    if (DQM.quest3CollectibleCount == DQM.totalQuest3Collectibles)
                    {
                        DQM.questsCompleted++;
                        DQM.triggerHit = true;
                    }
                    
                    break;
            }
            gameObject.SetActive(false);
        }
    }

    public void SetPosition(Vector3 pos, bool enabled)
    {
        transform.position = pos;
        gameObject.SetActive(enabled);
    }

    void PSRotation()
    {
        rdmNum = Random.Range(50, 300);
        rotX += rdmNum * Time.deltaTime;
        rotY += -rdmNum * Time.deltaTime;
        rotZ += rdmNum * Time.deltaTime;
        rotationVector.x = rotX;
        rotationVector.y = rotY;
        rotationVector.z = rotZ;
        myPS.transform.eulerAngles = rotationVector;
    }
    

}
