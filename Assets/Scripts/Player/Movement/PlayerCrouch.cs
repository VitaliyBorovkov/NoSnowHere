using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerCrouch : MonoBehaviour
{
    private const string LOG = "PlayerCrouch";

    public Camera playerCamera;
    public CharacterController characterController;

    public float standingHeight = 0f;
    public float crouchHeight = 0f;

    public Vector3 standingCameraLocalPosition = Vector3.zero;
    public Vector3 crouchCameraLocalPosition = Vector3.zero;

    public float transitionSpeed = 8f;

    public LayerMask obstacleMask = ~0;

    public float crouchSpeedMultiplier = 0.5f;

    public bool IsCrouching { get; private set; } = false;

    private float targetHeight;
    private Vector3 targetCenter;
    private Vector3 targetCameraLocalPosition;
    private float initialStandingHeight;
    private Vector3 initialStandingCenter;
    private Vector3 initialStandingCameraLocalPosition;

    private void Awake()
    {
        if (characterController == null)
        {
            characterController = GetComponent<CharacterController>();
        }

        if (standingHeight <= 0f)
        {
            standingHeight = characterController != null ? characterController.height : 1.8f;
        }

        if (crouchHeight <= 0f)
        {
            crouchHeight = standingHeight * 0.6f;
        }

        initialStandingCenter = characterController != null ? characterController.center : Vector3.up *
            (standingHeight / 2f);

        if (playerCamera != null)
        {
            initialStandingCameraLocalPosition = playerCamera.transform.localPosition;

            if (standingCameraLocalPosition == Vector3.zero)
            {
                standingCameraLocalPosition = initialStandingCameraLocalPosition;
            }

            if (crouchCameraLocalPosition == Vector3.zero)
            {
                crouchCameraLocalPosition = standingCameraLocalPosition + Vector3.down *
                    (standingHeight - crouchHeight * 0.5f);
            }
        }

        initialStandingHeight = standingHeight;

        targetHeight = standingHeight;
        targetCenter = initialStandingCenter;
        targetCameraLocalPosition = standingCameraLocalPosition;
    }

    private void Start()
    {
        SetControllerImmediate(standingHeight, initialStandingCenter);
    }

    private void LateUpdate()
    {
        if (characterController != null)
        {
            float newHeight = Mathf.Lerp(characterController.height, targetHeight, Time.deltaTime * transitionSpeed);
            characterController.height = newHeight;

            Vector3 newCenter = Vector3.Lerp(characterController.center, targetCenter, Time.deltaTime * transitionSpeed);
            characterController.center = newCenter;
        }

        if (playerCamera != null)
        {
            playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition,
                targetCameraLocalPosition, Time.deltaTime * transitionSpeed);
        }
    }

    public void StartCrouch()
    {
        if (IsCrouching)
        {
            return;
        }

        IsCrouching = true;

        var sprint = GetComponent<PlayerSprint>();
        sprint?.OnTrySprintStop();

        targetHeight = crouchHeight;
        targetCenter = Vector3.up * (crouchHeight / 2f);
        targetCameraLocalPosition = crouchCameraLocalPosition;

        Debug.Log($"{LOG}: Crouch started.");
    }

    public void StopCrouch()
    {
        if (!IsCrouching)
        {
            return;
        }

        if (!CanStand())
        {
            Debug.Log($"{LOG}: Cannot stand up, obstacle detected above.");
            return;
        }

        IsCrouching = false;
        targetHeight = standingHeight;
        targetCenter = Vector3.up * (crouchHeight / 2f);
        targetCameraLocalPosition = standingCameraLocalPosition;
        Debug.Log($"{LOG}: Stand up.");
    }

    private void SetControllerImmediate(float height, Vector3 center)
    {
        if (characterController == null)
        {
            return;
        }

        characterController.height = height;
        characterController.center = center;
    }

    private bool CanStand()
    {
        if (characterController == null)
        {
            return true;
        }

        float radius = characterController.radius;
        float halfStanding = standingHeight / 2f;
        float halfCurrnet = characterController.height / 2f;

        Vector3 worldCenter = transform.position + characterController.center;
        Vector3 bottom = worldCenter - Vector3.up * halfCurrnet + Vector3.up * radius;
        Vector3 topStanding = worldCenter + Vector3.up * (halfStanding - radius);

        bool blocked = Physics.CheckCapsule(bottom, topStanding, radius, obstacleMask, QueryTriggerInteraction.Ignore);
        return !blocked;
    }
}
