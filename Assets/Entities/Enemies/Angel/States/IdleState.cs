using System.Threading.Tasks;
using UnityEngine;

public class AngelIdleState : BaseAngelState<AngelEnemyStateManager.ANGEL_STATES> {

    private Task<AngelEnemyStateManager.ANGEL_STATES> ChangeToPatrolTimer;

    public AngelIdleState(AngelStateContext context, AngelEnemyStateManager.ANGEL_STATES stateKey, AngelEnemyStateManager esm) : base(context, stateKey, esm) {

    }

    public override void EnterState(){
        // anything this state should do as it initially gets called
        Context.RigidBody.velocity = new(0,0);
        ChangeToPatrolTimer = IdleWait(Random.Range(2f, 5f));
        TurnAngel(Random.Range(-1,1));
        Context.AngelRef._animator.SetBool("isWalking", false);
    }

    public override async void UpdateState(){
        // should mainly be used for checking the condition for when to change to another state

        CheckColliderForPlayer(Context.LeftPlayerDetectionArea, -1);
        if (!Context.playerDetected) CheckColliderForPlayer(Context.RightPlayerDetectionArea, 1);

        await ChangeToPatrolTimer;
    }

    public override void FixedUpdateState(){
        // should mainly be used for any physics calcs - like moving a rigidbody
    }

    public override void ExitState(){
        if (!Context.playerDetected){
            TurnAngel(
                Context.isFacingRight ? -1 : 1
            );
        }
    }

    public override AngelEnemyStateManager.ANGEL_STATES GetNextState(){
        Debug.Log(StateKey);

        if (Context.playerDetected){
            return AngelEnemyStateManager.ANGEL_STATES.CHASING;
        }

        if (ChangeToPatrolTimer.IsCompleted){
            return AngelEnemyStateManager.ANGEL_STATES.PATROLLING;
        }

        return StateKey;
    }

    private async Task<AngelEnemyStateManager.ANGEL_STATES> IdleWait(float SecondsToTransition){
        await Task.Delay((int)(SecondsToTransition * 1000));

        return AngelEnemyStateManager.ANGEL_STATES.PATROLLING;
    }

    public override void OnTriggerEnter(Collider collider){}

    public override void OnTriggerStay(Collider collider){}

    public override void OnTriggerExit(Collider collider){}
}