using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    private static QuestManager questManager;

    [SerializeField] private List<Quest> availableQuests;

    void Start(){
        questManager = QuestManager.Instance;
    }

    void Update(){
        if (InputManager.interactWasPressed){
            AddQuestToManager(0);
        }
    }

    public void AddQuestToManager(int questIndex){
        Quest quest = availableQuests[questIndex];
        if (quest != null){
            Debug.Log("Adding quest to manager : from giver");
            questManager.AddActiveQuest(quest);
            availableQuests.Remove(quest);
        }
    }
    private void OnDrawGizmos(){
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 1);
    }
}
