using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCountKey : BaseKeyObject
{
    [SerializeField] private List<BaseEnemy> _enemiesArray;

    private void Start(){
        foreach (BaseEnemy enemy in _enemiesArray){
            enemy.EnemyDefeated.AddListener((BaseEnemy enemy) => {
                _enemiesArray.Remove(enemy);
            });
        }
    }

    private void Update(){
        // foreach (BaseEnemy enemy in _enemiesArray){
        //     if (enemy == null){
                
        //     }
        // }
        
        if (_currentState == KEY_STATE.LOCKED && _enemiesArray.Count <= 0){
            SetKeyState(KEY_STATE.UNLOCKED);
        }
    }

}
