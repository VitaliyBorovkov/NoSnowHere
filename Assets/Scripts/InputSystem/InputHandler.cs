//using UnityEngine;

//[RequireComponent(typeof(InputManager))]
//public class InputHandler : MonoBehaviour
//{
//    private const string LOG = "InputManager";

//    private InputManager inputManager;

//    private PlayerMove playerMove;
//    private PlayerLook playerLook;
//    private PlayerJump playerJump;
//    private PlayerCrouch playerCrouch;
//    private PlayerSprint playerSprint;
//    private PlayerSprint PlayerCollect;
//    private PlayerInteract playerInteract;

//    private PlayerSwitchWeapon playerSwitchWeapon;

//    private Vector2 moveInput;
//    private Vector2 lookInput;

//    private bool isEnabled = true;

//    public bool IsEnabled => isEnabled;

//    private void Awake()
//    {
//        inputManager = GetComponent<InputManager>();

//        playerMove = GetComponent<PlayerMove>();
//        playerLook = GetComponent<PlayerLook>();
//        playerJump = GetComponent<PlayerJump>();
//        playerSprint = GetComponent<PlayerSprint>();
//        playerCollect = GetComponent<PlayerCollect>();
//        playerInteract = GetComponent<PlayerInteract>();

//        playerSwitchWeapon = GetComponent<PlayerSwitchWeapon>();

//        if (playerJump != null && playerMove != null)
//        {
//            playerJump.Initialize(playerMove);
//        }
//        else
//        {
//            Debug.LogWarning($"{LOG}: PlayerJump or PlayerMove is missing on Player.");
//        }
//    }

//    private void OnEnable()
//    {
//        if (isEnabled)
//        {
//            inputManager.SetActionsEnabled(true);
//        }

//        inputManager.Move.performed += OnMovePerformed;
//        inputManager.Move.canceled += OnMoveCanceled;

//        inputManager.Look.performed += OnLookPerformed;
//        inputManager.Look.canceled += OnLookCanceled;

//        inputManager.Jump.performed += OnJumpPerformed;

//        inputManager.Sprint.started += OnSprintStarted;
//        inputManager.Sprint.canceled += OnSprintCanceled;

//        inputManager.Sprint.started += OnSprintStarted;
//        inputManager.Sprint.canceled += OnSprintCanceled;

//        inputManager.Interact.performed += OnInteractPerformed;

//        inputManager.SwitchWeaponByScroll.performed += OnSwitchScrollPerformed;
//        inputManager.WeaponSlot1.performed += OnWeaponSlot1Performed;
//        inputManager.WeaponSlot2.performed += OnWeaponSlot2Performed;
//        inputManager.WeaponSlot3.performed += OnWeaponSlot3Performed;
//    }
//}
