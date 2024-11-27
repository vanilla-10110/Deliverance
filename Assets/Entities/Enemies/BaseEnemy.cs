using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    public int StartingHealth;

    public EntityStatsScriptableObject enemyStats;
    [SerializeField] private Hitbox _hitbox;

    public SpriteRenderer _spriteRenderer;
    public Animator _animator;

    [SerializeField] protected List<Collider2D> playerDetectionAreas;

    [SerializeField] private bool destroyEntityOnDead = true;

    private void Awake() {
        enemyStats = new()
        {
            health = StartingHealth
        };
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