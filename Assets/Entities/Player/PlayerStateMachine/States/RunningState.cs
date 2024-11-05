using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningState : BasePlayerState
{
    protected float jumpCayoteTime = 0f;
    protected bool cayoteBufferActive = false;

    private void Awake(){
        stateEnum = EnumBus.PLAYER_STATES.RUNNING;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        playerRef.numberOfJumpsUsed = 0;
        jumpCayoteTime = moveStatsRef.jumpCayoteTime;


        playerRef.animator.SetBool("isRunning", true);

    }

    public override void OnUpdate(){
        base.OnUpdate();

        // for jump cayote time
        if (!playerRef.isGrounded && jumpCayoteTime > 0f){
            cayoteBufferActive = true;
        }

        // reset cayote properties if on ground again
        if (playerRef.isGrounded){
            cayoteBufferActive = false;
            jumpCayoteTime = moveStatsRef.jumpCayoteTime;
        }        

        // for state transitions
        if (cayoteBufferActive){
            if (jumpCayoteTime <= 0f){
                // only transition to these if cayote time is 0 or less
                ParentStateMachine.TransitionStates(EnumBus.PLAYER_STATES.FALLING);
            }

            else if (jumpCayoteTime > 0f){
                jumpCayoteTime -= Time.deltaTime;
            }
        }

        if (InputManager.movement.x == 0 && playerRef.isGrounded){
            ParentStateMachine.TransitionStates(EnumBus.PLAYER_STATES.IDLE);
        }

        if (InputManager.jumpWasPressed){
            ParentStateMachine.TransitionStates(EnumBus.PLAYER_STATES.JUMPING);
        }

        if (InputManager.dashWasPressed){
            ParentStateMachine.TransitionStates(EnumBus.PLAYER_STATES.DASHING);
        }



    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        
        playerRef.velocity.y = 0f;


        // if turning other direction
        if (
            (InputManager.movement.x < 0 && playerRef.velocity.x > 0) ||
            (InputManager.movement.x > 0 && playerRef.velocity.x < 0)
        ){
            playerRef.velocity.x = InputManager.movement.x * Mathf.Pow(moveStatsRef.groundDecceleration, moveStatsRef.groundDecceleration) * Time.fixedDeltaTime;
        }
        // else normal accel
        else {
            playerRef.velocity.x += (InputManager.movement.x * moveStatsRef.groundAcceleration) * Time.fixedDeltaTime;
        }

        playerRef.velocity.x = Mathf.Clamp(playerRef.velocity.x, -moveStatsRef.maxRunSpeed, moveStatsRef.maxRunSpeed);
    }


    public override void OnHurt()
    {
        base.OnHurt();
    }

    public override void OnExit(){
        base.OnExit();
        jumpCayoteTime = 0f;
        cayoteBufferActive = false;

        playerRef.animator.SetBool("isRunning", false);
    }

}
