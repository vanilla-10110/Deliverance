using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingState : BasePlayerState
{
    protected float vertical;
    protected float speed = 8f;
    protected bool isClimbing;

    private void Awake(){
        stateEnum = EnumBus.PLAYER_STATES.CLIMBING;
    }
    
    public override void OnEnter()
    {
        base.OnEnter();
        playerRef.numberOfJumpsUsed = 0;
        playerRef.numberOfDashesUsed = 0;
    }

    public override void OnUpdate(){
        base.OnUpdate();

        vertical = Input.GetAxisRaw("Vertical");

        if (playerRef.isClimbable && Mathf.Abs(vertical) > 0f)
        {
            isClimbing = true;
        }
        else
        {
            if (playerRef.isGrounded)
            {
                ParentStateMachine.TransitionStates(EnumBus.PLAYER_STATES.IDLE);
            }

            if (!playerRef.isGrounded)
            {
                ParentStateMachine.TransitionStates(EnumBus.PLAYER_STATES.FALLING);
            }
        }

        if (InputManager.dashWasPressed && (playerRef.numberOfDashesUsed >= moveStatsRef.numberOfDashesAllowed))
        {
            ParentStateMachine.TransitionStates(EnumBus.PLAYER_STATES.DASHING);
        }

        
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        if (isClimbing)
        {
            playerRef.velocity = new Vector2(playerRef.velocity.x, vertical * speed);
        }
    }


    public override void OnHurt()
    {
        base.OnHurt();
    }

    public override void OnExit(){
        base.OnExit();
        isClimbing = false;
    }

}
