///////////////////////////////////////////////////////////////////////////////
//  Project:    Depth Protocol
//  Script:     MouseLook.cs
//
//  Author:     Izaiah Razon
//  Created:    2026-04-13
//  Updated:    2026-04-13
//
//  Description:
//  Handles mouse-based camera rotation with visual camera tilt.
//  Tilt does not affect movement direction.
//
//  Notes:
//  - Pitch is clamped to prevent disorientation
//  - Roll tilt is applied when strafing
//  - Extra visual pitch tilt is applied when moving forward/backward
//
//  Dependencies:
//  - Requires reference to player body transform
//  - Requires reference to orientation transform
//
//  Unity Version: 2022+
//
///////////////////////////////////////////////////////////////////////////////

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
    private Camera playerCamera;
    [SerializeField] private float normalFOV = 60f;
    [SerializeField] private float sprintFOV = 68f;
    [SerializeField] private float fovLerpSpeed = 6f;

    private float basePitch = 0f;
    private float currentRollTilt = 0f;
    private float currentMoveTilt = 0f;

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
        LookAround();
        HandleTilt();
        ApplyCameraRotation();
        UpdateOrientation();
        HandleSprintFOV();
    }

    private void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        basePitch -= mouseY;
        basePitch = Mathf.Clamp(basePitch, minPitch, maxPitch);

        if (playerBody != null)
        {
            playerBody.Rotate(Vector3.up * mouseX);
        }
        else
        {
            Debug.LogWarning("MouseLook: Player Body reference is missing.");
        }
    }

    private void HandleTilt()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        float targetRollTilt = -horizontalInput * maxStrafeTiltAngle;

        // W = slight downward camera tilt
        // S = slight upward camera tilt
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
        if (orientation != null && playerBody != null)
        {
            orientation.rotation = Quaternion.Euler(basePitch, playerBody.eulerAngles.y, 0f);
        }
        else
        {
            Debug.LogWarning("MouseLook: Orientation reference is missing.");
        }
    }

    private void HandleSprintFOV()
    {
        if (playerCamera == null)
        {
            Debug.LogWarning("MouseLook: Player Camera reference is missing.");
            return;
        }

        bool isSprintingForward = Input.GetKey(KeyCode.LeftShift) && Input.GetAxisRaw("Vertical") > 0f;
        float targetFOV = isSprintingForward ? sprintFOV : normalFOV;

        playerCamera.fieldOfView = Mathf.Lerp(
            playerCamera.fieldOfView,
            targetFOV,
            fovLerpSpeed * Time.deltaTime
        );
    }
}