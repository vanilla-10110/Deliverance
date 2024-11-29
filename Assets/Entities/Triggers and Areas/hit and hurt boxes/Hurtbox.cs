using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hurtbox : MonoBehaviour
{
    // these are the area doing the hitting. it'll invoke an event if it detects a hitbox.
    private float activeDuration = 0f;
    [NonSerialized] public int damageValue = 0;

    [NonSerialized] public UnityEvent HitboxDetected = new();
    [NonSerialized] public UnityEvent AttackTimeFinished = new();
    [SerializeField] public string ownerTag;

    private Collider2D _coll;

    [Header("Debug")]
    [SerializeField] private bool drawDebugBoxes = false; 

    void Start(){
        _coll = GetComponent<Collider2D>();
        gameObject.SetActive(false);
    }

    void Update(){
        if (gameObject.activeSelf){
            activeDuration -= Time.deltaTime;
        }

        if (activeDuration <= 0f){
            gameObject.SetActive(false);
            activeDuration = 0f;
            damageValue = 0;
            AttackTimeFinished.Invoke();
        }

        if (drawDebugBoxes){
            DrawDebugBoxes();
        }
    }

    // for more control, i made it so you need to pass a duration and damage value which are both reset to 0 after duration has lapsed
    public void ActivateHurtBox(float Duration, int damage){
        activeDuration = Duration;
        damageValue = damage;
        gameObject.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D collider){
        // do nothing if collider is meant to be ignored
        if (collider.gameObject.CompareTag("Hitbox")){
            HitboxDetected.Invoke();
        }
    } 

    private void DrawDebugBoxes(){
        // debugging box
        Debug.DrawRay(new Vector2(_coll.bounds.center.x - _coll.bounds.size.x / 2, _coll.bounds.center.y - _coll.bounds.size.y / 2), Vector2.right * _coll.bounds.size.x, Color.magenta);
        Debug.DrawRay(new Vector2(_coll.bounds.center.x + _coll.bounds.size.x / 2, _coll.bounds.center.y - _coll.bounds.size.y / 2), Vector2.up * _coll.bounds.size.y, Color.magenta);
        Debug.DrawRay(new Vector2(_coll.bounds.center.x + _coll.bounds.size.x / 2, _coll.bounds.center.y + _coll.bounds.size.y / 2), Vector2.left * _coll.bounds.size.x, Color.magenta);
        Debug.DrawRay(new Vector2(_coll.bounds.center.x - _coll.bounds.size.x / 2, _coll.bounds.center.y + _coll.bounds.size.y / 2), Vector2.down * _coll.bounds.size.y, Color.magenta);

    }
}
