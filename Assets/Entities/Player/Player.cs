using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    [SerializeField] LayerMask groundLayerMask;
    SpriteRenderer playerSprite;
    Animator animController;
    Rigidbody2D playerBody;
    CapsuleCollider2D bodyCollider;
    public int maxSpeed;
    public float horizontalAccel;
    public int jumpVelocity;
    public float gravityAccel;

    public bool isJumping = false;

    public float jumpDuration = 0.5f;
    public float jumpTime = 0.0f;
    public bool isRunning = false;
    public bool isOnGround = true;

    void Start()
    {
        playerSprite = GetComponent<SpriteRenderer>();
        animController = GetComponent<Animator>();
        playerBody = GetComponent<Rigidbody2D>();
        bodyCollider = GetComponent<CapsuleCollider2D>();
    }
    void Update()
    {
        isOnGround = IsPlayerOnGround();
        isRunning = IsPlayerRunning();

        ControlPlayerJumpTimer();
        ChangeAnimations();

    }

    void FixedUpdate ()
    {
        // playerBody.velocity = GetPlayerVerticalVelocity(playerBody.velocity);
        playerBody.velocity = GetPlayerHorizontalVelocity(playerBody.velocity);
        
        ApplyPlayerJump();
        
        if (!isJumping){
            playerBody.velocity = ApplyGravity(playerBody.velocity);
        }
    }

    void ControlPlayerJumpTimer(){
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround){
            jumpTime = jumpDuration;
            isJumping = true;
        }
        if (jumpTime <= 0.0f || Input.GetKeyUp(KeyCode.Space)){
            isJumping = false;
            jumpTime = 0.0f;
        }

    }

    void ApplyPlayerJump(){
        if (isJumping){
            jumpTime -= Time.fixedDeltaTime;
            playerBody.velocity = GetPlayerJumpVelocity(playerBody.velocity);
        }
    }

    Vector2 GetPlayerHorizontalVelocity(Vector2 velocity)
    {
        float HAxis = Input.GetAxis("Horizontal");

        Vector2 newVelocity = Vector2.zero;

        if (isRunning){
            newVelocity.x = HAxis * horizontalAccel;
        }        

        velocity = Vector2.Lerp(velocity, newVelocity * Time.fixedDeltaTime, 1);// * Time.fixedDeltaTime);      

        velocity.x = Mathf.Clamp(velocity.x, -maxSpeed, maxSpeed);

        return velocity;
    }

    Vector2 GetPlayerJumpVelocity(Vector2 velocity){
        float newVelocity = 0.0f;

        newVelocity = velocity.y > 0.0f ? velocity.y + jumpVelocity : jumpVelocity;
        // newVelocity += jumpVelocity * Time.fixedDeltaTime;

        velocity.y += ( velocity.y + jumpVelocity) * Time.fixedDeltaTime;

        return velocity;
    }

    Vector2 ApplyGravity(Vector2 velocity){
        // float newVelocity = velocity.y - gravityAccel;

        velocity.y = Mathf.Lerp(velocity.y, velocity.y - gravityAccel  * Time.fixedDeltaTime, 1);
    
        return velocity;
    }

    bool IsPlayerOnGround(){
        return Physics2D.BoxCast(bodyCollider.bounds.center,  bodyCollider.size, 0f, Vector2.down, 0.5f, groundLayerMask);
    }

    bool IsPlayerRunning(){
        return Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);
    }

    void ChangeAnimations(){
        animController.SetBool("isRunning", IsPlayerRunning());
        animController.SetBool("isOnGround", IsPlayerOnGround());
        animController.SetBool("isJumping", isJumping);

        if (Input.GetKeyDown(KeyCode.A)){
            playerSprite.flipX = true;
        }
        if (Input.GetKeyDown(KeyCode.D)){
            playerSprite.flipX = false;
        }
    }

    
}
