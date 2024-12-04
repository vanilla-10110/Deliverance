using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    public static PlayerInput playerInput;

    public static Vector2 movement;
    public static bool jumpWasPressed;
    public static bool jumpIsHeld;
    public static bool jumpWasReleased;
    public static bool dashWasPressed;
    public static bool climbWasPressed;
    public static bool dashIsheld;
    public static bool attackWasPressed;
    public static bool attackIsPressed;
    public static bool attackWasReleased;

    public static bool secondaryAttackWasPressed;
    public static bool secondaryAttackIsPressed;
    public static bool secondaryAttackWasReleased;
    
    [Header("Interact")]
    public static bool interactWasPressed;
    public static bool interactIsPressed;
    public static bool interactWasReleased;

    [Header("UI")]
    public static bool pauseWasPressed;


    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _dashAction;
    private InputAction _climbAction;
    private InputAction _attackAction;
    private InputAction _secondaryAttackAction;
    private InputAction _interactAction;
    private InputAction _pauseAction;

    private void Awake(){
        playerInput = GetComponent<PlayerInput>();

        _moveAction = playerInput.actions["Move"];
        _jumpAction = playerInput.actions["Jump"];
        _dashAction = playerInput.actions["Dash"];
        _climbAction = playerInput.actions["Climb"];
        _attackAction = playerInput.actions["MainAttack"];
        _interactAction = playerInput.actions["Interact"];
        _pauseAction = playerInput.actions["Pause"];
        _secondaryAttackAction = playerInput.actions["SecondaryAttack"];
    }


    private void Update()
    {
        movement = _moveAction.ReadValue<Vector2>();
    
        jumpWasPressed = _jumpAction.WasPressedThisFrame();
        jumpIsHeld = _jumpAction.IsPressed();
        jumpWasReleased = _jumpAction.WasReleasedThisFrame();

        dashWasPressed = _dashAction.WasPressedThisFrame();
        dashIsheld = _dashAction.IsPressed();

        climbWasPressed = _climbAction.WasPressedThisFrame();

        attackWasPressed = _attackAction.WasPressedThisFrame();
        attackIsPressed = _attackAction.IsPressed();
        attackWasReleased = _attackAction.WasReleasedThisFrame();

        interactWasPressed = _interactAction.WasPressedThisFrame();
        interactIsPressed = _interactAction.IsPressed();
        interactWasReleased = _interactAction.WasReleasedThisFrame();

        pauseWasPressed = _pauseAction.WasPressedThisFrame();

        secondaryAttackIsPressed = _secondaryAttackAction.IsPressed();
        secondaryAttackWasPressed = _secondaryAttackAction.WasPressedThisFrame();
        secondaryAttackWasReleased = _secondaryAttackAction.WasReleasedThisFrame();

    }
}
