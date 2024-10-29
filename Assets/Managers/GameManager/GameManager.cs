using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameStats gameStats;

    // to use the game manager - call GameManager.Instance and then any function you can find here
    public static GameManager Instance {get; private set; } 

    private void Awake(){
        if (Instance != null && Instance != this){
            Destroy(this);
        }
        else {
            Instance = this;
        }
    }

    private void Start(){
        UpdateUI();
    }

    public void  RestartScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    public void GameOverActions(){
        UIManager.Instance.ShowGameOverScreen();
    }

    public void ChangeWealth(int value){
        gameStats.wealth += value;
        UpdateUI();
    }

    private void UpdateUI(){
        UIManager.Instance.SetScore(gameStats.wealth);
    }

}
