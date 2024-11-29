using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelEnemy : BaseEnemy
{
    private AngelEnemyStateManager _esm;
 
    private new void Awake(){
        base.Awake();
        _esm = GetComponent<AngelEnemyStateManager>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private new void Start() {
        base.Start();
        _esm.Init(this);
        
    }
}