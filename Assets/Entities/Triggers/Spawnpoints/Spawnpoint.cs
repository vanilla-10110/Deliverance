using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnpointTrigger : MonoBehaviour
{
    public int id;
    public Vector3 position;
    
    // all spawnpoints add themselves to the scene manager's list on load
    protected void Start(){
        // Debug.Log("adding spawn to list: " + id);
        position = transform.position;
        ScenesManager.Instance.AddSpawnpointToList(this);
    }
 
    // only one active spawn point at a time
    private void OnTriggerEnter2D(Collider2D collider){
        if (collider.CompareTag("Player")){
            ScenesManager.Instance.SetSpawnpoint(this.id);
        }
    }

    protected void OnDestroy(){
        // Debug.Log("removing spawn to list: " + id);

        ScenesManager.Instance.RemoveSpawnpoint(this);
    }
}
