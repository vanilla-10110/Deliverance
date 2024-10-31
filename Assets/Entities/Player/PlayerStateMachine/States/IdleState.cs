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



    }

    public override void OnUpdate(){
        base.OnUpdate();

        if (InputManager.jumpWasPressed){
            // Debug.Log("jump pressed");
            ParentStateMachine.TransitionStates(EnumBus.PLAYER_STATES.JUMPING);
        }

        if (InputManager.dashWasPressed){
            // Debug.Log("dash pressed");
            ParentStateMachine.TransitionStates(EnumBus.PLAYER_STATES.DASHING);
        }

        if (InputManager.movement.x != 0f){
            // Debug.Log("jump pressed");
            ParentStateMachine.TransitionStates(EnumBus.PLAYER_STATES.RUNNING);
        }

        if (!playerRef.isGrounded){
            ParentStateMachine.TransitionStates(EnumBus.PLAYER_STATES.FALLING);

        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        playerRef._rb.velocity = Vector2.Lerp(playerRef._rb.velocity, Vector2.zero, playerRef.moveStats.groundDecceleration * Time.fixedDeltaTime);

    }

    public override void OnHurt()
    {
        base.OnHurt();
    }

    public override void OnExit(){
        base.OnExit();
    }
}
