using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AngelPatrollingState : BaseAngelState<AngelEnemyStateManager.ANGEL_STATES> {

    private Task<AngelEnemyStateManager.ANGEL_STATES> ChangeToIdleTimer;


    public AngelPatrollingState(AngelStateContext context, AngelEnemyStateManager.ANGEL_STATES stateKey, AngelEnemyStateManager esm) : base(context, stateKey, esm) {}

    public override void EnterState(){
        // anything this state should do as it initially gets called
        ChangeToIdleTimer = PatrolWait(Random.Range(2f, 5f));
        Context.AngelRef._animator.SetBool("isWalking", true);
        
    }

    public override async void UpdateState(){
        // should mainly be used for checking the condition for when to change to another state

        CheckColliderForPlayer(Context.LeftPlayerDetectionArea, -1);
        if (!Context.playerDetected) CheckColliderForPlayer(Context.RightPlayerDetectionArea, 1);


        await ChangeToIdleTimer;

    }
    


    public override void FixedUpdateState(){
        // should mainly be used for any physics calcs - like moving a rigidbody
        Context.RigidBody.velocity = new Vector2(2 * (Context.isFacingRight == true ? 1 : -1), Context.RigidBody.velocity.y);
    }


    public override void ExitState(){
        // anything this states should do before the state machine switches to another state
    }

    public override AngelEnemyStateManager.ANGEL_STATES GetNextState(){
        if (Context.AngelRef.enemyStats.health <= 0 ){
            return AngelEnemyStateManager.ANGEL_STATES.DEAD;
        }
        
        if (Context.playerDetected){
            return AngelEnemyStateManager.ANGEL_STATES.CHASING;
        }

        if (ChangeToIdleTimer.IsCompleted){
            return AngelEnemyStateManager.ANGEL_STATES.IDLE;
        }

        return StateKey;
    }

    private async Task<AngelEnemyStateManager.ANGEL_STATES> PatrolWait(float SecondsToTransition){


        await Task.Delay((int)(SecondsToTransition * 1000));

        return AngelEnemyStateManager.ANGEL_STATES.IDLE;
    }

    public override void OnTriggerEnter(Collider collider){
    }

    public override void OnTriggerStay(Collider collider){}

    public override void OnTriggerExit(Collider collider){}
}