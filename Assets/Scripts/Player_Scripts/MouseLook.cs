using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerBody;
    [SerializeField] private Transform orientation;

    [Header("Look Settings")]
    [SerializeField] private float mouseSensitivity = 150f;
    [SerializeField] private float minPitch = -75f;
    [SerializeField] private float maxPitch = 75f;

    [Header("Tilt Settings")]
    [SerializeField] private float maxStrafeTiltAngle = 8f;
    [SerializeField] private float maxMoveTiltAngle = 4f;
    [SerializeField] private float tiltSpeed = 5f;

    [Header("Sprint FOV Settings")]
    [SerializeField] private float normalFOV = 60f;
    [SerializeField] private float sprintFOV = 68f;
    [SerializeField] private float fovLerpSpeed = 6f;

    [Header("Interaction Look Lock")]
    [SerializeField] private float interactLookLockDuration = 0.05f;

    private Camera playerCamera;
    private float basePitch = 0f;
    private float currentRollTilt = 0f;
    private float currentMoveTilt = 0f;
    private float lookLockTimer = 0f;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerCamera = GetComponent<Camera>();

        if (playerCamera != null)
        {
            playerCamera.fieldOfView = normalFOV;
        }
    }

    private void Update()
    {
        if (playerBody == null || orientation == null || playerCamera == null)
        {
            return;
        }

        if (lookLockTimer > 0f)
        {
            lookLockTimer -= Time.deltaTime;
            return;
        }

        LookAround();
        HandleTilt();
        ApplyCameraRotation();
        UpdateOrientation();
        HandleSprintFOV();
    }

    private void LookAround()
    {
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        if (Mathf.Abs(mouseX) < 0.001f) mouseX = 0f;
        if (Mathf.Abs(mouseY) < 0.001f) mouseY = 0f;

        mouseX *= mouseSensitivity * Time.deltaTime;
        mouseY *= mouseSensitivity * Time.deltaTime;

        basePitch -= mouseY;
        basePitch = Mathf.Clamp(basePitch, minPitch, maxPitch);

        playerBody.Rotate(Vector3.up * mouseX);
    }

    private void HandleTilt()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        float targetRollTilt = -horizontalInput * maxStrafeTiltAngle;
        float targetMoveTilt = verticalInput * maxMoveTiltAngle;

        currentRollTilt = Mathf.Lerp(currentRollTilt, targetRollTilt, tiltSpeed * Time.deltaTime);
        currentMoveTilt = Mathf.Lerp(currentMoveTilt, targetMoveTilt, tiltSpeed * Time.deltaTime);
    }

    private void ApplyCameraRotation()
    {
        float finalPitch = basePitch + currentMoveTilt;
        transform.localRotation = Quaternion.Euler(finalPitch, 0f, currentRollTilt);
    }

    private void UpdateOrientation()
    {
        orientation.rotation = Quaternion.Euler(basePitch, playerBody.eulerAngles.y, 0f);
    }

    private void HandleSprintFOV()
    {
        bool isSprintingForward = Input.GetKey(KeyCode.LeftShift) && Input.GetAxisRaw("Vertical") > 0f;
        float targetFOV = isSprintingForward ? sprintFOV : normalFOV;

        playerCamera.fieldOfView = Mathf.Lerp(
            playerCamera.fieldOfView,
            targetFOV,
            fovLerpSpeed * Time.deltaTime
        );
    }
    public void LockLookBriefly()
    {
        lookLockTimer = interactLookLockDuration;
    }
}