// using UnityEngine;

using UnityEngine;

public class AngelDeadState : BaseAngelState<AngelEnemyStateManager.ANGEL_STATES> {

    public AngelDeadState(AngelStateContext context, AngelEnemyStateManager.ANGEL_STATES stateKey, AngelEnemyStateManager esm) : base(context, stateKey, esm) {
    }


    public override void EnterState(){
        // anything this state should do as it initially gets called
        Context.AngelRef._animator.SetTrigger("TriggerDeath");
    }


    public override void UpdateState(){
        // should mainly be used for checking the condition for when to change to another state
    }


    public override void FixedUpdateState(){
        // should mainly be used for any physics calcs - like moving a rigidbody
    }


    public override void ExitState(){
        // anything this states should do before the state machine switches to another state
    }

    public override AngelEnemyStateManager.ANGEL_STATES GetNextState(){

        return StateKey;
    }

    public override void OnTriggerEnter(Collider collider){}

    public override void OnTriggerStay(Collider collider){}

    public override void OnTriggerExit(Collider collider){}
}