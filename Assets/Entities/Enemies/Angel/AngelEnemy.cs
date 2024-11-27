using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelEnemy : BaseEnemy
{
    private AngelEnemyStateManager _esm;
    
 
    private void Awake(){
        _esm = GetComponent<AngelEnemyStateManager>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start(){
        _esm.Init(this);
    }

    private void Update(){

    }

    private void FixedUpdate(){
    }


}