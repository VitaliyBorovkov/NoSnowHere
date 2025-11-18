using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(InputManager))]
public class InputHandler : MonoBehaviour
{
    private const string LOG = "InputManager";

    private InputManager inputManager;

    private PlayerMove playerMove;
    private PlayerLook playerLook;
    private PlayerJump playerJump;
    private PlayerCrouch playerCrouch;
    private PlayerSprint playerSprint;
    //private PlayerCollect playerCollect;
    //private PlayerInteract playerInteract;

    //private PlayerSwitchWeapon playerSwitchWeapon;

    private Vector2 moveInput;
    private Vector2 lookInput;

    private bool isEnabled = true;

    public bool IsEnabled => isEnabled;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();

        playerMove = GetComponent<PlayerMove>();
        playerLook = GetComponent<PlayerLook>();
        playerJump = GetComponent<PlayerJump>();
        playerSprint = GetComponent<PlayerSprint>();
        playerCrouch = GetComponent<PlayerCrouch>();
        //playerCollect = GetComponent<PlayerCollect>();
        //playerInteract = GetComponent<PlayerInteract>();

        //playerSwitchWeapon = GetComponent<PlayerSwitchWeapon>();

        if (playerJump != null && playerMove != null)
        {
            playerJump.Initialize(playerMove);
        }
        else
        {
            Debug.LogWarning($"{LOG}: PlayerJump or PlayerMove is missing on Player.");
        }
    }

    private void OnEnable()
    {
        if (isEnabled)
        {
            inputManager.SetActionsEnabled(true);
        }

        inputManager.Move.performed += OnMovePerformed;
        inputManager.Move.canceled += OnMoveCanceled;

        inputManager.Look.performed += OnLookPerformed;
        inputManager.Look.canceled += OnLookCanceled;

        inputManager.Jump.performed += OnJumpPerformed;

        inputManager.Sprint.started += OnSprintStarted;
        inputManager.Sprint.canceled += OnSprintCanceled;

        inputManager.Collect.started += OnCollectStarted;
        inputManager.Collect.canceled += OnCollectCanceled;

        inputManager.Interact.performed += OnInteractPerformed;

        inputManager.Crouch.performed += OnCrouchPerformed;
        inputManager.Crouch.canceled += OnCrouchCanceled;

        //inputManager.SwitchWeaponByScroll.performed += OnSwitchScrollPerformed;
        //inputManager.WeaponSlot1.performed += OnWeaponSlot1Performed;
        //inputManager.WeaponSlot2.performed += OnWeaponSlot2Performed;
        //inputManager.WeaponSlot3.performed += OnWeaponSlot3Performed;
    }

    private void OnDisable()
    {
        inputManager.Move.performed -= OnMovePerformed;
        inputManager.Move.canceled -= OnMoveCanceled;

        inputManager.Look.performed -= OnLookPerformed;
        inputManager.Look.canceled -= OnLookCanceled;

        inputManager.Jump.performed -= OnJumpPerformed;

        inputManager.Sprint.started -= OnSprintStarted;
        inputManager.Sprint.canceled -= OnSprintCanceled;

        inputManager.Collect.started -= OnCollectStarted;
        inputManager.Collect.canceled -= OnCollectCanceled;

        inputManager.Interact.performed -= OnInteractPerformed;

        inputManager.Crouch.performed -= OnCrouchPerformed;
        inputManager.Crouch.canceled -= OnCrouchCanceled;

        //inputManager.SwitchWeaponByScroll.performed -= OnSwitchScrollPerformed;
        //inputManager.WeaponSlot1.performed -= OnWeaponSlot1Performed;
        //inputManager.WeaponSlot2.performed -= OnWeaponSlot2Performed;
        //inputManager.WeaponSlot3.performed -= OnWeaponSlot3Performed;

        inputManager.SetActionsEnabled(false);

        moveInput = Vector2.zero;
        lookInput = Vector2.zero;
    }

    private void Update()
    {
        if (!isEnabled)
        {
            return;
        }

        if (playerMove != null)
        {
            playerMove.Move(moveInput);
            playerSprint?.SetInputVector(moveInput);
        }
    }

    private void LateUpdate()
    {
        if (!isEnabled)
        {
            return;
        }

        if (playerLook != null && playerLook.playerCamera != null)
        {
            playerLook.Look(lookInput);
        }
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        if (!isEnabled)
        {
            return;
        }

        moveInput = ctx.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        if (!isEnabled)
        {
            return;
        }

        moveInput = Vector2.zero;
    }

    private void OnLookPerformed(InputAction.CallbackContext ctx)
    {
        if (!isEnabled)
        {
            return;
        }

        lookInput = ctx.ReadValue<Vector2>();
    }

    private void OnLookCanceled(InputAction.CallbackContext ctx)
    {
        if (!isEnabled)
        {
            return;
        }

        lookInput = Vector2.zero;
    }

    private void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        if (!isEnabled)
        {
            return;
        }

        playerJump?.Jump();
    }

    private void OnSprintStarted(InputAction.CallbackContext ctx)
    {
        if (!isEnabled)
        {
            return;
        }

        playerSprint?.OnTrySprintStart();
    }

    private void OnSprintCanceled(InputAction.CallbackContext ctx)
    {
        if (!isEnabled)
        {
            return;
        }

        playerSprint?.OnTrySprintStop();
    }

    private void OnCrouchPerformed(InputAction.CallbackContext ctx)
    {
        if (!isEnabled)
        {
            return;
        }
        playerCrouch?.StartCrouch();
    }

    private void OnCrouchCanceled(InputAction.CallbackContext ctx)
    {
        if (!isEnabled)
        {
            return;
        }
        playerCrouch?.StopCrouch();
    }

    private void OnCollectStarted(InputAction.CallbackContext ctx)
    {
        if (!isEnabled)
        {
            return;
        }

        //playerCollect?.StartCollect();
    }

    private void OnCollectCanceled(InputAction.CallbackContext ctx)
    {
        if (!isEnabled)
        {
            return;
        }

        //playerCollect?.StopCollect();
    }

    private void OnInteractPerformed(InputAction.CallbackContext ctx)
    {
        if (!isEnabled)
        {
            return;
        }

        //playerInteract?.Interact();
    }

    //private void OnSwitchScrollPerformed(InputAction.CallbackContext ctx)
    //{
    //    if (!isEnabled)
    //    {
    //        return;
    //    }

    //    playerSwitchWeapon?.HandleScrollWeapon(ctx);
    //}

    //private void OnWeaponSlot1Performed(InputAction.CallbackContext ctx)
    //{
    //    if (!isEnabled)
    //    {
    //        return;
    //    }

    //    playerSwitchWeapon?.SwitchWeaponByIndex(0);
    //}

    //private void OnWeaponSlot2Performed(InputAction.CallbackContext ctx)
    //{
    //    if (!isEnabled)
    //    {
    //        return;
    //    }

    //    playerSwitchWeapon?.SwitchWeaponByIndex(1);
    //}

    //private void OnWeaponSlot3Performed(InputAction.CallbackContext ctx)
    //{
    //    if (!isEnabled)
    //    {
    //        return;
    //    }

    //    playerSwitchWeapon?.SwitchWeaponByIndex(2);
    //}

    public void SetEnabled(bool value)
    {
        if (isEnabled == value)
        {
            return;
        }

        isEnabled = value;
        if (!isEnabled)
        {
            //playerCollect?.StopCollect();
            moveInput = Vector2.zero;
            lookInput = Vector2.zero;
        }

        if (inputManager != null)
        {
            inputManager.SetActionsEnabled(isEnabled);
        }
        Debug.Log($"{LOG}: Input is now {(isEnabled ? "enabled" : "disabled")}");
    }
}
