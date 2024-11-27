using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    private AsyncOperation loadSceneTask;

    public enum STATE {
        preLoading,
        loading,
        finishedLoading
    }

    public STATE currentState = STATE.finishedLoading;
    private string currentSceneName;

    private List<SpawnpointTrigger> spawnpointList = new();

    public static int currentSpawnPointId = -1;
    public static ScenesManager Instance {get; private set;} 

    private GameObject gameObjectHeld = null;

    private void Awake(){
        if (Instance != null){
            Destroy(this.gameObject);
        }
        else {
            Instance = this;
        }
    }

    public void LoadScene(string sceneName){
        currentSceneName = sceneName;
        currentState = STATE.preLoading;
    }

    private void Update(){
        if (currentState == STATE.preLoading && currentSceneName != null){
            loadSceneTask = SceneManager.LoadSceneAsync(currentSceneName);
            
            currentState = STATE.loading;
        }
        if (currentState == STATE.loading && loadSceneTask.isDone){
            currentState = STATE.finishedLoading;
            SignalBus.newSceneLoaded.Invoke();
        }

        if (gameObjectHeld != null){
            SpawnpointTrigger spawnPoint = GetSpawnpoint(currentSpawnPointId);
            if (spawnPoint != null){
                if (spawnPoint is RoomTransitionTrigger){
                    (spawnPoint as RoomTransitionTrigger).exitedAreaSinceSpawn = false;
                }
                gameObjectHeld.transform.position = spawnPoint.position; 
                gameObjectHeld = null;
            }
        }
    }
    
    public void SetSpawnpoint(int spawnPointID){
        currentSpawnPointId = spawnPointID;
        SignalBus.spawnPointUpdated.Invoke();
    }

    public void AddSpawnpointToList(SpawnpointTrigger spawnPoint){
        spawnpointList.Add(spawnPoint);
    }

    public void MoveGameobjectToSpawnpoint(GameObject gameObject){
        gameObjectHeld = gameObject;
    }

    public void RemoveSpawnpoint(SpawnpointTrigger spawnpoint){
        spawnpointList.Remove(spawnpoint);
    }

    public void RemoveAllSpawnpoints(){
        spawnpointList.Clear();
        currentSpawnPointId = -1;
    }

    public SpawnpointTrigger GetSpawnpoint(int spawnPointId){
        return spawnpointList.Find(p => p.id == spawnPointId);
    }

}
