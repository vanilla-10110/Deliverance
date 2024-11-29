using UnityEngine;

public class AngelSmiteAttackState : BaseAngelState<AngelEnemyStateManager.ANGEL_STATES> {

    public AngelSmiteAttackState(AngelStateContext context, AngelEnemyStateManager.ANGEL_STATES stateKey, AngelEnemyStateManager esm) : base(context, stateKey, esm) {

    }


    public override void EnterState(){
        // anything this state should do as it initially gets called
        Context.AngelRef._animator.SetTrigger("TriggerSmite");
        Context.RigidBody.velocity = Vector2.zero;


    }


    public override void UpdateState(){
        // should mainly be used for checking the condition for when to change to another state
        if (Context.AngelRef._animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.4){
            return;
        }

        if (Context.lastPlayerDirection == 1){
            // Debug.Log("player in right smite area");
            Context.RightSmiteArea.gameObject.SetActive(true);
            Context.LeftSmiteArea.gameObject.SetActive(false);
        }
        else {
            // Debug.Log("player in left smite area");
            Context.RightSmiteArea.gameObject.SetActive(false);
            Context.LeftSmiteArea.gameObject.SetActive(true);
        }
    }


    public override void FixedUpdateState(){
        // should mainly be used for any physics calcs - like moving a rigidbody
    }


    public override void ExitState(){
        // anything this states should do before the state machine switches to another state
        Context.RightSmiteArea.gameObject.SetActive(false);
        Context.LeftSmiteArea.gameObject.SetActive(false);
    }

    public override AngelEnemyStateManager.ANGEL_STATES GetNextState(){
        // Debug.Log(StateKey);
        if (Context.AngelRef.enemyStats.health <= 0 ){
            return AngelEnemyStateManager.ANGEL_STATES.DEAD;
        }
        if (

            !(Context.AngelRef._animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        ){
            return AngelEnemyStateManager.ANGEL_STATES.PATROLLING;
        }

        return StateKey;
    }

    public override void OnTriggerEnter(Collider collider){}

    public override void OnTriggerStay(Collider collider){}

    public override void OnTriggerExit(Collider collider){}
}