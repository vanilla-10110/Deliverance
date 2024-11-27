using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIdleState : BasePlayerState<PlayerStateManager.PLAYER_STATES>
{

    public PlayerIdleState(PlayerStateContext context, PlayerStateManager.PLAYER_STATES stateKey,  PlayerStateManager _psm) : base(context, stateKey, _psm){
        StateKey = stateKey;
        Context = context;
        psm = _psm;
    }

    public override void EnterState(){
        Context.Player.numberOfJumpsUsed = 0;
        Context.Player.numberOfDashesUsed = 0;
    }   
    public override void ExitState(){

        Context.SetPrevState(StateKey);

    }
    public override void UpdateState(){}
    public override PlayerStateManager.PLAYER_STATES GetNextState(){

        if (InputManager.jumpWasPressed){
            return PlayerStateManager.PLAYER_STATES.JUMPING;
        }

        if (InputManager.dashWasPressed){
            return PlayerStateManager.PLAYER_STATES.DASHING;
        }

        if (InputManager.movement.x != 0f){
            return PlayerStateManager.PLAYER_STATES.RUNNING;
        }

        if (!Context.Player.isGrounded){
            return PlayerStateManager.PLAYER_STATES.FALLING;
        }

        if (InputManager.climbWasPressed && Context.Player.isClimbable){
            return PlayerStateManager.PLAYER_STATES.CLIMBING;
        }

        return StateKey;
    }

    public override void FixedUpdateState(){
        Context.Player.velocity.y = 0f;

        if (Mathf.Abs(Context.Player.velocity.x) < 0.01){
            Context.Player.velocity.x = 0f;
        }
        else if (Context.Player.velocity.x != 0f){ 
            Context.Player.velocity.x += (Context.Player.velocity.x * -1) * Context.PlayerMovementStats.groundDecceleration * Time.fixedDeltaTime;
        }
    }

    public override void OnTriggerEnter(Collider collider){}
    public override void OnTriggerStay(Collider collider){}
    public override void OnTriggerExit(Collider collider){}
    

    // public override void OnFixedUpdate()
    // {
    //     base.OnFixedUpdate();
        
        


    // }

    // public override void OnHurt()
    // {
    //     base.OnHurt();
    // }

    // public override void OnExit(){
    //     base.OnExit();
    // }
}
