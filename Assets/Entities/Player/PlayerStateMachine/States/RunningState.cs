using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningState : BasePlayerState
{

    private void Awake(){
        stateEnum = EnumBus.PLAYER_STATES.RUNNING;
    }
    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnUpdate(){
        base.OnUpdate();

        if (InputManager.movement.x == 0 && playerRef.isGrounded){
            ParentStateMachine.TransitionStates(EnumBus.PLAYER_STATES.IDLE);
        }

        if (InputManager.jumpWasPressed){
            ParentStateMachine.TransitionStates(EnumBus.PLAYER_STATES.JUMPING);
        }

        if (InputManager.dashWasPressed){
            ParentStateMachine.TransitionStates(EnumBus.PLAYER_STATES.DASHING);
        }

        if (!playerRef.isGrounded){
            ParentStateMachine.TransitionStates(EnumBus.PLAYER_STATES.FALLING);

        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        playerRef._rb.velocity = Vector2.ClampMagnitude(playerRef._rb.velocity, playerRef.moveStats.maxRunSpeed);

        Vector2 targetVelocity = new Vector2(InputManager.movement.x * playerRef.moveStats.maxRunSpeed, playerRef._rb.velocity.y);

        playerRef._rb.velocity = Vector2.Lerp(playerRef._rb.velocity, targetVelocity, playerRef.moveStats.groundAcceleration * Time.fixedDeltaTime);
    }


    public override void OnHurt()
    {
        base.OnHurt();
    }

    public override void OnExit(){
        base.OnExit();
    }

}
