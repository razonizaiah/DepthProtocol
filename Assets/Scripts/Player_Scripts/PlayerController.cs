///////////////////////////////////////////////////////////////////////////////
//  Project:    Depth Protocol
//  Script:     PlayerController.cs
//
//  Author:     Izaiah Razon
//  Created:    2026-04-13
//  Updated:    2026-04-13
//
//  Description:
//  Handles player movement using a Rigidbody with smooth acceleration
//  and deceleration. Supports camera-relative movement (WASD) and
//  world-space vertical movement (Space / C).
//
//  Notes:
//  - Movement is smoothed for better feel
//  - Vertical speed is slower than forward speed
//
//  Dependencies:
//  - Requires Rigidbody component
//  - Requires reference to orientation (camera transform)
//
//  Unity Version: 2022+
//
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform orientation;

    [Header("Movement Speeds")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float verticalSpeed = 3.5f;

    [Header("Sprint Settings")]
    [SerializeField] private float sprintMultiplier = 1.8f;

    [Header("Smoothing")]
    [SerializeField] private float acceleration = 8f;
    [SerializeField] private float deceleration = 10f;

    private Rigidbody rb;

    private float horizontalInput;
    private float verticalInput;
    private float worldVerticalInput;
    private bool isSprinting;
    private bool movementEnabled = true;

    private Vector3 currentVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!movementEnabled)
        {
            return;
        }
        
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        isSprinting = Input.GetKey(KeyCode.LeftShift);

        worldVerticalInput = 0f;

        if (Input.GetKey(KeyCode.Space))
            worldVerticalInput += 1f;

        if (Input.GetKey(KeyCode.C))
            worldVerticalInput -= 1f;
    }

    private void FixedUpdate()
    {
        if (!movementEnabled)
        {
            return;
        }

        MovePlayer();
    }

    private void MovePlayer()
    {
        if (orientation == null)
        {
            Debug.LogWarning("PlayerController: Orientation reference is missing.");
            return;
        }

        // Camera-relative movement
        Vector3 localMove =
            orientation.forward * verticalInput +
            orientation.right * horizontalInput;

        if (localMove.sqrMagnitude > 1f)
            localMove.Normalize();

        float currentMoveSpeed = isSprinting ? moveSpeed * sprintMultiplier : moveSpeed;

        // WASD movement (affected by sprint)
        Vector3 localHorizontal = new Vector3(localMove.x, 0f, localMove.z) * currentMoveSpeed;
        Vector3 localVertical = Vector3.up * (localMove.y * verticalSpeed);

        // Space / C movement
        Vector3 globalVertical = Vector3.up * (worldVerticalInput * verticalSpeed);

        Vector3 targetVelocity = localHorizontal + localVertical + globalVertical;

        // Choose accel vs decel
        float rate = (targetVelocity.magnitude > currentVelocity.magnitude)
            ? acceleration
            : deceleration;

        // Smoothly move toward target velocity
        currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, rate * Time.fixedDeltaTime);

        rb.linearVelocity = currentVelocity;
    }
    
    public void SetMovementEnabled(bool isEnabled)
    {
        movementEnabled = isEnabled;

        if (!isEnabled)
        {
            currentVelocity = Vector3.zero;
            rb.linearVelocity = Vector3.zero;
        }
    }
}