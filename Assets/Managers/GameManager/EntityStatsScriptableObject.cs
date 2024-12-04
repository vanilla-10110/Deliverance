
using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName = "Entity Stats")]
public class EntityStatsScriptableObject : ScriptableObject {
    public int health = 5;
    public int maxHealth = 10;
    public int maxHealthLimit = 10;

    [System.NonSerialized]
    public UnityEvent<int, int> HealthChangedEvent; // current health, max health

    [System.NonSerialized]
    public UnityEvent HealthDepletedEvent;

    private void OnEnable() {
        // health = maxHealth;
        if (HealthChangedEvent == null){
            HealthChangedEvent = new UnityEvent<int, int>();
        }
        if (HealthDepletedEvent == null){
            HealthDepletedEvent = new UnityEvent();
        }
    }

    public void DecreaseHealth(int amount) {
        health -= Math.Abs(amount);

        HealthChangedEvent.Invoke(health, maxHealth);

        if (health <= 0){
            HealthDepletedEvent.Invoke();
        }
    }

    public void IncreaseHealth(int amount) {
        health = Mathf.Min(maxHealth, Math.Abs(health + amount));

        HealthChangedEvent.Invoke(health, maxHealth);
    }

    public void IncreaseMaxHealth(int amount){
        if (maxHealth + amount > maxHealthLimit){
            maxHealth = maxHealthLimit;
        }
        else maxHealth += amount;
    }
    public void DecreaseMaxHealth(int amount){
        if (maxHealth - amount < 0){
            maxHealth = 0;
        }
        else maxHealth -= amount;
    }

}