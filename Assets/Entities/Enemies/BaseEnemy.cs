using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseEnemy : MonoBehaviour
{
    public int StartingHealth;

    public EntityStatsScriptableObject enemyStats;
    public Hitbox _hitbox;
    public SpriteRenderer _spriteRenderer;
    public Animator _animator;

    public UnityEvent<BaseEnemy> EnemyDefeated = new();

    [SerializeField] protected List<Collider2D> playerDetectionAreas;

    [SerializeField] private bool destroyEntityOnDead = true;
    protected void Awake() {
        enemyStats = new()
        {
            health = StartingHealth
        };
    }


    protected void Start(){
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
        EnemyDefeated.Invoke(this);
        _hitbox.gameObject.SetActive(false);
        
        if (destroyEntityOnDead){
            Destroy(gameObject);
        }
    }

    void OnDestroy(){
        SignalBus.DestroyedEntityEvent.Invoke();
    }
}