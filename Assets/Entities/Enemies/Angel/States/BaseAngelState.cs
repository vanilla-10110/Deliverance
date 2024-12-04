
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public abstract class BaseAngelState<EState> : BaseState<AngelEnemyStateManager.ANGEL_STATES> {

    protected AngelStateContext Context;
    protected AngelEnemyStateManager Esm;
    protected List<Collider2D> playerDetectColliderContacts = new();

    public BaseAngelState(AngelStateContext context, AngelEnemyStateManager.ANGEL_STATES stateKey, AngelEnemyStateManager esm) : base(stateKey) {
        Context = context;
        Esm = esm;
    }

    protected void TurnAngel(int direction){
        if (direction == 1){
            Context.AngelRef._spriteRenderer.flipX = true;
            Context.isFacingRight = true;
        }
        else if (direction == -1) {
            Context.AngelRef._spriteRenderer.flipX = false;
            Context.isFacingRight = false;
        }
    }

    protected void CheckColliderForPlayer(Collider2D collider, int colliderDirection){
        
        collider.GetContacts(playerDetectColliderContacts);

        if (playerDetectColliderContacts.Count > 0){

            if (playerDetectColliderContacts.Exists(c => 
                c.gameObject.CompareTag("Hitbox") &&
                LayerMask.LayerToName(c.gameObject.layer) == "Player"
            )){
                Context.playerDetected = true;
                Context.lastPlayerDirection = colliderDirection; 
            }
            else Context.playerDetected = false;
        } 
    }
}