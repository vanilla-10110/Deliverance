using System;
using System.Collections;
using System.Collections.Generic;
// using deVoid.Utils;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public GameObject MainUI;
    public GameObject GameOverOverlay;
    public GameObject ScoreLabel;

    public GameObject PauseOverlay;

    public static UIManager Instance {get; private set; }

    private void Awake(){
        // base.OnAwake();
        if (Instance != null && Instance != this){
            Destroy(this);
        }
        else {
            Instance = this;
        }

        // DontDestroyOnLoad(ScoreLabel);
        // DontDestroyOnLoad(MainUI);
    }

    private void ConnectSignals(){
        // SignalBus.signalSample.Connect(ShowGameOverScreen);
    }

    private void Start(){
        ShowMainUI();
        ConnectSignals();
    
    }

    public void ShowGameOverScreen(){
        MainUI.SetActive(false);
        GameOverOverlay.SetActive(true);
    }

    public void ShowMainUI(){
        MainUI.SetActive(true);
        GameOverOverlay.SetActive(false);
    }

    public void SetScore(int newScore){
        ScoreLabel.GetComponent<TMPro.TextMeshProUGUI>().text = newScore.ToString(); 
    }

    public void ShowPauseOverlay(){
        PauseOverlay.SetActive(true);
        MainUI.SetActive(false);
    }
}
