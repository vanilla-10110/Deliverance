using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingState : BasePlayerState
{
    // this is for when the player trys to jump right before they hit the ground
    protected float jumpBufferTimeLeft = 0f;

    protected bool isFastFalling = false;
    protected bool isSlowFalling = false;
    protected float timeSinceFallModifierStart = 0f;

    private void Awake(){
        stateEnum = EnumBus.PLAYER_STATES.FALLING;
    }
    public override void OnEnter()
    {
        base.OnEnter();

        timeSinceFallModifierStart = 0f;

        jumpBufferTimeLeft = 0f;

        if (ParentStateMachine.prevState == EnumBus.PLAYER_STATES.JUMPING){
            if ((ParentStateMachine.GetState(ParentStateMachine.prevState) as JumpingState).isFastFalling){
                isFastFalling = true;
                playerRef.velocity.y = -5f;
            }
        }
        if (ParentStateMachine.prevState == EnumBus.PLAYER_STATES.DASHING){
            isFastFalling = true;
        }
        
    }

    public override void OnUpdate(){
        base.OnUpdate();

        // fast fall related
        if (isFastFalling){
            timeSinceFallModifierStart += Time.deltaTime;
        }
        
        if (isFastFalling && timeSinceFallModifierStart > moveStatsRef.timeForUpwardsCancel){
            isFastFalling = false;
        }

        // slow falling related
        if (isSlowFalling){
            timeSinceFallModifierStart += Time.deltaTime;
        }

        if (isSlowFalling && timeSinceFallModifierStart > moveStatsRef.afterDashFallingDuration){
            isSlowFalling = false;
        }

        // for the jump buffer
        if (jumpBufferTimeLeft > 0f){
            jumpBufferTimeLeft -= Time.deltaTime;
        }

        if (InputManager.jumpWasPressed && playerRef.numberOfJumpsUsed >= moveStatsRef.numberOfJumpsAllowed ){
            jumpBufferTimeLeft = moveStatsRef.jumpBufferTime;
        }
        else if (InputManager.jumpWasPressed && playerRef.numberOfJumpsUsed < moveStatsRef.numberOfJumpsAllowed){
            playerRef.animator.SetTrigger("doubleJumpTrigger");
            ParentStateMachine.TransitionStates(EnumBus.PLAYER_STATES.JUMPING);
        }
        
        // for state transitions
        if (InputManager.dashWasPressed && playerRef.numberOfDashesUsed <= moveStatsRef.numberOfDashesAllowed)
        {
            ParentStateMachine.TransitionStates(EnumBus.PLAYER_STATES.DASHING);
        }

        if (InputManager.climbWasPressed && playerRef.isClimbable)
        {
            ParentStateMachine.TransitionStates(EnumBus.PLAYER_STATES.CLIMBING);
        }

        if (playerRef.isGrounded && jumpBufferTimeLeft > 0f){
            playerRef.numberOfJumpsUsed = 0;
            ParentStateMachine.TransitionStates(EnumBus.PLAYER_STATES.JUMPING);
        }

        else if (playerRef.isGrounded){
            ParentStateMachine.TransitionStates(EnumBus.PLAYER_STATES.IDLE);
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        // for vertical movement

        // for when player cuts the jump short
        if (isFastFalling){
            playerRef.velocity.y = Mathf.Lerp(playerRef.velocity.y, 0f, timeSinceFallModifierStart / moveStatsRef.timeForUpwardsCancel);
        }
        
        // mainly for when faslling after dashing - to give more of a floaty feel at first
        else if (isSlowFalling){
            playerRef.velocity.y += moveStatsRef.gravity * moveStatsRef.afterDashFallingSpeedMultiplier * Time.fixedDeltaTime;
        }

        // this is the normal falling accel
        else if (!isFastFalling && !isSlowFalling){
            playerRef.velocity.y += moveStatsRef.gravity * Time.fixedDeltaTime;
        }
        playerRef.velocity.y = Math.Clamp(playerRef.velocity.y, -moveStatsRef.fallingMaxSpeed, 50f); // 50f just to avoid having 0f, but we shouldnt be falling up in the falling state

        // for horizontal movement
        // if turning other direction
        if (
            (InputManager.movement.x < 0 && playerRef.velocity.x > 0) ||
            (InputManager.movement.x > 0 && playerRef.velocity.x < 0)
        ){
            playerRef.velocity.x = InputManager.movement.x * Mathf.Pow(moveStatsRef.airAcceleration, moveStatsRef.airDecceleration) * Time.fixedDeltaTime;
        }
        // else normal accel
        else if (InputManager.movement.x != 0) {
            playerRef.velocity.x += (InputManager.movement.x * moveStatsRef.airAcceleration) * Time.fixedDeltaTime;
        }
        else {
            playerRef.velocity.x = Mathf.Lerp(playerRef.velocity.x, 0f, moveStatsRef.airDecceleration *  Time.fixedDeltaTime);
        }

        playerRef.velocity.x = Mathf.Clamp(playerRef.velocity.x, -moveStatsRef.maxAirHorizontalSpeed, moveStatsRef.maxAirHorizontalSpeed);

    }


    public override void OnHurt()
    {
        base.OnHurt();
    }

    public override void OnExit(){
        base.OnExit();
        isFastFalling = false;
        isSlowFalling = false;

        if (jumpBufferTimeLeft != 0f){
            jumpBufferTimeLeft = 0f;
        }
    }

}
