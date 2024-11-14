using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTransitionTrigger : SpawnpointTrigger
{
    
    public string sceneNameToTranstionTo;
    public int targetSpawnpointId;

    public bool exitedAreaSinceSpawn = true;
 
    private void OnTriggerExit2D(Collider2D collider){
        if (collider.CompareTag("Player") && exitedAreaSinceSpawn == false){
            exitedAreaSinceSpawn = true;
        }
    }

    // only one active spawn point at a time
    private void OnTriggerEnter2D(Collider2D collider){
        if (collider.CompareTag("Player") && exitedAreaSinceSpawn == true){
            GameManager.Instance.LoadLevelFromNameWithTarget(sceneNameToTranstionTo, targetSpawnpointId);
        }
    }
}
