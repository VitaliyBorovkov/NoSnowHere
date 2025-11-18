using UnityEngine;

public class PlayerSprint : MonoBehaviour
{
    private CharacterController characterController;

    public Vector2 inputVector;

    public bool isSprinting = false;
    public bool isHoldingSprintButton = false;

    private PlayerCrouch playerCrouch;

    public bool IsSprinting()
    {
        return isSprinting;
    }

    public bool IsHoldingSprintButton()
    {
        return isHoldingSprintButton;
    }

    public Vector2 GetInputVector()
    {
        return inputVector;
    }

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerCrouch = GetComponent<PlayerCrouch>();
    }

    private void Update()
    {
        isSprinting = isHoldingSprintButton && CanSprint();
    }

    public void SetInputVector(Vector2 input)
    {
        inputVector = input;
    }

    public bool CanSprint()
    {
        if (playerCrouch != null && playerCrouch.IsCrouching)
        {
            return false;
        }

        return inputVector != Vector2.zero && isHoldingSprintButton && characterController.isGrounded;
    }

    public void OnTrySprintStart()
    {
        isHoldingSprintButton = true;
    }

    public void OnTrySprintStop()
    {
        isHoldingSprintButton = false;
    }
}
