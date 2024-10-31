using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Serialization;
using UnityEngine;

public class Player : MonoBehaviour
{
   [Header("References")]
   public PlayerMovementStats moveStats;
   [SerializeField] private Collider2D _feetColl;
   [SerializeField] private Collider2D _bodyColl;

   public Rigidbody2D _rb;

   //movement vars
    public Vector2 _moveVelocity;
    public bool isFacingRight;

    //collision check vars
    private RaycastHit2D _groundHit;
    private RaycastHit2D _headHit;
    public bool isGrounded;
    private bool bumpedHead;

    // jump vars
    public float verticalVelocity;
    private bool _isJumping;
    private bool _isFastFalling;
    private bool _isFalling;
    private float _fastFallTime;
    private float _fastFallReleaseSpeed;
    private int _numberOfJumpsUsed;

    // apex vars
    private float _apexPoint;
    private float _timePastApexThreshold;
    private bool _isPastApexThreshold;

    // buffer vars
    private float _jumpBufferTimer;
    private bool _jumpReleasedDuringBuffer;

    // cayote time vars
    private float _cayoteTimer;

    public PSM psm;


    private void Awake(){
        isFacingRight = true;
        _rb = GetComponent<Rigidbody2D>();


    }

    private void Start(){
        psm.OnAwake(this);
    }

    private void Update(){
        psm.OnUpdate();

        IsGrounded();

        TurnCheck(InputManager.movement);
    }
    
    private void FixedUpdate(){

        psm.OnFixedUpdate();
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

        _groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, moveStats.groundDetectionRayLength, moveStats.groundLayer);

        if (_groundHit.collider != null ){
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
    #endregion

}