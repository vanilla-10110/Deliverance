using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : BasePlayerState<PlayerStateManager.PLAYER_STATES>
{
    // this is for when the player trys to jump right before they hit the ground
    protected float jumpBufferTimeLeft = 0f;
    protected bool isSlowFalling = false;
    protected float timeSinceFallModifierStart = 0f;

    public PlayerFallingState(PlayerStateContext context, PlayerStateManager.PLAYER_STATES stateKey, PlayerStateManager _psm) : base(context, stateKey, _psm){
        Context = context;
        psm = _psm;
    }

    public override void EnterState(){
        timeSinceFallModifierStart = 0f;

        jumpBufferTimeLeft = 0f;

        if (Context.PrevState == PlayerStateManager.PLAYER_STATES.JUMPING){
            if (Context.playerIsFastFalling){
                Context.playerIsFastFalling = true;
                Context.Player.velocity.y = -5f;
            }
        }
        if (Context.PrevState == PlayerStateManager.PLAYER_STATES.DASHING){
            Context.playerIsFastFalling = true;
        }

    }   
    public override void ExitState(){

        Context.playerIsFastFalling = false;
        isSlowFalling = false;

        if (jumpBufferTimeLeft != 0f){
            jumpBufferTimeLeft = 0f;
        }

        Context.SetPrevState(StateKey);
    }

    public override void UpdateState(){

        // fast fall related
        if (Context.playerIsFastFalling){
            timeSinceFallModifierStart += Time.deltaTime;
        }
        
        if (Context.playerIsFastFalling && timeSinceFallModifierStart > Context.PlayerMovementStats.timeForUpwardsCancel){
            Context.playerIsFastFalling = false;
        }

        // slow falling related
        if (isSlowFalling){
            timeSinceFallModifierStart += Time.deltaTime;
        }

        if (isSlowFalling && timeSinceFallModifierStart > Context.PlayerMovementStats.afterDashFallingDuration){
            isSlowFalling = false;
        }

        // for the jump buffer
        if (jumpBufferTimeLeft > 0f){
            jumpBufferTimeLeft -= Time.deltaTime;
        }

        if (InputManager.jumpWasPressed && Context.Player.numberOfJumpsUsed >= Context.PlayerMovementStats.numberOfJumpsAllowed ){
            jumpBufferTimeLeft = Context.PlayerMovementStats.jumpBufferTime;
        }
    }

    public override void FixedUpdateState()
    {
        // for when player cuts the jump short
        if (Context.playerIsFastFalling){
            Context.Player.velocity.y = Mathf.Lerp(Context.Player.velocity.y, 0f, timeSinceFallModifierStart / Context.PlayerMovementStats.timeForUpwardsCancel);
        }
        
        // mainly for when faslling after dashing - to give more of a floaty feel at first
        else if (isSlowFalling){
            Context.Player.velocity.y += Context.PlayerMovementStats.gravity * Context.PlayerMovementStats.afterDashFallingSpeedMultiplier * Time.fixedDeltaTime;
        }

        // this is the normal falling accel
        else if (!Context.playerIsFastFalling && !isSlowFalling){
            Context.Player.velocity.y += Context.PlayerMovementStats.gravity * Time.fixedDeltaTime;
        }
        Context.Player.velocity.y = Math.Clamp(Context.Player.velocity.y, -Context.PlayerMovementStats.fallingMaxSpeed, 50f); // 50f just to avoid having 0f, but we shouldnt be falling up in the falling state

        // for horizontal movement
        // if turning other direction
        if (
            (InputManager.movement.x < 0 && Context.Player.velocity.x > 0) ||
            (InputManager.movement.x > 0 && Context.Player.velocity.x < 0)
        ){
            Context.Player.velocity.x = InputManager.movement.x * Mathf.Pow(Context.PlayerMovementStats.airAcceleration, Context.PlayerMovementStats.airDecceleration) * Time.fixedDeltaTime;
        }
        // else normal accel
        else if (InputManager.movement.x != 0) {
            Context.Player.velocity.x += (InputManager.movement.x * Context.PlayerMovementStats.airAcceleration) * Time.fixedDeltaTime;
        }
        else {
            Context.Player.velocity.x = Mathf.Lerp(Context.Player.velocity.x, 0f, Context.PlayerMovementStats.airDecceleration *  Time.fixedDeltaTime);
        }

        Context.Player.velocity.x = Mathf.Clamp(Context.Player.velocity.x, -Context.PlayerMovementStats.maxAirHorizontalSpeed, Context.PlayerMovementStats.maxAirHorizontalSpeed);
    }

    public override PlayerStateManager.PLAYER_STATES GetNextState(){

        if (InputManager.jumpWasPressed && Context.Player.numberOfJumpsUsed < Context.PlayerMovementStats.numberOfJumpsAllowed){
            Context.Player.animator.SetTrigger("doubleJumpTrigger");
            return PlayerStateManager.PLAYER_STATES.JUMPING;
        }
        
        // for state transitions
        if (InputManager.dashWasPressed && Context.Player.numberOfDashesUsed <= Context.PlayerMovementStats.numberOfDashesAllowed)
        {
            return PlayerStateManager.PLAYER_STATES.DASHING;
        }

        if (InputManager.climbWasPressed && Context.Player.isClimbable)
        {
            return PlayerStateManager.PLAYER_STATES.CLIMBING;
        }

        if (Context.Player.isGrounded && jumpBufferTimeLeft > 0f){
            Context.Player.numberOfJumpsUsed = 0;
            return PlayerStateManager.PLAYER_STATES.JUMPING;
        }

        else if (Context.Player.isGrounded){
            return PlayerStateManager.PLAYER_STATES.IDLE;
        }

        return StateKey;
    }
    public override void OnTriggerEnter(Collider collider){}
    public override void OnTriggerStay(Collider collider){}
    public override void OnTriggerExit(Collider collider){}

}
