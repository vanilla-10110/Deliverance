using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Player Movement")]

public class PlayerMovementStats : ScriptableObject
{

    [Header("Walk")]
    [Range(1f, 100f)] public float maxWalkSpeed = 12.5f;
    [Range(0.251f, 50f)] public float groundAcceleration = 5f;
    [Range(0.251f, 50f)] public float groundDecceleration = 20f;
    [Range(0.251f, 50f)] public float airAcceleration = 5f;
    [Range(0.251f, 50f)] public float airDecceleration = 5f;

    [Header("Dash")]
    [Range(1f, 100f)] public float maxDashSpeed = 20f;

    [Header("Grounded/Collision Checks")]
    public LayerMask groundLayer;
    public float groundDetectionRayLength = 0.02f;
    public float headDetectionRayLength = 0.02f;
    [Range(0f,1f)] public float headWidth = 0.75f;


    [Header("Jump")]
    public float jumpHeight = 6.5f;
    [Range(1f, 1.1f)] public float jumpcHeightCompensationFactor = 1.05f; 
    public float timeTillJumpApex = 0.35f;
    [Range(0.01f, 5f)] public float gravityOnReleaseMultiplier = 2f;
    public float maxFallSpeed = 26f;
    [Range(1,5)] public int numberOfJumpsAllowed = 2;

    [Header("Jump Cut")]
    [Range(0.02f, 0.3f)] public float timeForUpwardsCancel = 0.027f;

    [Header("Jump Apex")]
    [Range(0.5f, 1f)] public float apexThreshold = 0.97f;
    [Range(0.01f, 1f)] public float apexHangTime = 0.075f;

    [Header("Jump Buffer")]
    [Range(0f, 1f)] public float jumpBufferTime = 0.125f;

    [Header("Jump Cayote Time")]
    [Range(0f,1f)] public float jumpCayoteTime = 0.1f;




    [Header("Debug")]
    public bool debugShowIsGroundedBox;
    public bool debugShowHeadBumpBox;

    [Header("Jump Visualisation Tool")]
    public bool showWalkJumpArc = false;
    public bool showDashJumpArc = false;
    public bool stopOnCollision = true;
    public bool drawRight = true;
    [Range(5,100)] public int arcResolution = 20;
    [Range(6, 500)] public int visualisationSteps = 90;

    public float gravity {get; private set; }

    public float initialJumpVelocity { get; private set; }
    public  float adjustedJumpHeight {get; private set;}

    private void OnValidate(){
        CalculateValues();
    }

    private void OnEnable(){
        CalculateValues();
    }

    private void CalculateValues(){
        adjustedJumpHeight = jumpHeight * jumpcHeightCompensationFactor;
        gravity = -(2f * adjustedJumpHeight) / Mathf.Pow(timeTillJumpApex, 2f);
        initialJumpVelocity = Mathf.Abs(gravity) * timeTillJumpApex;
    }

}
