using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider){
        if (collider.CompareTag("Player")){
            GameManager.Instance.SetCheckpoint(transform.position);
        }
    }
}
