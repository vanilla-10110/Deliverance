using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using MyBox;
using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Quest
{
    [NonSerialized] public UnityEvent questCompleted = new();

    public enum QUEST_TYPE {
        ABILITY_USAGE,
        DESTROY_ENTITIES,
        COLLECT_SOUL
    }

    public QUEST_TYPE type;
    
    [ConditionalField(nameof(type), false, QUEST_TYPE.ABILITY_USAGE)] public string abilityUsageEnumString; // assign the name of the enum you want to use. to be processed elsewhere

    [SerializeField] private int taskCount;
    public int TaskCount {
        get {return taskCount;}
        set {
            taskCount = value;
            if (taskCount <= 0) {
                questCompleted.Invoke();
            }
        }
    }

    public string title;
    public string description;
    public int xpReward;
    public int wealthReward;
    

    public void ReduceTaskCountAction(){
        TaskCount -= 1;
    }
    
    public void ReduceTaskCountAction(string enumString){
        if(enumString == abilityUsageEnumString){
            TaskCount -= 1;
        }
    }
    
    // public void ConnectEventToTaskCount(UnityEvent newEvent){
    //     newEvent?.AddListener(ReduceTaskCountAction);
    // }
    
    // public void ConnectEventToTaskCount(UnityEvent<String> newEvent){
    //     if (type == QUEST_TYPE.ABILITY_USAGE){
    //         newEvent?.AddListener(ReduceTaskCountAction);

    //     }
    // }
}


