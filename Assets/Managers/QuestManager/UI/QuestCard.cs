using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuestCard : MonoBehaviour
{
    [NonSerialized] public UnityEvent<Quest> questAccepted = new();

    [NonSerialized] public int questIndex;
    [SerializeField] public Quest quest {get; private set;}
    [SerializeField] private TMPro.TextMeshProUGUI titleText;
    [SerializeField] private TMPro.TextMeshProUGUI descriptionText;
    [SerializeField] private Button acceptButton;

    void Start(){
        acceptButton.onClick.AddListener(() => questAccepted.Invoke(quest));
    }

    public void InitialiseCard(Quest newQuest, int Index){
        quest = newQuest;
        questIndex = Index;
        SetTitle(quest.title);
        SetDescription(quest.description);
    }

    private void SetTitle(string title){
        titleText.text = title;
    }

    private void SetDescription(string description){
        descriptionText.text = description;
    }

}
