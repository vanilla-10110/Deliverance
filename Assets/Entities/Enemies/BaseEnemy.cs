using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    public EntityStatsScriptableObject enemyStats;
    [SerializeField] private Hitbox _hitbox;

    private void Awake(){
        enemyStats = new();
    }

    private void Start(){
        if (_hitbox){
            _hitbox.HitDetected.AddListener(OnHitDetected);
        }
        // death actions
        enemyStats.HealthDepletedEvent.AddListener(() => {Destroy(gameObject);});
    }

    public void OnHitDetected(int damageValue){
        enemyStats.DecreaseHealth(damageValue);
        Debug.Log("i am damaged, health is now " + enemyStats.health);
    }
}