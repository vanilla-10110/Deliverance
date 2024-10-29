using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;


// using System.Numerics;
using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
   [Header("References")]
   public PlayerMovementStats moveStats;
   [SerializeField] private Collider2D _feetColl;
   [SerializeField] private Collider2D _bodyColl;

   private Rigidbody2D _rb;

   //movement vars
    private Vector2 _moveVelocity;
    private bool _isFacingRight;

    //collision check vars
    private RaycastHit2D _groundHit;
    private RaycastHit2D _headHit;
    private bool _isGrounded;
    private bool _bumpedHead;

    // jump vars
    public float verticalVelocity { get; private set; }
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

    // i followed a tutorial for this movement script, we can defs make some modifications but otherwise it is quite complete
    // https://www.youtube.com/watch?v=zHSWG05byEc&t=412s

    private void Update(){
        CountTimers();
        JumpChecks();
    }


    private void FixedUpdate(){
        CollisionChecks();
        Jump();

        if (_isGrounded){
            Move(moveStats.groundAcceleration, moveStats.groundDecceleration, InputManager.movement);
        }
        else {
            Move(moveStats.airAcceleration, moveStats.airDecceleration, InputManager.movement);
        }
    }


    private void Awake(){
        _isFacingRight = true;
        _rb = GetComponent<Rigidbody2D>();
    }



    #region Move
    
    private void Move(float accelaration, float decceleration, Vector2 moveInput){
        Vector2 targetVelocity = Vector2.zero;
        
        if (moveInput != Vector2.zero){
            // check if needs to turn around
            if (InputManager.dashIsheld) {
                targetVelocity = new Vector2(moveInput.x, 0f) * moveStats.maxDashSpeed; 
            }
        
            else { targetVelocity = new Vector2(moveInput.x, 0f) * moveStats.maxWalkSpeed; }
        }
        // lerp from current movementVel to target
        _moveVelocity = Vector2.Lerp(_moveVelocity, targetVelocity, accelaration * Time.fixedDeltaTime);
        // then apply to rigidBody
        _rb.velocity = new Vector2(_moveVelocity.x, _rb.velocity.y);
    }

    private void TurnCheck(Vector2 moveInput){
        if (_isFacingRight && moveInput.x < 0){
            Turn(false);
        }
        else {
            Turn(true);
        }

    }

    private void Turn (bool turnRight){
        if (turnRight){
            _isFacingRight = true;
            transform.Rotate(0f, 180f, 0f);
        }
        else {
            _isFacingRight = false;
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
            _isGrounded = true;
        }
        else{ _isGrounded = false; }
    
        #region DebugVisualisation
        if (moveStats.debugShowIsGroundedBox){
            Color rayColor;
            if (_isGrounded){
                rayColor = Color.green;
            }

            else { rayColor = Color.red; }

            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * moveStats.groundDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x + boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * moveStats.groundDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y - moveStats.groundDetectionRayLength), Vector2.right * boxCastSize.x, rayColor);

        }
            
        #endregion
    }

    private void CollisionChecks(){

        IsGrounded();
    }
        
    #endregion

    #region Jump

    private void JumpChecks(){
        // WHEN WE PRESS THE JUMP BUTTON
        if (InputManager.jumpWasPressed){
            _jumpBufferTimer = moveStats.jumpBufferTime;
        }


        // WHEN WE RELEASE THE JUMP BUTTON
        if (_isPastApexThreshold){
            _isPastApexThreshold = false;
            _isFastFalling = true;
            _fastFallTime = moveStats.timeForUpwardsCancel;

            verticalVelocity = 0f;
        }
        else {
            _isFastFalling = true;
            _fastFallReleaseSpeed = verticalVelocity;
        }


        // INITIATE JUMP WITH JUMP BUFFERING AND CAYOTE TIME
        if ( _jumpBufferTimer > 0f && !_isJumping && (_isGrounded || _cayoteTimer > 0f)){
            InitiateJump(1);

            if (_jumpReleasedDuringBuffer){
                _isFastFalling = true;
                _fastFallReleaseSpeed = verticalVelocity;
            }
        }
        // DOUBLE JUMP
        else if (_jumpBufferTimer > 0f && _isJumping && _numberOfJumpsUsed < moveStats.numberOfJumpsAllowed){
            _isFastFalling = false;
            InitiateJump(1);
        }

        // AIR TIME AFTER CAYOTE TIME LAPSED
        else if (_jumpBufferTimer > 0f && _isFalling && _numberOfJumpsUsed < moveStats.numberOfJumpsAllowed - 1){
            InitiateJump(2);
            _isFastFalling = false;
        }

        //LANDED
        if ((_isJumping || _isFalling) && _isGrounded && verticalVelocity <= 0f){
            _isJumping = false;
            _isFalling = false;
            _isFastFalling = false;
            _fastFallTime = 0f;
            _isPastApexThreshold = false;
            _numberOfJumpsUsed = 0;
            
            verticalVelocity = Physics2D.gravity.y;
        }

    }

    private void InitiateJump(int numberOfJumpsUsed){
        if (!_isJumping){
            _isJumping = true;
        }

        _jumpBufferTimer = 0f;
        _numberOfJumpsUsed += numberOfJumpsUsed;
        verticalVelocity = moveStats.initialJumpVelocity;

    }

    private void Jump(){
        // APPLY GRAVITY WHILE JUMPING
        if (_isJumping){

            // CHECK FOR HEAD BUMP
            if (_bumpedHead){
                _isFastFalling = true;
            }

            // GRAVITY ON ASCENDING
            if (verticalVelocity >= 0f){
                
                // APEX CONTROLS
                _apexPoint = Mathf.InverseLerp(moveStats.initialJumpVelocity, 0f, verticalVelocity);

                if (_apexPoint > moveStats.apexThreshold){
                    if (!_isPastApexThreshold){
                        _isPastApexThreshold = true;

                        _timePastApexThreshold = 0f;
                    }

                    if (_isPastApexThreshold){
                        _timePastApexThreshold += Time.fixedDeltaTime;

                        if (_timePastApexThreshold < moveStats.apexHangTime){
                            verticalVelocity = 0f;
                        }
                        else { verticalVelocity = -0.01f; }
                    }
                }

                // GRAVITY ON ASCENDING BUT NOT PAST APEX THRESHOLD
                else {
                     verticalVelocity += moveStats.gravity * Time.fixedDeltaTime;
                     if (_isPastApexThreshold){
                        _isPastApexThreshold = false;
                     }
                }
            }

            // GRAVITY ON DESCENDING
            else if (!_isFastFalling) {
                verticalVelocity += moveStats.gravity * moveStats.gravityOnReleaseMultiplier * Time.fixedDeltaTime;

            }

            else if (verticalVelocity < 0f){
                if (!_isFalling){
                    _isFalling = true;
                }
            }
            
        }
        // JUMP CUT
        if (_isFastFalling){
            if(_fastFallTime >= moveStats.timeForUpwardsCancel){
                verticalVelocity += moveStats.gravity * moveStats.gravityOnReleaseMultiplier * Time.fixedDeltaTime;
            }
            else if (_fastFallTime < moveStats.timeForUpwardsCancel){
                verticalVelocity = Mathf.Lerp(_fastFallReleaseSpeed, 0f, (_fastFallTime / moveStats.timeForUpwardsCancel));
            }

            _fastFallTime += Time.fixedDeltaTime;
        }

        // NORMAL GRAVITY WHILE FALLING
        if (!_isGrounded && !_isJumping){
            if (!_isFalling){
                _isFalling = true;
            }

            
            verticalVelocity += moveStats.gravity * Time.fixedDeltaTime;
        }

        // CLAMP FALL SPEED
        verticalVelocity = Mathf.Clamp(verticalVelocity, -moveStats.maxFallSpeed, 50f);

        _rb.velocity = new Vector2(_rb.velocity.x, verticalVelocity);
    }
        
    #endregion

    #region Timers
        
    private void CountTimers(){
        _jumpBufferTimer -= Time.deltaTime;
        if (!_isGrounded){
            _cayoteTimer -= Time.deltaTime;
        }
        else { _cayoteTimer = moveStats.jumpCayoteTime;}

    }

    #endregion

}