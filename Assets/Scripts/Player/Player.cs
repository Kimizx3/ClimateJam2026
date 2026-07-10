using UnityEngine;
using UnityEngine.InputSystem;

public class Player : BaseCharacter
{
    private InputSystem_Actions _inputActions;
    [SerializeField] private KodakCapture kodakCapture;
    private bool _isAiming;

    protected override void Awake()
    {
        base.Awake();

        _inputActions = new InputSystem_Actions();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        _inputActions.Player.Move.performed += OnMovePerformed;
        _inputActions.Player.Move.canceled += OnMoveCanceled;

        _inputActions.Player.Look.performed += OnLookPerformed;
        _inputActions.Player.Look.canceled += OnLookCanceled;
        
        // Kodak Mechanics
        _inputActions.Player.Interact.started += OnEquipPerformed;

        _inputActions.LocalPlayer.Aim.started += OnCameraAimPerformed;
        _inputActions.LocalPlayer.Aim.canceled += OnCameraAimCanceled;

        _inputActions.Player.Attack.performed += OnAttackPerformed;
        
        _inputActions.Player.Enable();
        _inputActions.LocalPlayer.Enable();
    }

    private void OnDisable()
    {
        // Kodak Mechanics
        _inputActions.Player.Interact.started -= OnEquipPerformed;
        
        _inputActions.LocalPlayer.Aim.started -= OnCameraAimPerformed;
        _inputActions.LocalPlayer.Aim.canceled -= OnCameraAimCanceled;
        
        _inputActions.Player.Move.performed -= OnMovePerformed;
        _inputActions.Player.Move.canceled -= OnMoveCanceled;

        _inputActions.Player.Look.performed -= OnLookPerformed;
        _inputActions.Player.Look.canceled -= OnLookCanceled;

        _inputActions.Player.Attack.performed -= OnAttackPerformed;
        
        _inputActions.Player.Disable();
        _inputActions.LocalPlayer.Disable();
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        movementComponent.SetMoveInput(ctx.ReadValue<Vector2>());
    }
    
    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        movementComponent.SetMoveInput(Vector2.zero);
    }
    
    private void OnLookPerformed(InputAction.CallbackContext ctx)
    {
        lookComponent.SetLookInput(ctx.ReadValue<Vector2>());
    }
    
    private void OnLookCanceled(InputAction.CallbackContext ctx)
    {
        lookComponent.SetLookInput(Vector2.zero);
    }

    private void OnCameraAimPerformed(InputAction.CallbackContext ctx)
    {
        _isAiming = true;
        kodak.kodakEquip.SetAiming(_isAiming);
    }
    
    private void OnCameraAimCanceled(InputAction.CallbackContext ctx)
    {
        _isAiming = false;
        kodak.kodakEquip.SetAiming(_isAiming);
    }

    private void OnEquipPerformed(InputAction.CallbackContext ctx)
    {
        Debug.Log("Toggle Camera!");
        kodak.kodakEquip.ToggleCamera();
    }

    private void OnAttackPerformed(InputAction.CallbackContext ctx)
    {
        if (!_isAiming)
        {
            return;
        }
        
        kodakCapture.TryCapture();
    }
}
