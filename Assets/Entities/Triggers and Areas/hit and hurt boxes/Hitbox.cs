using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Hitbox : MonoBehaviour
{
    // This class is for the area of the entity that is receiving the hit, and invoked an event when it detects one.

    // add any hurtboxes you want this hitbox to ignore into the editor, since some might overlap on the same entity
    public Hurtbox[] ignoredHurtboxes;
    [SerializeField] private ParticleSystem _hitParticleSystem;
    /* 
    from another script all you gotta do is add a listener to this event and 
    do whatever with it - ie. decrease health / play an animation

    it passes a int paramater which is meant to be the damage it has recieved
    */
    [NonSerialized] public UnityEvent<int> HitDetected = new();
    [NonSerialized] public UnityEvent<float> HitboxIntersecting = new();

    private Collider2D _coll;

    // private ParticleSystem hitParticleTrigger;

    [Header("Debug")]
    [SerializeField] private bool drawDebugBoxes = true;

    private void Start(){
        _coll = GetComponent<Collider2D>();
        // _hitParticleSystem = GetComponent<ParticleSystem>();
    }

    private void Update(){
        if (drawDebugBoxes){
            DrawDebugBoxes();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider){
        // do nothing if collider is meant to be ignored
        if (ignoredHurtboxes.Length > 0){
            if (Array.Exists(ignoredHurtboxes, a => a == collider)){
                return;
            }
        }

        if (collider.gameObject.CompareTag("Hurtbox")){
            // Debug.Log("hurtbox entered hitbox: " + collider.gameObject.GetComponent<Hurtbox>().damageValue + " damage");
            if (LayerMask.LayerToName(gameObject.layer) == collider.gameObject.GetComponent<Hurtbox>().ownerTag){
                return;
            }
            HitDetected.Invoke(collider.gameObject.GetComponent<Hurtbox>().damageValue);

            _hitParticleSystem.Play();


        }
    }

    private void OnTriggerStay2D(Collider2D collider){
        if (collider.gameObject.CompareTag("Hitbox")){
            HitboxIntersecting.Invoke(Vector3.Angle(gameObject.transform.position, collider.transform.position));
        }
    }

    private void DrawDebugBoxes(){
        // debugging box
        Debug.DrawRay(new Vector2(_coll.bounds.center.x - _coll.bounds.size.x / 2, _coll.bounds.center.y - _coll.bounds.size.y / 2), Vector2.right * _coll.bounds.size.x, Color.yellow);
        Debug.DrawRay(new Vector2(_coll.bounds.center.x + _coll.bounds.size.x / 2, _coll.bounds.center.y - _coll.bounds.size.y / 2), Vector2.up * _coll.bounds.size.y, Color.yellow);
        Debug.DrawRay(new Vector2(_coll.bounds.center.x + _coll.bounds.size.x / 2, _coll.bounds.center.y + _coll.bounds.size.y / 2), Vector2.left * _coll.bounds.size.x, Color.yellow);
        Debug.DrawRay(new Vector2(_coll.bounds.center.x - _coll.bounds.size.x / 2, _coll.bounds.center.y + _coll.bounds.size.y / 2), Vector2.down * _coll.bounds.size.y, Color.yellow);

    }

}
