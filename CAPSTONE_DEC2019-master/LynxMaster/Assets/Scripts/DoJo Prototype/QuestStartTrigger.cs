//Written by Mike Elkin 06/28/2019
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[ExecuteInEditMode]
[AddComponentMenu("Mike's Scripts/QuestStartTrigger", 15)]

public class QuestStartTrigger : MonoBehaviour
{
    [SerializeField]
    DojoQuestManager DQM;

    private void OnTriggerEnter(Collider o)
    {
        if(o.gameObject.tag == "Player")
        {
            DQM.questsCompleted++;
            DQM.triggerHit = true; 
            gameObject.SetActive(false);
        }
    }
}
