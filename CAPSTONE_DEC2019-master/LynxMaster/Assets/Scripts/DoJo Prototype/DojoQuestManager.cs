// Written by Mike Elkin 06-26-2019
//edited by Mike Elkin 06-28-2019
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[ExecuteInEditMode]
[AddComponentMenu("Mike's Scripts/DojoQuestManager", 18)]
public class DojoQuestManager : MonoBehaviour
{
    public enum QuestManState { AwaitingTrigger, SetQuest, End }
    [SerializeField]
    QuestManState questState;
    
    Column_Movement_Manager c_Mov_Man;// Column Movement Manager Reference

    #region Quest Variables
    int _quest1CollectibleCount = 0;// Collectibles Player Has Collected For Quest 1
    public int quest1CollectibleCount
    {
        get { return _quest1CollectibleCount; }
        set { _quest1CollectibleCount = value; }
    }// Get & Set
    
    int _quest2CollectibleCount = 0;// Collectibles Player Has Collected For Quest 2 
    public int quest2CollectibleCount
    {
        get { return _quest2CollectibleCount; }
        set { _quest2CollectibleCount = value; }
    }// Get & Set

    int _quest3CollectibleCount = 0;// Collectibles Player Has Collected For Quest 3
    public int quest3CollectibleCount
    {
        get { return _quest3CollectibleCount; }
        set { _quest3CollectibleCount = value; }
    }// Get & Set

    int _totalQuest1Collectibles = 3;// Required Collectibles for Quest 1
    public int totalQuest1Collectibles
    {
        get { return _totalQuest1Collectibles; }
        set { _totalQuest1Collectibles = value; }
    }// Get & Set

    int _totalQuest2Collectibles = 4;// Required Collectibles for Quest 2
    public int totalQuest2Collectibles
    {
        get { return _totalQuest2Collectibles; }
        set { _totalQuest2Collectibles = value; }
    }// Get & Set

    int _totalQuest3Collectibles = 1;// Required Collectibles for Quest 3
    public int totalQuest3Collectibles
    {
        get { return _totalQuest3Collectibles; }
        set { _totalQuest3Collectibles = value; }
    }// Get & Set

    int _questsCompleted = 0;// Number Of DoJo Quests Player Has Completed
    public int questsCompleted
    {
        get { return _questsCompleted; }
        set { _questsCompleted = value; }
    }
    #endregion
    
    bool _triggerHit;// Trigger Sets DoJoQuestManager state to SetQuest State
    public bool triggerHit
    {
        get { return _triggerHit; }
        set { _triggerHit = value; }
    }
    bool notSettingQuest;// SetQuest State Switch
    


    private void Awake()
    {
        c_Mov_Man = GetComponentInChildren<Column_Movement_Manager>();        
    }

    // Update is called once per frame
    void Update()
    {
        switch(questState)
        {
            case QuestManState.AwaitingTrigger:
                if(triggerHit)
                {
                    notSettingQuest = true;
                    questState = QuestManState.SetQuest;
                }
                break;
            case QuestManState.SetQuest:
                if(notSettingQuest)
                {
                    if(questsCompleted == 4)
                    {
                        questState = QuestManState.End;
                    }
                    else
                    {
                        NotifyMangerOfChange();
                    }                    
                }
                break;
            case QuestManState.End:
                // Set Next Task For Player

                break;
        }
    }
    void NotifyMangerOfChange()// Start Next DojoQuest 
    {
        notSettingQuest = false;
        c_Mov_Man.ActivateMoveManager(true, questsCompleted);
        questState = QuestManState.AwaitingTrigger;
        triggerHit = false;
    }

    

}
