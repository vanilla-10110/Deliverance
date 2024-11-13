using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DashingState : BasePlayerState
{

    // [Range(0.05f, 1f)][SerializeField] private float dashDuration = 0.2f;
    // [Range(1f, 100f)] [SerializeField] private float dashSpeed = 100f;
    private float timeSinceDashStart = 0f;
    public Vector2 lastDashDirection;
    private Vector2 dashDirection = Vector2.zero;


    private void Awake(){
        stateEnum = EnumBus.PLAYER_STATES.DASHING;
    }
    public override void OnEnter()
    {
        base.OnEnter();

        if (playerRef.numberOfDashesUsed >= moveStatsRef.numberOfDashesAllowed)
        {
            if (playerRef.isGrounded)
            {
                ParentStateMachine.TransitionStates(EnumBus.PLAYER_STATES.IDLE);
            }
            else
            {
                ParentStateMachine.TransitionStates(EnumBus.PLAYER_STATES.FALLING);
            }
        }
        else
        {
        // Player trail renderer
        playerRef.PlayerTrail.emitting = true;
        playerRef.PlayerTrail.time = moveStatsRef.dashDuration * 3;

        timeSinceDashStart = 0f;

        if (InputManager.movement == Vector2.zero){
            dashDirection.x = playerRef.isFacingRight ? 1 : -1;
        }
        else {
            dashDirection = InputManager.movement;
            }
        }
    }

    public override void OnUpdate(){
        base.OnUpdate();

        if (playerRef.numberOfDashesUsed >= moveStatsRef.numberOfDashesAllowed)
        {
            if (playerRef.isGrounded)
            {
                ParentStateMachine.TransitionStates(EnumBus.PLAYER_STATES.IDLE);
            }
            else
            {
                ParentStateMachine.TransitionStates(EnumBus.PLAYER_STATES.FALLING);
            }
        }

        if (timeSinceDashStart >= moveStatsRef.dashDuration && playerRef.isGrounded){
            ParentStateMachine.TransitionStates(EnumBus.PLAYER_STATES.IDLE);
        }

        if (timeSinceDashStart >= moveStatsRef.dashDuration && !playerRef.isGrounded){
            ParentStateMachine.TransitionStates(EnumBus.PLAYER_STATES.FALLING);
        }

        timeSinceDashStart += Time.deltaTime;

    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        playerRef.velocity = dashDirection * moveStatsRef.dashSpeed;

    }

    public override void OnHurt()
    {
        base.OnHurt();
    }

    public override void OnExit(){
        base.OnExit();
        playerRef.numberOfDashesUsed += 1;
        playerRef.PlayerTrail.emitting = false ;

        // reseting the stats so it doesnt carry over next time you dash
        timeSinceDashStart = 0f;
        lastDashDirection = dashDirection;
        dashDirection = Vector2.zero;
    }

}
