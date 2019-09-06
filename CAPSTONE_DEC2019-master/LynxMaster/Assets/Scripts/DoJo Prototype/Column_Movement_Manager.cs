// Written by Mike Elkin 06-26-2019
//edited by Mike Elkin 06-28-2019
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
[AddComponentMenu("Mike's Scripts/Column_Movement_Manager", 8)]

public class Column_Movement_Manager : MonoBehaviour
{
    public enum Mov_Manager_State { WaitingForInput, MovingColumns, SpawningCollectibles };
    [SerializeField]
    Mov_Manager_State columnState;

    #region Columns For Movement
    //Drag & Drop Column GameObjects In Through Inspector
    [SerializeField]
    Column_Movement column_1;
    [SerializeField]
    Column_Movement column_2;
    [SerializeField]
    Column_Movement column_3;
    [SerializeField]
    Column_Movement column_4;
    [SerializeField]
    Column_Movement column_5;
    [SerializeField]
    Column_Movement column_6;
    [SerializeField]
    Column_Movement column_7;
    [SerializeField]
    Column_Movement column_8;
    [SerializeField]
    Column_Movement column_9;
    [SerializeField]
    Column_Movement column_10;
    [SerializeField]
    Column_Movement column_11;
    [SerializeField]
    Column_Movement column_12;
    [SerializeField]
    Column_Movement[] columnArray;

    #endregion
   
    Collectible_Placeholder_Script[] CollectiblesPool; //Pool For Quest Collectibles

    [SerializeField]
    GameObject collectiblePrefab;// Collectible Prefab. Add in Inspector    

    bool _hasNewInput;// Game Loop Switch
    public bool hasNewInput
    {
        get { return _hasNewInput; }
        set { _hasNewInput = value; }
    }// Game Loop Switch Get and Set

    bool isSpawning;// Spawn State switch

    bool ifShiftingStarted;// Move State Switch

    int collectiblesIndex = 0;// Index Used For Collectibles Spawning
    int questNum = 0;// Holds QuestCompleted in DojoQuestManager   
    
    private void Awake()
    {
        columnArray = new Column_Movement[] { column_1, column_2, column_3, column_4, column_5, column_6, column_7, column_8,
                                              column_9, column_10, column_11, column_12 };
        CollectiblesPool = new Collectible_Placeholder_Script[4];
        PopulateCollectiblesPool();
    }
    // Update is called once per frame
    void Update()
    {
        switch(columnState)
        {
            case Mov_Manager_State.WaitingForInput:
                if(hasNewInput)
                {
                    columnState = Mov_Manager_State.MovingColumns;
                    break;
                }
                break;
            case Mov_Manager_State.MovingColumns:
                if (!ifShiftingStarted)
                {
                    MoveColumns();
                }
                if(column_1.transform.position == column_1.GetNewLocalPosition())
                {
                    columnState = Mov_Manager_State.SpawningCollectibles;
                }
                break;
            case Mov_Manager_State.SpawningCollectibles:
                if (!isSpawning)
                {
                    SpawnCollectibles();
                }
                if (isSpawning)
                {
                    WaitForNewInput();
                }
                break;
        }
    }

    void PopulateCollectiblesPool()// Populates Collectibles Pool
    {
        for (int i = 0; i < CollectiblesPool.Length; i++)
        {
            CollectiblesPool[i] = Instantiate(collectiblePrefab).GetComponent<Collectible_Placeholder_Script>();
            CollectiblesPool[i].gameObject.SetActive(false);
        }
    }

    public void ActivateMoveManager(bool hasInput, int qNum)//Manager Trigger Function
    {
        hasNewInput = hasInput;
        questNum = qNum;
    }

    void MoveColumns()// Sets and Initiates New Movement for Colums
    {
        ifShiftingStarted = true;
        switch(questNum)
        {
            case 1:
                column_1.SetColumnParameters(true, 5, true);
                column_2.SetColumnParameters(true, 2,false);
                column_3.SetColumnParameters(true, 1, false);
                column_4.SetColumnParameters(true, 1, false);
                column_5.SetColumnParameters(true, 4, false);
                column_6.SetColumnParameters(true, 3, false);
                column_7.SetColumnParameters(true, 5, true);
                column_8.SetColumnParameters(true, 3, false);
                column_9.SetColumnParameters(true, 4, false);
                column_10.SetColumnParameters(true, 4, false);
                column_11.SetColumnParameters(true, 5, true);
                column_12.SetColumnParameters(true, 5, false);
                ifShiftingStarted = true;
                break;
            case 2:
                column_1.SetColumnParameters(true, 1, false);
                column_2.SetColumnParameters(true, 5, true);
                column_3.SetColumnParameters(true, 1, false);
                column_4.SetColumnParameters(true, 5, true);
                column_5.SetColumnParameters(true, 4, false);
                column_6.SetColumnParameters(true, 5, true);
                column_7.SetColumnParameters(true, 2, false);
                column_8.SetColumnParameters(true, 4, false);
                column_9.SetColumnParameters(true, 4, false);
                column_10.SetColumnParameters(true, 5, true);
                column_11.SetColumnParameters(true, 3, false);
                column_12.SetColumnParameters(true, 3, false);
                ifShiftingStarted = true;
                break;
            case 3:
                column_1.SetColumnParameters(true, 4, false);
                column_2.SetColumnParameters(true, 1, false);
                column_3.SetColumnParameters(true, 1, false);
                column_4.SetColumnParameters(true, 4, false);
                column_5.SetColumnParameters(true, 4, false);
                column_6.SetColumnParameters(true, 4, false);
                column_7.SetColumnParameters(true, 2, false);
                column_8.SetColumnParameters(true, 2, false);
                column_9.SetColumnParameters(true, 3, false);
                column_10.SetColumnParameters(true, 3, false);
                column_11.SetColumnParameters(true, 5, true);
                column_12.SetColumnParameters(true, 1, false);
                ifShiftingStarted = true;
                break;
        }
    }

    void SpawnCollectibles()// Spawns Collectibles at Desired Quest Locations
    {
        isSpawning = true;
        for (int i = 0; i < columnArray.Length; i++)
        {
            if(columnArray[i].GetCanSpawnHere())
            {
                CollectiblesPool[collectiblesIndex].SetPosition(columnArray[i].GetSpawnPointPos(), true);
                collectiblesIndex++;
            }
        }        
    }

    void WaitForNewInput()// Puts Manager in WaitingForInput State
    {
        ifShiftingStarted = false;
        isSpawning = false;
        collectiblesIndex = 0;
        hasNewInput = false;
        columnState = Mov_Manager_State.WaitingForInput;
    }



}
