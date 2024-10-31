using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingState : BasePlayerState
{

    private void Awake(){
        stateEnum = EnumBus.PLAYER_STATES.FALLING;
    }
    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnUpdate(){
        base.OnUpdate();
        
        if (InputManager.dashWasPressed){
            ParentStateMachine.TransitionStates(EnumBus.PLAYER_STATES.DASHING);
        }

        if (playerRef.isGrounded){
            ParentStateMachine.TransitionStates(EnumBus.PLAYER_STATES.IDLE);
        }

    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        var newVel = playerRef._rb.velocity;

        newVel.y += playerRef.moveStats.gravity * playerRef.moveStats.gravityOnReleaseMultiplier * Time.fixedDeltaTime;

        playerRef._rb.velocity = newVel;



    }


    public override void OnHurt()
    {
        base.OnHurt();
    }

    public override void OnExit(){
        base.OnExit();
    }

}
