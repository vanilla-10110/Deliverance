using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerDashingState : BasePlayerState<PlayerStateManager.PLAYER_STATES>
{

    // [Range(0.05f, 1f)][SerializeField] private float dashDuration = 0.2f;
    // [Range(1f, 100f)] [SerializeField] private float dashSpeed = 100f;
    private float timeSinceDashStart = 0f;
    public Vector2 lastDashDirection;
    private Vector2 dashDirection = Vector2.zero;

    public PlayerDashingState(PlayerStateContext context, PlayerStateManager.PLAYER_STATES stateKey, PlayerStateManager _psm) : base(context, stateKey, _psm){
        Context = context;
        psm = _psm;
    }
    
    public override void EnterState(){
        Context.Player.canAttack = false;

        // Player trail renderer
        Context.Player.PlayerTrail.emitting = true;
        Context.Player.PlayerTrail.time = Context.PlayerMovementStats.dashDuration * 3;

        timeSinceDashStart = 0f;

        if (InputManager.movement == Vector2.zero){
            dashDirection.x = Context.Player.isFacingRight ? 1 : -1;
        }
        else {
            dashDirection = InputManager.movement;
            }
        
        SignalBus.AbilityUsedEvent.Invoke(EnumBus.PLAYER_ABILITIES.DASH.ToString());
    }
    public override void ExitState(){
        Context.Player.canAttack = true;

        Context.Player.numberOfDashesUsed += 1;
        Context.Player.PlayerTrail.emitting = false ;

        // reseting the stats so it doesnt carry over next time you dash
        timeSinceDashStart = 0f;
        lastDashDirection = dashDirection;
        dashDirection = Vector2.zero;

        Context.SetPrevState(StateKey);

    }
    public override void UpdateState(){
        // dash timer
        timeSinceDashStart += Time.deltaTime;
    }
    public override PlayerStateManager.PLAYER_STATES GetNextState(){
        if (Context.Player.numberOfDashesUsed >= Context.PlayerMovementStats.numberOfDashesAllowed) {
            if (Context.Player.isGrounded) {
                return PlayerStateManager.PLAYER_STATES.IDLE;
            } else {
                return PlayerStateManager.PLAYER_STATES.FALLING;
            }
        }

        if (timeSinceDashStart >= Context.PlayerMovementStats.dashDuration && Context.Player.isGrounded){
            return PlayerStateManager.PLAYER_STATES.IDLE;
        }

        if (timeSinceDashStart >= Context.PlayerMovementStats.dashDuration && !Context.Player.isGrounded){
            Context.playerIsFastFalling = true;
            return PlayerStateManager.PLAYER_STATES.FALLING;
        }

        return StateKey;
    }
    public override void FixedUpdateState(){
        Context.Player.velocity = dashDirection * Context.PlayerMovementStats.dashSpeed;
    }
    public override void OnTriggerEnter(Collider collider){}
    public override void OnTriggerStay(Collider collider){}
    public override void OnTriggerExit(Collider collider){}

}
