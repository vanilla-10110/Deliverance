using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class QuestManager : MonoBehaviour
{
    public QuestGiverUI QuestGiverUI;
    
    [SerializeField] private QuestGiverUI PauseMenuTAskUI;

    [SerializeField] private List<Quest> activeQuests = new();
    [SerializeField] private List<Quest> completedQuests = new();

    public static QuestManager Instance {get; private set;}

    void Awake()
    {
        if (Instance != null){
            Destroy(this.gameObject);
        }
        else{
            Instance = this;
        }
    }

    // to be called by the quest giver to add a new quest to the list
    public void AddActiveQuest(Quest newQuest){
        Debug.Log("quest received, adding to active list : Manager : " + newQuest.title );

        Quest quest = newQuest;

        activeQuests.Add(quest);

        PauseMenuTAskUI.UpdateQuests(activeQuests);


        ConnectQuestEvents(quest);
        quest.questCompleted.AddListener(() => {
            SetQuestAsComplete(quest); 
        });
    }

    private void SetQuestAsComplete(Quest quest){
        quest.questCompleted.RemoveAllListeners();
        DisconnectQuestEvents(quest);
        activeQuests.Remove(quest);
        completedQuests.Add(quest);

        PauseMenuTAskUI.UpdateQuests(activeQuests);
    }

    private void ConnectQuestEvents(Quest quest) {
        switch (quest.type){
            case Quest.QUEST_TYPE.COLLECT_SOUL:
                SignalBus.CollectedSoulEvent.AddListener(quest.ReduceTaskCountAction);
                return;
            case Quest.QUEST_TYPE.DESTROY_ENTITIES:
                SignalBus.DestroyedEntityEvent.AddListener(quest.ReduceTaskCountAction);
                return;
            case Quest.QUEST_TYPE.ABILITY_USAGE:
                SignalBus.AbilityUsedEvent.AddListener(quest.ReduceTaskCountAction);
                return;
        }
    }
    private void DisconnectQuestEvents(Quest quest) {
        switch (quest.type){
            case Quest.QUEST_TYPE.COLLECT_SOUL:
                SignalBus.CollectedSoulEvent.RemoveListener(quest.ReduceTaskCountAction);
                return;
            case Quest.QUEST_TYPE.DESTROY_ENTITIES:
                SignalBus.DestroyedEntityEvent.RemoveListener(quest.ReduceTaskCountAction);
                return;
            case Quest.QUEST_TYPE.ABILITY_USAGE:
                SignalBus.AbilityUsedEvent.RemoveListener(quest.ReduceTaskCountAction);
                return;
        }
    }



}
