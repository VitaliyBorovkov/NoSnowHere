using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    private const float accelerationGravity = -2f;

    private PlayerMove playerMove;

    private bool isJumping = false;

    public float jumpHeight = 3f;
    public float jumpPower = -3f;

    public bool IsJumping()
    {
        return isJumping;
    }

    public void Initialize(PlayerMove move)
    {
        playerMove = move;
    }

    public void Jump()
    {
        if (playerMove.isGrounded)
        {
            float jumpVelocity = Mathf.Sqrt(accelerationGravity * playerMove.gravity * jumpHeight);
            playerMove.playerVelocity.y = jumpVelocity;
        }
    }
}
