using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    private static QuestManager questManager;

    [SerializeField] private GameObject _popup;

    [SerializeField] public List<Quest> availableQuests;
    private bool Interactable = false;

    void Start(){
        questManager = QuestManager.Instance;
        _popup.SetActive(false);

    }

    void Update(){
        if (Interactable && InputManager.interactWasPressed){
            ShowUI();
        }
    }

    public void AddQuestToManager(Quest selectedQuest){
        Quest quest = availableQuests[availableQuests.FindIndex((q) => q == selectedQuest)];
        if (quest != null){
            Debug.Log("Adding quest to manager : from giver");
            questManager.AddActiveQuest(quest);
            availableQuests.Remove(quest);
        }
    }

    // add area to trigger UI
    private void OnTriggerEnter2D(Collider2D collision2D){
        if (collision2D.gameObject.CompareTag("Player")){
            Debug.Log("Player walked in");

            Interactable = true;
            _popup.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision2D){
        if (collision2D.gameObject.CompareTag("Player")){
            Debug.Log("Player exited");

            Interactable = false;
            _popup.SetActive(false);

        }
    }

    private void ShowUI(){
        questManager.QuestGiverUI.InitialiseQuestGiverUI(this.gameObject.GetComponent<QuestGiver>());
    }

    private void OnDrawGizmos(){
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 1);
    }
}
