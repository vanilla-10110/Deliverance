using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseDoor : BaseLockObject
{
    [SerializeField] private Vector3 targetUnlockedPositionOffset;
    private Vector3 targetLockedPosition;

    private bool isMoving = false;
    public static float t = 0f;
    [SerializeField] [Range(0.05f, 2f)] private float _doorSpeed = 1f;

    private void Start(){
        
        targetLockedPosition = transform.position;

    }

    protected override void Update(){
        base.Update();
        UpdateDoorPosition();
    }

    private void UpdateDoorPosition(){ 

        switch (CurrentState){
            case LOCK_STATE.LOCKED:
                if (!isMoving && transform.position != targetLockedPosition){
                    t = 0f;
                    isMoving = true;
                }

                else if (isMoving && transform.position == targetLockedPosition){
                    isMoving = false;
                }

                if (isMoving){
                    t += Time.deltaTime * _doorSpeed;
                    transform.position = Vector3.Lerp(transform.position, targetLockedPosition, t);
                }
                return;
            case LOCK_STATE.UNLOCKED:
                if (!isMoving && transform.position != targetUnlockedPositionOffset){
                    t = 0f;
                    isMoving = true;
                }

                else if (isMoving && transform.position == targetUnlockedPositionOffset){
                    isMoving = false;
                }
                
                if (isMoving){
                    t += Time.deltaTime * _doorSpeed;
                    transform.position = Vector3.Lerp(transform.position, targetLockedPosition + targetUnlockedPositionOffset, t);
                }
                return;
        }
    }


}
