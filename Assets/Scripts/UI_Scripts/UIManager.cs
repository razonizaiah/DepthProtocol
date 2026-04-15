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
//  - Manages modal UI states such as memory fragment reading and code entry.
//  - Freezes and restores player control during modal UI interactions.
//
///////////////////////////////////////////////////////////////////////////////

using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Gameplay References")]
    [SerializeField] private InteractionController interactionController;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private MouseLook mouseLook;

    [Header("UI References")]
    [SerializeField] private InteractionPromptUI interactionPromptUI;
    [SerializeField] private ReticleUI reticleUI;
    [SerializeField] private MemoryFragmentPanelUI memoryFragmentPanel;
    [SerializeField] private CodeInputPanelUI codeInputPanel;

    private bool isModalUIOpen = false;

    private void Update()
    {
        if (interactionController == null || isModalUIOpen)
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

    public void ShowMemoryFragment(string title, string body, Action onClose)
    {
        isModalUIOpen = true;
        SetGameplayInputEnabled(false);
        SetGameplayUIVisible(false);

        if (memoryFragmentPanel != null)
        {
            memoryFragmentPanel.OpenPanel(title, body, () =>
            {
                isModalUIOpen = false;
                SetGameplayInputEnabled(true);
                SetGameplayUIVisible(true);
                onClose?.Invoke();
            });
        }
    }

    public void ShowCodeInputPanel(string panelTitle, Func<string, bool> onSubmit, Action onCancel)
    {
        isModalUIOpen = true;
        SetGameplayInputEnabled(false);
        SetGameplayUIVisible(false);

        if (codeInputPanel != null)
        {
            codeInputPanel.OpenPanel(
                panelTitle,
                onSubmit,
                () =>
                {
                    HideCodeInputPanel();
                    onCancel?.Invoke();
                }
            );
        }
    }

    public void HideCodeInputPanel()
    {
        if (codeInputPanel != null)
        {
            codeInputPanel.ClosePanel();
        }

        isModalUIOpen = false;
        SetGameplayInputEnabled(true);
        SetGameplayUIVisible(true);
    }

    public void SetCodePanelStatus(string message)
    {
        if (codeInputPanel != null)
        {
            codeInputPanel.SetStatusMessage(message);
        }
    }

    private void SetGameplayInputEnabled(bool isEnabled)
    {
        if (playerController != null)
        {
            playerController.SetMovementEnabled(isEnabled);
        }

        if (mouseLook != null)
        {
            mouseLook.SetLookEnabled(isEnabled);
        }
    }

    private void SetGameplayUIVisible(bool isVisible)
    {
        if (interactionPromptUI != null)
        {
            interactionPromptUI.gameObject.SetActive(isVisible);
        }

        if (reticleUI != null)
        {
            reticleUI.gameObject.SetActive(isVisible);
        }
    }
}