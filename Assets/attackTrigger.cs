using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackTrigger : MonoBehaviour
{

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
    private void AttackEvent()
    {
        Debug.Log("YA");
        MeleeEnemy attackScript = this.transform.parent.GetComponent<MeleeEnemy>();
        attackScript.DamagePlayer();
    }
}
