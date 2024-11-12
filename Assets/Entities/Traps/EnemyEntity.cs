using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEntity : MonoBehaviour
{
   
   private void OnTriggerEnter2D(Collider2D collider){
        if (collider.CompareTag("Player")){
            GameManager.Instance.RestartCheckpoint();
        }
   }
}
