using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePlayerState : BaseState
{
    public EnumBus.PLAYER_STATES stateEnum;

    public static new PSM ParentStateMachine {get; private set;}
    public Player playerRef;
    protected PlayerMovementStats moveStatsRef;

    public void InitialSetup(Player player){
        playerRef = player;
       
    }

    public void OnAwake(PSM psm){
        ParentStateMachine = psm;
    }

     public override void OnEnter()
    {
        base.OnEnter();
        moveStatsRef = playerRef.moveStats; // re fetches the move stats (in case its not a continuous reference)
        Debug.Log("entering new state: " + name);
    }

     public override void OnExit()
    {
        base.OnExit();
        Debug.Log("exiting state: " + name);
    }

}
