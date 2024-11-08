using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("References")]
    public PlayerMovementStats moveStats;
    [SerializeField] private Collider2D _feetColl;
    [SerializeField] private Collider2D _bodyColl;
    
    public Animator animator;
    private Rigidbody2D _rb;
    public TrailRenderer PlayerTrail {get; private set;}

    public Vector2 velocity = Vector2.zero;
    public bool isFacingRight;

    //collision check vars
    private RaycastHit2D _leftGroundHit;
    private RaycastHit2D _rightGroundHit;

    private RaycastHit2D _headHit;
    public bool isGrounded;
    public bool bumpedHead;

    // jump vars
    public int numberOfJumpsUsed = 0;

    public PSM psm;

    // dash vars
    public int numberOfDashesUsed = 0;


    private void Awake(){
        isFacingRight = true;
        _rb = GetComponent<Rigidbody2D>();
        PlayerTrail = GetComponent<TrailRenderer>();
        PlayerTrail.emitting = false;
        animator = GetComponent<Animator>();
    }

    private void Start(){
        psm.OnAwake(this);
    }

    private void Update(){
        psm.OnUpdate();

        IsGrounded();
        IsHeadBumped(); 

        TurnCheck(InputManager.movement);

        animator.SetBool("isOnGround", isGrounded);
    }
    
    private void FixedUpdate(){

        psm.OnFixedUpdate();

        _rb.velocity = velocity;
    }

    #region horizontal movement
    private void TurnCheck(Vector2 moveInput){
        if (isFacingRight && moveInput.x < 0){
            Turn(false);
        }
        else if (!isFacingRight && moveInput.x > 0) {
            Turn(true);
        }

    }

    private void Turn (bool turnRight){
        if (turnRight){
            isFacingRight = true;
            transform.Rotate(0f, 180f, 0f);
        }
        else {
            isFacingRight = false;
            transform.Rotate(0f, -180f, 0f);
        }
    }

    #endregion

    #region CollisonChecks
    private void IsGrounded(){
        Vector2 boxCastOrigin = new Vector2(_feetColl.bounds.center.x, _feetColl.bounds.min.y);
        Vector2 boxCastSize = new Vector2(_feetColl.bounds.size.x, moveStats.groundDetectionRayLength);

        // _groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, moveStats.groundDetectionRayLength, moveStats.groundLayer);
        _leftGroundHit = Physics2D.Raycast(new(boxCastOrigin.x - _feetColl.bounds.size.x / 2, boxCastOrigin.y),Vector2.down, moveStats.groundDetectionRayLength, moveStats.groundLayer);
        _rightGroundHit = Physics2D.Raycast(new(boxCastOrigin.x + _feetColl.bounds.size.x / 2, boxCastOrigin.y),Vector2.down, moveStats.groundDetectionRayLength, moveStats.groundLayer);

        if (_leftGroundHit.collider != null || _rightGroundHit.collider != null){
            isGrounded = true;
        }
        else{ isGrounded = false; }
    
        if (moveStats.debugShowIsGroundedBox){
            Color rayColor;
            if (isGrounded){
                rayColor = Color.green;
            }

            else { rayColor = Color.red; }

            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * moveStats.groundDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x + boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * moveStats.groundDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y - moveStats.groundDetectionRayLength), Vector2.right * boxCastSize.x, rayColor);

        }
    }

    private void IsHeadBumped(){
        Vector2 boxCastOrigin = new Vector2(_bodyColl.bounds.center.x, _bodyColl.bounds.max.y);
        Vector2 boxCastSize = new Vector2(moveStats.headWidth, moveStats.headDetectionRayLength);

        _headHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, moveStats.headDetectionRayLength, moveStats.groundLayer);

        if (_headHit.collider != null){
            bumpedHead = true;
        }
        else { bumpedHead = false; }

        if (moveStats.debugShowHeadBumpBox){
            Color rayColor;
            if (bumpedHead){
                rayColor = Color.green;
            }

            else { rayColor = Color.red; }

            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y), Vector2.up * moveStats.headDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x + boxCastSize.x / 2, boxCastOrigin.y), Vector2.up * moveStats.headDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y + moveStats.headDetectionRayLength), Vector2.right * boxCastSize.x, rayColor);

        }
    }
    #endregion

}