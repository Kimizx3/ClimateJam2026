using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : BaseCharacter
{
    private InputSystem_Actions _inputActions;

    protected override void Awake()
    {
        base.Awake();

        _inputActions = new InputSystem_Actions();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        _inputActions.Enable();

        _inputActions.Player.Move.performed += ctx =>
        {
            _movementComponent.SetMoveInput(ctx.ReadValue<Vector2>());
        };

        _inputActions.Player.Move.canceled += ctx =>
        {
            _movementComponent.SetMoveInput(Vector2.zero);
        };

        _inputActions.Player.Look.performed += ctx =>
        {
            _lookComponent.SetLookInput(ctx.ReadValue<Vector2>());
        };

        _inputActions.Player.Look.canceled += ctx =>
        {
            _lookComponent.SetLookInput(Vector2.zero);
        };
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }
}
