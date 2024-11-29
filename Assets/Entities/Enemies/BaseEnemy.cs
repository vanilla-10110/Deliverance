using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseEnemy : MonoBehaviour
{
    public int StartingHealth;

    public EntityStatsScriptableObject enemyStats;
    public Hitbox _hitbox;
    [NonSerialized] private Rigidbody2D _rb; 
    public SpriteRenderer _spriteRenderer;
    public Animator _animator;

    public UnityEvent<BaseEnemy> EnemyDefeated = new();

    protected Vector2 _environmentalVelocity;

    [SerializeField] protected List<Collider2D> playerDetectionAreas;

    [SerializeField] private bool destroyEntityOnDead = true;
    protected void Awake() {
        _rb = GetComponent<Rigidbody2D>();
        enemyStats = new()
        {
            health = StartingHealth
        };
    }


    protected void Start(){
        if (_hitbox){
            _hitbox.HitDetected.AddListener(OnHitDetected);
        }
        
        enemyStats.HealthDepletedEvent.AddListener(OnHealthDepletedActions);
        _hitbox.HitboxIntersectingForce.AddListener((Vector2 force) =>{
            _environmentalVelocity = new Vector2(force.x, 0); // adds a opposing force if inside another hitbox
        });
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

    protected void FixedUpdate(){
        _rb.velocity += _environmentalVelocity * Time.fixedDeltaTime;
        _environmentalVelocity = Vector2.zero;
    }

    void OnDestroy(){
        SignalBus.DestroyedEntityEvent.Invoke();
    }
}