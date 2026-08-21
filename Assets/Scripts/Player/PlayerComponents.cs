using UnityEngine;

[DefaultExecutionOrder(-50)]
public class PlayerComponents : MonoBehaviour
{
    public InputManager InputManager { get; private set; }
    public CharacterController CharacterController { get; private set; }
    public PlayerMove Move { get; private set; }
    public PlayerLook Look { get; private set; }
    public PlayerJump Jump { get; private set; }
    public PlayerSprint Sprint { get; private set; }
    public PlayerCrouch Crouch { get; private set; }
    public PlayerCollect Collect { get; private set; }
    public PlayerInventory Inventory { get; private set; }
    public PlayerWallet Wallet { get; private set; }

    private void Awake()
    {
        InputManager = GetComponent<InputManager>();
        CharacterController = GetComponent<CharacterController>();
        Move = GetComponent<PlayerMove>();
        Look = GetComponent<PlayerLook>();
        Jump = GetComponent<PlayerJump>();
        Sprint = GetComponent<PlayerSprint>();
        Crouch = GetComponent<PlayerCrouch>();
        Collect = GetComponent<PlayerCollect>();
        Inventory = GetComponent<PlayerInventory>();
        Wallet = GetComponent<PlayerWallet>();
    }
}
