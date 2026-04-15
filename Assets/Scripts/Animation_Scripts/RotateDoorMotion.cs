///////////////////////////////////////////////////////////////////////////////
//  Project:    DepthProtocol
//  Script:     RotateDoorMotion.cs
//
//  Author:     Izaiah Razon
//  Created:    04/14/2026
//  Updated:    04/14/2026
//
//  Description:
//  Door motion behavior that opens and closes a door by smoothly rotating a
//  target transform between closed and open Euler angles.
//
//  Notes:
//  - Inherits from DoorMotionBase.
//  - Can be reused by any hinged or rotating door.
//  - For hinge-style motion, the assigned doorVisual should have its pivot
//    positioned at the hinge side, typically using an empty parent object.
//
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;

public class RotateDoorMotion : DoorMotionBase
{
    [Header("References")]
    [SerializeField] private Transform doorVisual;

    [Header("Rotation Settings")]
    [SerializeField] private Vector3 closedRotation;
    [SerializeField] private Vector3 openRotation = new Vector3(0f, 90f, 0f);
    [SerializeField] private float rotationSpeed = 180f;

    private Quaternion targetRotation;

    private void Start()
    {
        if (doorVisual != null)
        {
            targetRotation = Quaternion.Euler(closedRotation);
            doorVisual.localRotation = targetRotation;
        }
    }

    private void Update()
    {
        if (doorVisual == null)
        {
            return;
        }

        doorVisual.localRotation = Quaternion.RotateTowards(
            doorVisual.localRotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    public override void SetOpen(bool isOpen)
    {
        targetRotation = Quaternion.Euler(isOpen ? openRotation : closedRotation);
    }
}