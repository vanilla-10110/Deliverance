using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStateMachine : MonoBehaviour
{
    // public BaseState initialState; 
   public virtual BaseState CurrentState {get; private set;}

//    private void Awake(){
//     currentState = initialState;
//    }

    public virtual void OnUpdate(){
        if (!CurrentState){
            return;
        }
        CurrentState.OnUpdate();
    }


   public virtual void OnFixedUpdate(){
        if (!CurrentState){
            return;
        }

        CurrentState.OnFixedUpdate();
   }

   public virtual void TransitionStates(BaseState newState){
    CurrentState.OnExit();

    CurrentState = newState;

    CurrentState.OnEnter();
   }
}
