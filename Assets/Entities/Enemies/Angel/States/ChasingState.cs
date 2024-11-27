using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AngelChasingState : BaseAngelState<AngelEnemyStateManager.ANGEL_STATES> {

    // private Task<AngelEnemyStateManager.ANGEL_STATES> ChangeToIdleTimer;

    private List<Collider2D> _collidersInAttackArea = new(); 

    public AngelChasingState(AngelStateContext context, AngelEnemyStateManager.ANGEL_STATES stateKey, AngelEnemyStateManager esm) : base(context, stateKey, esm) {}

    public override void EnterState(){
        TurnAngel(Context.lastPlayerDirection);
        Context.AngelRef._animator.SetBool("isWalking", true);
        
    }

    public override void UpdateState(){
        // should mainly be used for checking the condition for when to change to another state
        CheckColliderForPlayer(Context.LeftPlayerDetectionArea, -1);
        if (!Context.playerDetected) CheckColliderForPlayer(Context.RightPlayerDetectionArea, 1);


        
    }

    public override void FixedUpdateState(){
        // should mainly be used for any physics calcs - like moving a rigidbody
        Context.RigidBody.velocity = new Vector2(3 * (Context.isFacingRight == true ? 1 : -1), Context.RigidBody.velocity.y);
    }

    public override void ExitState(){
        // anything this states should do before the state machine switches to another state
    }

    public override AngelEnemyStateManager.ANGEL_STATES GetNextState(){
        Debug.Log(StateKey);
        
        if(IsPlayerInSmite(Context.lastPlayerDirection == -1 ? Context.LeftSmiteArea : Context.RightSmiteArea)){
            return AngelEnemyStateManager.ANGEL_STATES.SMITE_ATTACK;
        }

        if (!Context.playerDetected){
            return AngelEnemyStateManager.ANGEL_STATES.IDLE;
        }

        return StateKey;
    }

    private bool IsPlayerInSmite(Collider2D collider){
        
        collider.GetContacts(_collidersInAttackArea);

        if (_collidersInAttackArea.Count > 0){
            if (_collidersInAttackArea.Exists(c => c.gameObject.CompareTag("Player"))){
                return true;
            }
        }

        return false;
    }


    public override void OnTriggerEnter(Collider collider){
        
    }

    public override void OnTriggerStay(Collider collider){}

    public override void OnTriggerExit(Collider collider){}
}