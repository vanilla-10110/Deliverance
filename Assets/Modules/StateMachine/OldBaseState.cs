using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OldBaseState : MonoBehaviour {
    // public static BaseStateMachine ParentStateMachine { get; private set;}


    // public virtual void OnAwake(BaseStateMachine stateMachine){
    //     ParentStateMachine = stateMachine;
    // }


    public virtual void OnEnter(){
        // anything this state should do as it initially gets called
    }


    public virtual void OnUpdate(){
        // should mainly be used for checking the condition for when to change to another state
    }


    public virtual void OnFixedUpdate(){
        // should mainly be used for any physics calcs - like moving a rigidbody
    }


    public virtual void OnHurt(){
        // to change to the hurt state
    }


    public virtual void OnExit(){
        // anything this states should do before the state machine switches to another state
    }
}