using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTigger : MonoBehaviour
{
    [SerializeField] private string triggerOwnerTag;

    void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.CompareTag("Hurtbox")){
            if (collider.gameObject.GetComponent<Hurtbox>().ownerTag == triggerOwnerTag){
                TriggerEffect();
            }
        }
    }

    public void TriggerEffect(){}
}
