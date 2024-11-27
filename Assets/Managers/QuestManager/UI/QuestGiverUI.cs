using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class QuestGiverUI : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject contentObject;
    [SerializeField] private Button exitUIButton;
    [NonSerialized] public QuestGiver questGiver;

    [SerializeField] private List<QuestCard> Cards = new();

    void Start(){
        exitUIButton?.onClick.AddListener(() => {
            ResetUI();
            gameObject.SetActive(false);
        });
    }

    // for the quest giver to initialise
    public void InitialiseQuestGiverUI(QuestGiver giver){
        questGiver = giver;

        foreach (Quest quest in questGiver.availableQuests){
            AddQuestCard(quest);
        }

        gameObject.SetActive(true);
    }

    public void UpdateQuests(List<Quest> quests){
        ResetUI();

        foreach (Quest quest in quests){
            AddQuestCard(quest);
        }
    }

    private void AddQuestCard(Quest quest) {
        QuestCard newCard = Instantiate(cardPrefab, contentObject.transform.position, quaternion.identity, contentObject.transform).GetComponent<QuestCard>();

        Cards.Add(newCard);

        newCard.questAccepted.AddListener((Quest quest) => {HandleQuestAccepted(newCard);});

        newCard.InitialiseCard(quest, questGiver.availableQuests.IndexOf(quest));
    }
    private void HandleQuestAccepted(QuestCard card){
        
        questGiver.AddQuestToManager(card.quest);
        Cards.Remove(card);
        Destroy(card.gameObject);
    }
    private void ResetUI(){
        questGiver = null;
        
        if (Cards.Count > 0){
            foreach (QuestCard card in Cards){
               Destroy(card.gameObject);
            }
        }

        Cards.Clear();
        
    }
}
