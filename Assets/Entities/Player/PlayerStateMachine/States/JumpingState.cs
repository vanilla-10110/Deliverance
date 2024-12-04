using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpingState : BasePlayerState<PlayerStateManager.PLAYER_STATES>
{

    // apex vars
    protected float apexPoint;
    protected float timePastApexThreshold;
    protected bool isPastApexThreshold;

    public PlayerJumpingState(PlayerStateContext context, PlayerStateManager.PLAYER_STATES stateKey, PlayerStateManager _psm) : base(context, stateKey, _psm){
        Context = context;
        psm = _psm;
    }

    public override void EnterState(){
        Context.playerIsFastFalling = false;

        Context.Player.velocity.y = Context.PlayerMovementStats.initialJumpVelocity;

        Context.Player.animator.SetBool("isJumping", true);

        SignalBus.AbilityUsedEvent.Invoke(EnumBus.PLAYER_ABILITIES.JUMP.ToString());

        SoundManager.Instance.PlaySoundFX(Context.Player.jumpSoundFX);
    }   
    public override void ExitState(){
        Context.Player.numberOfJumpsUsed += 1;

        isPastApexThreshold = false;

        Context.Player.animator.SetBool("isJumping", false);

        Context.SetPrevState(StateKey);


    }
    public override void UpdateState(){}

    public override void FixedUpdateState(){
        if (
            (InputManager.movement.x < 0 && Context.Player.velocity.x > 0) ||
            (InputManager.movement.x > 0 && Context.Player.velocity.x < 0)
        ){
            Context.Player.velocity.x = InputManager.movement.x * Mathf.Pow(Context.PlayerMovementStats.airAcceleration, Context.PlayerMovementStats.airDecceleration) * Time.fixedDeltaTime;
        }
        // else normal accel
        else {
            Context.Player.velocity.x += (InputManager.movement.x * Context.PlayerMovementStats.airAcceleration) * Time.fixedDeltaTime;
        }

        Context.Player.velocity.x = Mathf.Clamp(Context.Player.velocity.x, -Context.PlayerMovementStats.maxAirHorizontalSpeed, Context.PlayerMovementStats.maxAirHorizontalSpeed);


        // APEX CONTROLS
        apexPoint = Mathf.InverseLerp(Context.PlayerMovementStats.initialJumpVelocity, 0f, Context.Player.velocity.y);
        if (apexPoint > Context.PlayerMovementStats.apexThreshold){
            if (!isPastApexThreshold){
                isPastApexThreshold = true;

                timePastApexThreshold = 0f;
            }

            if (isPastApexThreshold){
                timePastApexThreshold += Time.fixedDeltaTime;

                if (timePastApexThreshold < Context.PlayerMovementStats.apexHangTime){
                    Context.Player.velocity.y = 0f;
                }
            }
        }

        // GRAVITY ON ASCENDING BUT BEFORE APEX THRESHOLD
        else if (apexPoint <= Context.PlayerMovementStats.apexThreshold){
            Context.Player.velocity.y += Context.PlayerMovementStats.gravity * Time.fixedDeltaTime;
        }
    }
    public override PlayerStateManager.PLAYER_STATES GetNextState(){

        if (Context.Player.bumpedHead){
            Context.playerIsFastFalling = true;
            return PlayerStateManager.PLAYER_STATES.FALLING;
        }

        if (InputManager.dashWasPressed && (Context.Player.numberOfDashesUsed >= Context.PlayerMovementStats.numberOfDashesAllowed)){
            return PlayerStateManager.PLAYER_STATES.DASHING;
        }
        if (InputManager.climbWasPressed && Context.Player.isClimbable)
        {
            return PlayerStateManager.PLAYER_STATES.CLIMBING;
        }

        if (Context.Player.numberOfJumpsUsed >= Context.PlayerMovementStats.numberOfJumpsAllowed){
            return PlayerStateManager.PLAYER_STATES.FALLING;
        }

        if (InputManager.jumpWasReleased){
            Context.playerIsFastFalling = true;
            return PlayerStateManager.PLAYER_STATES.FALLING;
        }

        if (isPastApexThreshold && timePastApexThreshold > Context.PlayerMovementStats.apexHangTime){
            return PlayerStateManager.PLAYER_STATES.FALLING;
        }

        return StateKey;
    }
    public override void OnTriggerEnter(Collider collider){}
    public override void OnTriggerStay(Collider collider){}
    public override void OnTriggerExit(Collider collider){}

}
