using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    private UnityEngine.SceneManagement.Scene currentScene;

    private Vector3 currentCheckpoint;
    public static ScenesManager Instance {get; private set;} 

    private void Awake(){
        if (Instance != null && Instance != this){
            Destroy(this);
        }
        else {
            Instance = this;
        }
    }

    public void LoadNewScene(string sceneName){
        Debug.Log("trying to load scene at path: " + sceneName);

        SceneManager.LoadScene(sceneName);

        currentScene = SceneManager.GetActiveScene();
    }

    public void SetCheckpoint(Vector3 newPosition){
        currentCheckpoint = newPosition;

        Debug.Log("Checkpoint set to "+ currentCheckpoint);
    }

    public void RestartOnCheckpoint(GameObject player){
        if (currentCheckpoint != null){
            Debug.Log("Restarting from checkpoint");
            player.transform.position = currentCheckpoint;
        }

    }

}
