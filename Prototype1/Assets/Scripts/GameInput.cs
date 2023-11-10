using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    public event EventHandler OnPauseAction;
    public event EventHandler OnInteractAction;
    public event EventHandler OnToggleInventoryAction;
    public event EventHandler OnJumpAction;
    public event EventHandler OnSprintPressAction;
    public event EventHandler OnSprintReleaseAction;
    public event EventHandler OnCrouchPressAction;
    public event EventHandler OnCrouchReleaseAction;

    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        Instance = this;

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.Pause.performed += Pause_performed;
        playerInputActions.Player.Jump.performed += Jump_performed;
        playerInputActions.Player.Inventory.performed += ToggleInventory_performed;
        playerInputActions.Player.SprintPress.performed += SprintPress_performed;
        playerInputActions.Player.SprintRelease.performed += SprintRelease_performed;
        playerInputActions.Player.CrouchPress.performed += CrouchPress_performed;
        playerInputActions.Player.CrouchRelease.performed += CrouchRelease_performed;
    }

    private void OnDestroy()
    {
        playerInputActions.Player.Interact.performed -= Interact_performed;
        playerInputActions.Player.Pause.performed -= Pause_performed;
        playerInputActions.Player.Jump.performed -= Jump_performed;
        playerInputActions.Player.Inventory.performed -= ToggleInventory_performed;
        playerInputActions.Player.SprintPress.performed -= SprintPress_performed;
        playerInputActions.Player.SprintRelease.performed -= SprintRelease_performed;
        playerInputActions.Player.CrouchPress.performed -= CrouchPress_performed;
        playerInputActions.Player.CrouchRelease.performed -= CrouchRelease_performed;

        playerInputActions.Dispose();
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    private void ToggleInventory_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnToggleInventoryAction?.Invoke(this, EventArgs.Empty);
    }

    private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnJumpAction?.Invoke(this, EventArgs.Empty);
    }

    private void SprintPress_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnSprintPressAction?.Invoke(this, EventArgs.Empty);
    }

    private void SprintRelease_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnSprintReleaseAction?.Invoke(this, EventArgs.Empty);
    }

    private void CrouchPress_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnCrouchPressAction?.Invoke(this, EventArgs.Empty);
    }

    private void CrouchRelease_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnCrouchReleaseAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        return playerInputActions.Player.Move.ReadValue<Vector2>().normalized;
    }
}
