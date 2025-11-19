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
            standingHeight = characterController.height;
        }

        if (crouchHeight <= 0f)
        {
            crouchHeight = standingHeight * 0.6f;
        }

        initialStandingCenter = characterController.center;
        initialStandingHeight = standingHeight;

        if (playerCamera != null)
        {
            initialStandingCameraLocalPosition = playerCamera.transform.localPosition;

            if (standingCameraLocalPosition == Vector3.zero)
            {
                standingCameraLocalPosition = initialStandingCameraLocalPosition;
            }

            if (crouchCameraLocalPosition == Vector3.zero)
            {
                float downward = (standingHeight - crouchHeight) * 0.5f;
                crouchCameraLocalPosition = standingCameraLocalPosition + Vector3.down * downward;
            }
        }

        targetHeight = characterController.height;
        targetCenter = characterController.center;
        targetCameraLocalPosition = playerCamera != null ? playerCamera.transform.localPosition : Vector3.zero;
    }

    private void Start()
    {
        SetControllerImmediate(standingHeight, initialStandingCenter);
        if (playerCamera != null)
        {
            playerCamera.transform.localPosition = standingCameraLocalPosition;
        }
    }

    private void LateUpdate()
    {
        if (characterController == null)
        {
            return;
        }

        float newHeight = Mathf.Lerp(characterController.height, targetHeight, Time.deltaTime * transitionSpeed);
        characterController.height = newHeight;

        Vector3 newCenter = Vector3.Lerp(characterController.center, targetCenter, Time.deltaTime * transitionSpeed);
        characterController.center = newCenter;

        if (playerCamera != null)
        {
            playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition,
                targetCameraLocalPosition, Time.deltaTime * transitionSpeed);
        }
    }

    public void StartCrouch()
    {
        if (IsCrouching || characterController == null)
        {
            return;
        }
        float bottomBefore = GetControllerBottomY(characterController);

        IsCrouching = true;

        var sprint = GetComponent<PlayerSprint>();
        sprint?.OnTrySprintStop();

        float currentHeight = characterController.height;
        float newHeight = crouchHeight;

        Vector3 currentCenter = characterController.center;
        float newCenterY = currentCenter.y + (newHeight - currentHeight) * 0.5f;
        Vector3 newCenter = new Vector3(currentCenter.x, newCenterY, currentCenter.z);

        targetHeight = newHeight;
        targetCenter = newCenter;

        if (playerCamera != null)
        {
            if (crouchCameraLocalPosition != Vector3.zero)
            {
                targetCameraLocalPosition = crouchCameraLocalPosition;
            }
            else
            {
                float downward = (standingHeight - crouchHeight) * 0.5f;
                targetCameraLocalPosition = standingCameraLocalPosition + Vector3.down * downward;
            }
        }

        float bottomAfter = ComputeExecutedBottom(transform.position.y, newCenter.y, newHeight);
    }

    public void StopCrouch()
    {
        if (!IsCrouching || characterController == null)
        {
            return;
        }

        if (!CanStand())
        {
            Debug.Log($"{LOG}: Cannot stand up, obstacle detected above.");
            return;
        }

        float newHeight = standingHeight;
        Vector3 newCenter = ComputeCenterForHeight(newHeight);

        IsCrouching = false;

        targetHeight = newHeight;
        targetCenter = newCenter;
        targetCameraLocalPosition = standingCameraLocalPosition;
    }

    private bool CanStand()
    {
        if (characterController == null)
        {
            return true;
        }

        float currentHeight = characterController.height;
        Vector3 currentCenter = characterController.center;
        float newHeight = standingHeight;

        float bottom = GetControllerBottomY(characterController);
        Vector3 worldBottom = new Vector3(transform.position.x, bottom, transform.position.z);

        Vector3 topStanding = worldBottom + Vector3.up * newHeight;

        float radius = characterController.radius;
        Vector3 bottomPoint = worldBottom + Vector3.up * radius;
        Vector3 topPoint = topStanding - Vector3.up * radius;

        bool blocked = Physics.CheckCapsule(bottomPoint, topPoint, radius, obstacleMask, QueryTriggerInteraction.Ignore);
        return !blocked;
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

    private float GetControllerBottomY(CharacterController characterController)
    {
        return transform.position.y + characterController.center.y - characterController.height * 0.5f;
    }

    private float ComputeExecutedBottom(float worldPositionY, float centerY, float height)
    {
        return worldPositionY + centerY - height * 0.5f;
    }

    private Vector3 ComputeCenterForHeight(float desiredHeight)
    {
        float currentHeight = characterController.height;
        Vector3 currentCenter = characterController.center;
        float newCenterY = currentCenter.y + (desiredHeight - currentHeight) * 0.5f;
        return new Vector3(currentCenter.x, newCenterY, currentCenter.z);
    }
}
