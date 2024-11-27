using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunningState : BasePlayerState<PlayerStateManager.PLAYER_STATES>
{
    protected float jumpCayoteTime = 0f;
    protected bool cayoteBufferActive = false;
    public PlayerRunningState(PlayerStateContext context, PlayerStateManager.PLAYER_STATES stateKey,  PlayerStateManager _psm) : base(context, stateKey, _psm){
        Context = context;
        psm = _psm;
    }

    public override void EnterState() {
        Context.Player.numberOfJumpsUsed = 0;
        Context.Player.numberOfDashesUsed = 0;
        jumpCayoteTime = Context.PlayerMovementStats.jumpCayoteTime;


        Context.Player.animator.SetBool("isRunning", true);
    }
    public override void ExitState(){
        jumpCayoteTime = 0f;
        cayoteBufferActive = false;

        Context.Player.animator.SetBool("isRunning", false);

        Context.SetPrevState(StateKey);

    }

    public override void UpdateState() {
        // for jump cayote time
        if (!Context.Player.isGrounded && jumpCayoteTime > 0f){
            cayoteBufferActive = true;
        }

        // reset cayote properties if on ground again
        if (Context.Player.isGrounded){
            cayoteBufferActive = false;
            jumpCayoteTime = Context.PlayerMovementStats.jumpCayoteTime;
        }        

        // for state transitions
        if (cayoteBufferActive){
            if (jumpCayoteTime > 0f){
                jumpCayoteTime -= Time.deltaTime;
            }
        }

       

    }

    public override void FixedUpdateState() {
        
        Context.Player.velocity.y = 0f;


        // if turning other direction
        if (
            (InputManager.movement.x < 0 && Context.Player.velocity.x > 0) ||
            (InputManager.movement.x > 0 && Context.Player.velocity.x < 0)
        ){
            Context.Player.velocity.x = InputManager.movement.x * Mathf.Pow(Context.PlayerMovementStats.groundDecceleration, Context.PlayerMovementStats.groundDecceleration) * Time.fixedDeltaTime;
        }
        // else normal accel
        else {
            Context.Player.velocity.x += (InputManager.movement.x * Context.PlayerMovementStats.groundAcceleration) * Time.fixedDeltaTime;
        }

        Context.Player.velocity.x = Mathf.Clamp(Context.Player.velocity.x, -Context.PlayerMovementStats.maxRunSpeed, Context.PlayerMovementStats.maxRunSpeed);
    }


    
    public override PlayerStateManager.PLAYER_STATES GetNextState(){

         if (jumpCayoteTime <= 0f){
                // only transition to these if cayote time is 0 or less
                return PlayerStateManager.PLAYER_STATES.FALLING;
        }
        if (InputManager.movement.x == 0 && Context.Player.isGrounded){
            return PlayerStateManager.PLAYER_STATES.IDLE;
        }

        if (InputManager.jumpWasPressed){
            return PlayerStateManager.PLAYER_STATES.JUMPING;
        }

        if (InputManager.dashWasPressed){
            return PlayerStateManager.PLAYER_STATES.DASHING;
        }

        if (InputManager.climbWasPressed && Context.Player.isClimbable) {
            return PlayerStateManager.PLAYER_STATES.CLIMBING;
        }
        return StateKey;
    }
    public override void OnTriggerEnter(Collider collider){}
    public override void OnTriggerStay(Collider collider){}
    public override void OnTriggerExit(Collider collider){}




}
