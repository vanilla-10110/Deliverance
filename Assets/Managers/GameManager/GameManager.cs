using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private UIManager uiManager;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private ScenesManager sceneManager;
    public GameStats gameStats;
    public Player playerRef = null;

    [SerializeReference] private string FirstLevelPath;
    [SerializeReference] private GameObject playerPrefab;

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
        SignalBus.StartMenuTriggerSignal.Connect(OnStartMenuTrigger);

        SignalBus.StartMenuTriggerSignal.Emit();


        UpdateUI();
    }

    public void OnStartMenuTrigger(){
        Debug.Log("start menu triggered");
        uiManager.ShowStartMenu();
    }

    public void RestartCheckpoint(){
        sceneManager.RestartOnCheckpoint(playerRef.gameObject);
    }

    public void LoadLevelFromName(string sceneName){
        playerRef.transform.position = new Vector3(0, 0, 0);

        uiManager.ShowMainUI();
        sceneManager.LoadNewScene(sceneName);
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
