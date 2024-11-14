
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName = "Entity Stats")]
public class EntityStatsScriptableObject : ScriptableObject {
    public int health = 5;
    public int maxHealth = 5;
    public int maxHealthLimit = 10;

    [System.NonSerialized]
    public UnityEvent<int> HealthChangedEvent;

    private void OnEnable() {
        health = maxHealth;
        if (HealthChangedEvent == null){
            HealthChangedEvent = new UnityEvent<int>();
        }
    }

    public void DecreaseHealth(int amount) {
        health -= amount;
    }

    public void IncreaseHealth(int amount) {
        health += amount;
    }
}