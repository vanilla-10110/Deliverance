using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashingState : BasePlayerState
{

    [SerializeField] private float dashDuration = 1f;
    private float timeSinceDashStart = 0f;


    private void Awake(){
        stateEnum = EnumBus.PLAYER_STATES.DASHING;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        timeSinceDashStart = 0f;
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

        

    }

    public override void OnHurt()
    {
        base.OnHurt();
    }

    public override void OnExit(){
        base.OnExit();

        timeSinceDashStart = 0f;
    }

}
