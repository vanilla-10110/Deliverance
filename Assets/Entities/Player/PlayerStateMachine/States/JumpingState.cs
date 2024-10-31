using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingState : BasePlayerState
{

    private void Awake(){
        stateEnum = EnumBus.PLAYER_STATES.JUMPING;
    }
    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnUpdate(){
        base.OnUpdate();
        if (InputManager.jumpWasReleased){
            ParentStateMachine.TransitionStates(EnumBus.PLAYER_STATES.FALLING);
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }


    public override void OnHurt()
    {
        base.OnHurt();
    }

    public override void OnExit(){
        base.OnExit();
    }

}
