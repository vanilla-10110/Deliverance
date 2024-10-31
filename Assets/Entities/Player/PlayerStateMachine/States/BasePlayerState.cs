using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePlayerState : BaseState
{
    public EnumBus.PLAYER_STATES stateEnum;
    // protected PSM psm;

    public static new PSM ParentStateMachine {get; private set;}
    public Player playerRef;

    public void InitialSetup(Player player){
        playerRef = player;
    }

    public void OnAwake(PSM psm){
        ParentStateMachine = psm;
    }

     public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("entering new state: " + name);
    }

     public override void OnExit()
    {
        base.OnExit();
        Debug.Log("exiting state: " + name);
    }

}
