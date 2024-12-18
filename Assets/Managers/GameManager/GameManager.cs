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

    [NonSerialized] public EnumBus.GAME_STATE gameState = EnumBus.GAME_STATE.DUNGEON;

    [Header("References")]
    [SerializeField] private UIManager uiManager;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private ScenesManager sceneManager;
    public  GameStats gameStats;
    public  EntityStatsScriptableObject playerStats;

    [SerializeField] private AudioClip _startMenuSong;
    [SerializeField] private AudioClip _hubSong;


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
        SignalBus.StartMenuTriggerSignal.AddListener(OnStartMenuTrigger);

        if (startOnMenu == true){
            SignalBus.StartMenuTriggerSignal.Invoke();
        }

        else{
            uiManager.ShowMainUI();
        }

        gameStats.currentSceneName = SceneManager.GetActiveScene().name;
        
        uiManager.InitialiseHealth(playerStats);

        UpdateUI();

    }

    public void MovePlayerToSpawnpoint(){
        // playerRef.transform.position = sceneManager.GetSpawnpoint(sceneManager.currentSpawnPointId).position;
        sceneManager.MoveGameobjectToSpawnpoint(playerRef.gameObject);
        // add other reset actions
    }

    private void OnStartMenuTrigger(){
        Debug.Log("start menu triggered");
        gameStats.currentGameState = EnumBus.GAME_STATE.HUB;
        uiManager.ShowStartMenu();
        SoundManager.Instance.PlayBackgroundTrack(_startMenuSong);
    }

    public void LoadLevelFromName(string sceneName){
        if (sceneName == "Hub"){
            SoundManager.Instance.PlayBackgroundTrack(_hubSong);
        }

        sceneManager.RemoveAllSpawnpoints();
        sceneManager.LoadScene(sceneName);

        gameStats.currentSceneName = sceneName;
        UpdateUI();
        uiManager.ShowMainUI();
    }



    public void LoadLevelFromNameWithTarget(string sceneName, int targetSpawnID){
        sceneManager.RemoveAllSpawnpoints();
        sceneManager.SetSpawnpoint(targetSpawnID);
        sceneManager.LoadScene(sceneName);

        gameStats.currentSceneName = sceneName;
        UpdateUI();
        uiManager.ShowMainUI();
    }

    public void GameOverActions(){
        gameState = EnumBus.GAME_STATE.DEAD;
        uiManager.ShowGameOverScreen();
    }

    public void WinGameActions(){
        gameState = EnumBus.GAME_STATE.WIN;
        uiManager.ShowWinScreen();
        StartCoroutine(SendBackToStart());
    }

    private IEnumerator SendBackToStart(){
        yield return new WaitForSeconds(35);
        // LoadLevelFromName("MainScene");
        ExitGame();
    }

    public void RestartFromSpawnpoint(){
        MovePlayerToSpawnpoint();
        uiManager.ShowMainUI();
        playerRef.ResetPlayerState();
        playerStats.IncreaseHealth(10);
    }

    public void ChangeWealth(int value){
        gameStats.wealth += value;
        UpdateUI();
    }

    private void UpdateUI(){
        uiManager.UpdateUITitle(gameStats.currentSceneName);
        //uiManager.SetScore(gameStats.wealth);
    }

    public void ExitGame(){
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

}
