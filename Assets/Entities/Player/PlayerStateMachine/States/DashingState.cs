using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DashingState : BasePlayerState
{

    [Range(0.05f, 1f)][SerializeField] private float dashDuration = 0.2f;
    [Range(1f, 100f)] [SerializeField] private float dashSpeed = 100f;
    private float timeSinceDashStart = 0f;
    private Vector2 dashDirection = Vector2.zero;


    private void Awake(){
        stateEnum = EnumBus.PLAYER_STATES.DASHING;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        timeSinceDashStart = 0f;

        if (InputManager.movement == Vector2.zero){
            dashDirection.x = playerRef.isFacingRight ? 1 : -1;
        }
        else {
            dashDirection = InputManager.movement;
        }
    }

    public override void OnUpdate(){
        base.OnUpdate();

        if (timeSinceDashStart >= dashDuration){
            ParentStateMachine.TransitionStates(EnumBus.PLAYER_STATES.IDLE);
        } 

        timeSinceDashStart += Time.deltaTime;

        Debug.Log(timeSinceDashStart);
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        playerRef._rb.velocity = dashDirection * dashSpeed;//new Vector2( playerRef.moveStats.maxDashSpeed * dashDirection, 0f);

    }

    public override void OnHurt()
    {
        base.OnHurt();
    }

    public override void OnExit(){
        base.OnExit();

        timeSinceDashStart = 0f;
        dashDirection = Vector2.zero;
    }

}
