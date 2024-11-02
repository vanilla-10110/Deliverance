using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingState : BasePlayerState
{
    private void Awake(){
        stateEnum = EnumBus.PLAYER_STATES.CLIMBING;
    }
    
    public override void OnEnter()
    {
        base.OnEnter();
        playerRef.numberOfJumpsUsed = 0;

    }

    public override void OnUpdate(){
        base.OnUpdate();

        if (InputManager.dashWasPressed){
            ParentStateMachine.TransitionStates(EnumBus.PLAYER_STATES.DASHING);
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
