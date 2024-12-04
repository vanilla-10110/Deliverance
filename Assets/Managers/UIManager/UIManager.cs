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
    //[SerializeField] private GameObject ScoreLabel;

    [SerializeField] private GameObject StartMenu;

    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject WinScreen;

    //Health
    [SerializeField] private GameObject HealthBar;
    List<HealthHeart> hearts = new List<HealthHeart>();
    public GameObject heartPrefab;

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

    private void Update(){
                // to change
        if (PauseMenu.activeInHierarchy != true && InputManager.pauseWasPressed){
            PauseMenu.SetActive(true);
        }
        else if (PauseMenu.activeInHierarchy == true && InputManager.pauseWasPressed){
            PauseMenu.SetActive(false);
        }
    }

    public void ShowGameOverScreen(){
        MainUI.SetActive(false);
        GameOverScreen.SetActive(true);
        WinScreen.SetActive(false);

    }

    public void ShowStartMenu(){
        MainUI.SetActive(false);
        GameOverScreen.SetActive(false);
        StartMenu.SetActive(true);
        WinScreen.SetActive(false);

    }

    public void ShowMainUI(){
        MainUI.SetActive(true);
        GameOverScreen.SetActive(false);
        StartMenu.SetActive(false);
        WinScreen.SetActive(false);

    }

    public void ShowWinScreen(){
        MainUI.SetActive(false);
        GameOverScreen.SetActive(false);
        StartMenu.SetActive(false);
        WinScreen.SetActive(true);
    }

    public void UpdateUITitle(string title){
        MainUI.transform.Find("LevelLabel").GetComponent<TMPro.TextMeshProUGUI>().text = title;
    }

    //public void SetScore(int newScore){
    //    ScoreLabel.GetComponent<TMPro.TextMeshProUGUI>().text = newScore.ToString(); 
    //}


    //Hearts
    public void InitialiseHealth(EntityStatsScriptableObject healthObj){
        healthObj.HealthChangedEvent.AddListener(ChangeHealth);

        DrawHearts(healthObj.health, healthObj.maxHealth);
    }
    public void DrawHearts(int newHealth, int maxHealth)
    {
        ClearHearts();

        float maxHealthRemainder = maxHealth % 3;
        int heartsToMake = (maxHealth / 3) + (int)maxHealthRemainder;
        for (int i = 0; i < heartsToMake; i++)
        {
            CreateEmptyHeart();
        }
        for (int i = 0; i < hearts.Count; i++)
        {
            int heartStatusRemainder = (int)Mathf.Clamp(newHealth - (i*3), 0, 3);
            hearts[i].ChangeSkull((SkullStatus)heartStatusRemainder);
        }
    }
    public void CreateEmptyHeart()
    {
        GameObject newHeart = Instantiate(heartPrefab);
        newHeart.transform.SetParent(HealthBar.transform);

        HealthHeart heartComponent = newHeart.GetComponent<HealthHeart>();
        heartComponent.ChangeSkull(SkullStatus.None);
        hearts.Add(heartComponent);
    }
    public void ClearHearts()
    {
        foreach (Transform t in HealthBar.transform)
        {
            Destroy(t.gameObject);
        }
        hearts = new List<HealthHeart>();
    }
    public void ChangeHealth(int newHealth, int maxHealth){
        Debug.Log("Changing UI health: " + (float)newHealth / maxHealth);

        DrawHearts(newHealth, maxHealth);
    }

    // public void ShowPauseOverlay(){
    //     PauseMenu.SetActive(true);
    //     MainUI.SetActive(false);
    // }
}
