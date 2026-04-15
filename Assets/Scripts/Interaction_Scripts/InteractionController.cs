///////////////////////////////////////////////////////////////////////////////
//  Project:    DepthProtocol
//  Script:     InteractionController.cs
//
//  Author:     Izaiah Razon
//  Created:    04/13/2026
//  Updated:    04/13/2026
//
//  Description:
//  Handles player interaction by raycasting from the camera forward direction.
//  Detects valid interactable objects within range and triggers interaction
//  when the player presses E.
//
//  Notes:
//  - Uses IInteractable for scalable interaction handling.
//  - Raycast origin and direction come from the assigned camera transform.
//  - This script should live on the Player object.
//
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float interactionRange = 3f;
    [SerializeField] private LayerMask interactionMask = ~0;
    [SerializeField] private float interactionRadius = 0.5f;
    
    [Header("References")]
    [SerializeField] private MouseLook mouseLook;

    private InteractableBase currentInteractable;
    public InteractableBase CurrentInteractable => currentInteractable;
    private RaycastHit currentHit;
    private InteractableBase lastInteractable;

    private void Update()
    {
        DetectInteractable();
        UpdateHighlight();

        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            if (mouseLook != null)
            {
                mouseLook.LockLookBriefly();
            }
            currentInteractable.Interact();
        }
    }

    private void DetectInteractable()
    {
        currentInteractable = null;

        if (cameraTransform == null)
        {
            return;
        }

        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);

        if (Physics.SphereCast(ray, interactionRadius, out currentHit, interactionRange, interactionMask))
        {
            currentInteractable = currentHit.collider.GetComponentInParent<InteractableBase>();
        }
    }

    public bool HasInteractable()
    {
        return currentInteractable != null;
    }

    public RaycastHit GetCurrentHit()
    {
        return currentHit;
    }

    private void OnDrawGizmosSelected()
    {
        if (cameraTransform == null)
        {
            return;
        }

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(
            cameraTransform.position + cameraTransform.forward * interactionRange,
            interactionRadius
        );
        Gizmos.DrawRay(cameraTransform.position, cameraTransform.forward * interactionRange);
    }

    private void UpdateHighlight()
    {
        if (currentInteractable == lastInteractable)
        {
            return;
        }

        if (lastInteractable != null)
        {
            lastInteractable.SetHighlight(false);
        }

        if (currentInteractable != null && currentInteractable.CanInteract)
        {
            currentInteractable.SetHighlight(true);
        }

        lastInteractable = currentInteractable;
    }
}