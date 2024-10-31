using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PSM : BaseStateMachine
{
    [SerializeField] private BasePlayerState[] stateArray;

    [SerializeField] private EnumBus.PLAYER_STATES initialState = EnumBus.PLAYER_STATES.IDLE;
    new protected BasePlayerState CurrentState {get; private set;}
    public Player playerRef;

    public void OnAwake(Player newPlayerRef){

        CurrentState = GetState(initialState);

        playerRef = newPlayerRef;

        foreach (BasePlayerState childState in stateArray){
            childState.InitialSetup(playerRef);
            childState.OnAwake(this);
            
        }
    }

    public override void OnUpdate(){
            if (!CurrentState){
                return;
            }

            CurrentState.OnUpdate();

    }

    public override void OnFixedUpdate(){
        if (!CurrentState){
            return;
        }

        CurrentState.OnFixedUpdate();
    }

    public void TransitionStates(EnumBus.PLAYER_STATES newStateEnum)
    {
        CurrentState.OnExit();

        CurrentState = GetState(newStateEnum);

        CurrentState.OnEnter();
    }

    private BasePlayerState GetState(EnumBus.PLAYER_STATES stateEnum){
        return System.Array.Find<BasePlayerState>(stateArray, (state) => {return state.stateEnum == stateEnum;});
    }

}