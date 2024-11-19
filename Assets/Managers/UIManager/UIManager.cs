using System;
using System.Collections;
using System.Collections.Generic;
// using deVoid.Utils;
using TMPro;
using Unity.Burst;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField] private GameObject MainUI;
    [SerializeField] private GameObject GameOverScreen;
    [SerializeField] private GameObject ScoreLabel;

    [SerializeField] private GameObject StartMenu;

    [SerializeField] private GameObject PauseOverlay;

    [SerializeField] private GameObject HealthBar;

    public static UIManager Instance {get; private set; }

    private void Awake(){
        // base.OnAwake();
        if (Instance != null && Instance != this){
            Destroy(this);
        }
        else {
            Instance = this;
        }

    }

    private void ConnectSignals(){
        // SignalBus.signalSample.Connect(ShowGameOverScreen);
    }

    private void Start(){
        // ShowMainUI();
        ConnectSignals();
    }

    public void ShowGameOverScreen(){
        MainUI.SetActive(false);
        GameOverScreen.SetActive(true);
    }

    public void ShowStartMenu(){
        MainUI.SetActive(false);
        GameOverScreen.SetActive(false);
        StartMenu.SetActive(true);
    }

    public void ShowMainUI(){
        MainUI.SetActive(true);
        GameOverScreen.SetActive(false);
        StartMenu.SetActive(false);
    }

    public void UpdateUITitle(string title){
        MainUI.transform.Find("LevelLabel").GetComponent<TMPro.TextMeshProUGUI>().text = title;
    }

    public void SetScore(int newScore){
        ScoreLabel.GetComponent<TMPro.TextMeshProUGUI>().text = newScore.ToString(); 
    }

    public void InitialiseHealth(EntityStatsScriptableObject healthObj){
        Image img = HealthBar.GetComponent<Image>();
        img.fillAmount = healthObj.health / healthObj.maxHealth;

        healthObj.HealthChangedEvent.AddListener(ChangeHealth);
    }

    public void ChangeHealth(int newHealth, int maxHealth){
        Debug.Log("Changing UI health: " + (float)newHealth / maxHealth);
        
        HealthBar.GetComponent<Image>().fillAmount = (float)newHealth / maxHealth;
    }

    public void ShowPauseOverlay(){
        PauseOverlay.SetActive(true);
        MainUI.SetActive(false);
    }
}
