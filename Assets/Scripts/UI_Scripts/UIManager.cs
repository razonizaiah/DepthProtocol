///////////////////////////////////////////////////////////////////////////////
//  Project:    DepthProtocol
//  Script:     UIManager.cs
//
//  Author:     Izaiah Razon
//  Created:    04/13/2026
//  Updated:    04/14/2026
//
//  Description:
//  Top-level UI coordinator that reads gameplay state from the
//  InteractionController and forwards that state to specialized UI scripts.
//
//  Notes:
//  - Keeps UI systems modular by separating prompt logic and reticle logic.
//  - Does not perform its own detection logic.
//  - This script should live on a dedicated UI manager object.
//
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Gameplay References")]
    [SerializeField] private InteractionController interactionController;

    [Header("UI References")]
    [SerializeField] private InteractionPromptUI interactionPromptUI;
    [SerializeField] private ReticleUI reticleUI;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private MouseLook mouseLook;
    [SerializeField] private MemoryFragmentPanelUI memoryFragmentPanel;

    private void Update()
    {
        if (interactionController == null)
        {
            return;
        }

        InteractableBase currentInteractable = interactionController.CurrentInteractable;
        bool canInteract = currentInteractable != null && currentInteractable.CanInteract;

        if (interactionPromptUI != null)
        {
            interactionPromptUI.UpdatePrompt(currentInteractable, canInteract);
        }

        if (reticleUI != null)
        {
            reticleUI.UpdateReticle(canInteract);
        }
    }

    public void ShowMemoryFragment(string title, string body, System.Action onClose)
    {
        if (playerController != null)
        {
            playerController.SetMovementEnabled(false);
        }

        if (mouseLook != null)
        {
            mouseLook.SetLookEnabled(false);
        }

        if (interactionPromptUI != null)
        {
            interactionPromptUI.gameObject.SetActive(false);
        }

        if (reticleUI != null)
        {
            reticleUI.gameObject.SetActive(false);
        }

        memoryFragmentPanel.OpenPanel(title, body, () =>
        {
            if (playerController != null)
            {
            playerController.SetMovementEnabled(true);
            }

            if (mouseLook != null)
            {
                mouseLook.SetLookEnabled(true);
            }

            if (interactionPromptUI != null)
            {
                interactionPromptUI.gameObject.SetActive(true);
            }

            if (reticleUI != null)
            {
                reticleUI.gameObject.SetActive(true);
            }

            onClose?.Invoke();
        });
    }
}