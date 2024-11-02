using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingState : BasePlayerState
{

    // apex vars
    protected float apexPoint;
    protected float timePastApexThreshold;
    protected bool isPastApexThreshold;

    public bool isFastFalling = false;

    private void Awake(){
        stateEnum = EnumBus.PLAYER_STATES.JUMPING;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        isFastFalling = false;

        playerRef.velocity.y = moveStatsRef.initialJumpVelocity;
    }

    public override void OnUpdate(){
        base.OnUpdate();

        if (playerRef.bumpedHead){
            isFastFalling = true;
            ParentStateMachine.TransitionStates(EnumBus.PLAYER_STATES.FALLING);
        }

        if (InputManager.dashWasPressed){
            ParentStateMachine.TransitionStates(EnumBus.PLAYER_STATES.DASHING);
        }

        if (playerRef.numberOfJumpsUsed >= moveStatsRef.numberOfJumpsAllowed){
            ParentStateMachine.TransitionStates(EnumBus.PLAYER_STATES.FALLING);
        }

        if (InputManager.jumpWasReleased){
            isFastFalling = true;
            ParentStateMachine.TransitionStates(EnumBus.PLAYER_STATES.FALLING);
        }

        if (isPastApexThreshold && timePastApexThreshold > moveStatsRef.apexHangTime){
            ParentStateMachine.TransitionStates(EnumBus.PLAYER_STATES.FALLING);
        }

    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        
        // Vector2 targetVelocity = new Vector2(InputManager.movement.x * moveStatsRef.airAcceleration, playerRef.velocity.y);


        // horizontal movement related
        if (
            (InputManager.movement.x < 0 && playerRef.velocity.x > 0) ||
            (InputManager.movement.x > 0 && playerRef.velocity.x < 0)
        ){
            playerRef.velocity.x = InputManager.movement.x * Mathf.Pow(moveStatsRef.airAcceleration, moveStatsRef.airDecceleration) * Time.fixedDeltaTime;
        }
        // else normal accel
        else {
            playerRef.velocity.x += (InputManager.movement.x * moveStatsRef.airAcceleration) * Time.fixedDeltaTime;
        }

        playerRef.velocity.x = Mathf.Clamp(playerRef.velocity.x, -moveStatsRef.maxAirHorizontalSpeed, moveStatsRef.maxAirHorizontalSpeed);


        // APEX CONTROLS
        apexPoint = Mathf.InverseLerp(moveStatsRef.initialJumpVelocity, 0f, playerRef.velocity.y);
        if (apexPoint > moveStatsRef.apexThreshold){
            if (!isPastApexThreshold){
                isPastApexThreshold = true;

                timePastApexThreshold = 0f;
            }

            if (isPastApexThreshold){
                timePastApexThreshold += Time.fixedDeltaTime;

                if (timePastApexThreshold < moveStatsRef.apexHangTime){
                    playerRef.velocity.y = 0f;
                }
            }
        }

        // GRAVITY ON ASCENDING BUT BEFORE APEX THRESHOLD
        else if (apexPoint <= moveStatsRef.apexThreshold){
            playerRef.velocity.y += moveStatsRef.gravity * Time.fixedDeltaTime;
        }

        // playerRef.velocity = Vector2.Lerp(playerRef.velocity, targetVelocity, Time.fixedDeltaTime);
    }


    public override void OnHurt()
    {
        base.OnHurt();
    }

    public override void OnExit(){
        base.OnExit();
        playerRef.numberOfJumpsUsed += 1;

        isPastApexThreshold = false;
    }

}
