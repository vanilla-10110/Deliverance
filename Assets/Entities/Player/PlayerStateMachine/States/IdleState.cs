using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class IdleState : BasePlayerState
{
    
    private void Awake(){
        stateEnum = EnumBus.PLAYER_STATES.IDLE;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        
        playerRef.numberOfJumpsUsed = 0;
        playerRef.numberOfDashesUsed = 0;


    }

    public override void OnUpdate(){
        base.OnUpdate();

        if (InputManager.jumpWasPressed){
            ParentStateMachine.TransitionStates(EnumBus.PLAYER_STATES.JUMPING);
        }

        if (InputManager.dashWasPressed){
            ParentStateMachine.TransitionStates(EnumBus.PLAYER_STATES.DASHING);
        }

        if (InputManager.movement.x != 0f){
            ParentStateMachine.TransitionStates(EnumBus.PLAYER_STATES.RUNNING);
        }

        if (!playerRef.isGrounded){
            ParentStateMachine.TransitionStates(EnumBus.PLAYER_STATES.FALLING);

        }

        if (InputManager.climbWasPressed && playerRef.isClimbable)
        {
            ParentStateMachine.TransitionStates(EnumBus.PLAYER_STATES.CLIMBING);
        }

    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        
        playerRef.velocity.y = 0f;

        if (Mathf.Abs(playerRef.velocity.x) < 0.01){
            playerRef.velocity.x = 0f;
        }
        else if (playerRef.velocity.x != 0f){ 
            playerRef.velocity.x += (playerRef.velocity.x * -1) * moveStatsRef.groundDecceleration * Time.fixedDeltaTime;
        }


    }

    public override void OnHurt()
    {
        base.OnHurt();
    }

    public override void OnExit(){
        base.OnExit();
    }
}
