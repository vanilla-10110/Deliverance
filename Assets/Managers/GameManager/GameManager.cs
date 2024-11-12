using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private ScenesManager sceneManager;

    public GameStats gameStats;

    public Player playerRef = null;

    // to use the game manager - call GameManager.Instance and then any function you can find here
    public static GameManager Instance {get; private set;} 

    private void Awake(){
        DontDestroyOnLoad(gameObject);

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

    public void RestartCheckpoint(){
        sceneManager.RestartOnCheckpoint(playerRef.gameObject);
    }

    public void LoadLevelFromPath(string scenePath){
        sceneManager.LoadNewScene(scenePath);
    }

    public void GameOverActions(){
        uiManager.ShowGameOverScreen();
    }

    public void SetCheckpoint(Vector3 newCheckpoint){
        sceneManager.SetCheckpoint(newCheckpoint);
    }

    public void ChangeWealth(int value){
        gameStats.wealth += value;
        UpdateUI();
    }

    private void UpdateUI(){
        uiManager.SetScore(gameStats.wealth);
    }

    public void ExitGame(){
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

}
