using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
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

    [Header("Toggables")]
    [SerializeField] bool startOnMenu = false; 

    // to use the game manager - call GameManager.Instance and then any function you can find here
    public static GameManager Instance {get; private set;}

    private void Awake(){
        DontDestroyOnLoad(gameObject);

        if (Instance != null && Instance != this){
            Destroy(this.gameObject);
        }
        else {
            Instance = this;
        }
    }

    private void Start(){
        SignalBus.StartMenuTriggerSignal.Connect(OnStartMenuTrigger);

        if (startOnMenu == true){
            SignalBus.StartMenuTriggerSignal.Emit();
        }

        else{
            uiManager.ShowMainUI();
        }

        gameStats.currentSceneName = SceneManager.GetActiveScene().name;
        
        UpdateUI();
    }

    public void MovePlayerToSpawnpoint(){
        // playerRef.transform.position = sceneManager.GetSpawnpoint(sceneManager.currentSpawnPointId).position;
        sceneManager.MoveGameobjectToSpawnpoint(playerRef.gameObject);
        // add other reset actions
    }

    private void OnStartMenuTrigger(){
        Debug.Log("start menu triggered");
        uiManager.ShowStartMenu();
    }

    public void LoadLevelFromName(string sceneName){

        sceneManager.RemoveAllSpawnpoints();
        sceneManager.LoadScene(sceneName);

        gameStats.currentSceneName = sceneName;
        UpdateUI();
        uiManager.ShowMainUI();
        
        // playerRef.transform.position = sceneManager.GetSpawnpoint(sceneManager.currentSpawnPointId).position;
    }

    public void LoadLevelFromNameWithTarget(string sceneName, int targetSpawnID){
        sceneManager.RemoveAllSpawnpoints();
        sceneManager.SetSpawnpoint(targetSpawnID);
        sceneManager.LoadScene(sceneName);

        gameStats.currentSceneName = sceneName;
        UpdateUI();
        uiManager.ShowMainUI();

        // playerRef.transform.position = sceneManager.GetSpawnpoint(targetSpawnID).position;
    }


    public void GameOverActions(){
        uiManager.ShowGameOverScreen();
    }

    public void ChangeWealth(int value){
        gameStats.wealth += value;
        UpdateUI();
    }

    private void UpdateUI(){
        uiManager.UpdateUITitle(gameStats.currentSceneName);
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
