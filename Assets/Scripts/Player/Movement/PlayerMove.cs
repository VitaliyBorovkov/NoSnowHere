using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private const string LOG = "PlayerMove";
    private const float gravityPressure = -0.5f;

    public CharacterController characterController;

    public PlayerData playerData;
    public Vector3 playerVelocity;

    private PlayerSprint playerSprint;
    private PlayerCrouch playerCrouch;

    public bool isGrounded;

    public float gravity = gravityPressure;

    public float crouchSpeedMultiplier = 0.5f;

    [HideInInspector] public float walkSpeed;
    [HideInInspector] public float runSpeed;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerSprint = GetComponent<PlayerSprint>();
        playerCrouch = GetComponent<PlayerCrouch>();

        if (characterController == null || playerSprint == null)
        {
            Debug.Log($"{LOG}: CharacterController or PlayerRun not found");
        }
    }

    private void FixedUpdate()
    {
        walkSpeed = playerData.WalkSpeed;
        runSpeed = playerData.RunSpeed;
        isGrounded = characterController.isGrounded;
    }

    public void Move(Vector2 input)
    {
        playerSprint.inputVector = input;

        Vector3 moveDirection = transform.TransformDirection(new Vector3(input.x, 0f, input.y));

        float targetSpeed = playerSprint.IsSprinting() ? runSpeed : walkSpeed;
        if (playerCrouch != null && playerCrouch.IsCrouching)
        {
            targetSpeed = walkSpeed * crouchSpeedMultiplier;
        }

        characterController.Move(targetSpeed * Time.deltaTime * moveDirection);

        if (characterController.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = gravity * Time.deltaTime;
        }
        else
        {
            playerVelocity.y += gravity * Time.deltaTime;
        }

        characterController.Move(playerVelocity * Time.deltaTime);
    }
}
