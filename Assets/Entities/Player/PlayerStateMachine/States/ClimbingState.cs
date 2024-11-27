using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimbingState : BasePlayerState<PlayerStateManager.PLAYER_STATES>
{
    protected float vertical;
    protected float speed = 8f;
    protected bool isClimbing;
    
    public PlayerClimbingState(PlayerStateContext context, PlayerStateManager.PLAYER_STATES stateKey, PlayerStateManager _psm) : base(context, stateKey, _psm) {
        Context = context;
        psm = _psm;
    }

    public override void EnterState(){
        Context.Player.numberOfJumpsUsed = 0;
        Context.Player.numberOfDashesUsed = 0;
    }

    public override void ExitState(){
        isClimbing = false;
        Context.SetPrevState(StateKey);
    }

    public override void UpdateState(){
        vertical = Input.GetAxisRaw("Vertical");

        if (Context.Player.isClimbable && Mathf.Abs(vertical) > 0f) {
            isClimbing = true;
        } else {
            isClimbing = false;
        }
    }

    public override void FixedUpdateState(){
        if (isClimbing) {
            Context.Player.velocity = new Vector2(Context.Player.velocity.x, vertical * speed);
        }
    }

    public override PlayerStateManager.PLAYER_STATES GetNextState(){
        if (!isClimbing){
            if (Context.Player.isGrounded) {
                return PlayerStateManager.PLAYER_STATES.IDLE;
            }
            if (!Context.Player.isGrounded) {
                return PlayerStateManager.PLAYER_STATES.FALLING;
            }
        }

         if (InputManager.dashWasPressed && (Context.Player.numberOfDashesUsed >= Context.PlayerMovementStats.numberOfDashesAllowed)) {
            return PlayerStateManager.PLAYER_STATES.DASHING;
        }

        return StateKey;
    }
    public override void OnTriggerEnter(Collider collider){}
    public override void OnTriggerStay(Collider collider){}
    public override void OnTriggerExit(Collider collider){}

    // public  void OnUpdate(){

    //     vertical = Input.GetAxisRaw("Vertical");

    //     if (playerRef.isClimbable && Mathf.Abs(vertical) > 0f)
    //     {
    //         isClimbing = true;
    //     }
    //     else
    //     {
    //         if (playerRef.isGrounded)
    //         {
    //             ParentStateMachine.TransitionStates(EnumBus.PLAYER_STATES.IDLE);
    //         }

    //         if (!playerRef.isGrounded)
    //         {
    //             ParentStateMachine.TransitionStates(EnumBus.PLAYER_STATES.FALLING);
    //         }
    //     }

    //     if (InputManager.dashWasPressed && (playerRef.numberOfDashesUsed >= moveStatsRef.numberOfDashesAllowed))
    //     {
    //         ParentStateMachine.TransitionStates(EnumBus.PLAYER_STATES.DASHING);
    //     }

        
    // }

    // public override void OnFixedUpdate()
    // {
    //     base.OnFixedUpdate();

    //     if (isClimbing)
    //     {
    //         playerRef.velocity = new Vector2(playerRef.velocity.x, vertical * speed);
    //     }
    // }


    // public override void OnHurt()
    // {
    //     base.OnHurt();
    // }

    // public override void OnExit(){
    //     base.OnExit();

    // }

}
