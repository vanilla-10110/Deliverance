using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    public EntityStatsScriptableObject enemyStats;
    [SerializeField] private Hitbox _hitbox;
    public SpriteRenderer _spriteRenderer;
    public Animator _animator;

    [SerializeField] protected List<Collider2D> playerDetectionAreas;

    [SerializeField] private bool destroyEntityOnDead = true;
 
    private void Awake(){
        enemyStats = new();
    }

    private void Start(){
        // _ESM = GetComponent<EnemyStateMachine>();
        // _ESM.animatorRef = GetComponent<Animator>();
        // _ESM.enemyRef = this;

        if (_hitbox){
            _hitbox.HitDetected.AddListener(OnHitDetected);
        }
        // death actions
        enemyStats.HealthDepletedEvent.AddListener(OnHealthDepletedActions);
    }

    public void OnHitDetected(int damageValue){
        enemyStats.DecreaseHealth(damageValue);
        Debug.Log("i am damaged, health is now " + enemyStats.health);
    }

    void OnHealthDepletedActions(){
        SignalBus.DestroyedEntityEvent.Invoke();
        
        if (destroyEntityOnDead){
            Destroy(gameObject);
        }
    }

    void OnDestroy(){
        SignalBus.DestroyedEntityEvent.Invoke();
    }
}