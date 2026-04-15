///////////////////////////////////////////////////////////////////////////////
//  Project:    DepthProtocol
//  Script:     InteractionPromptUI.cs
//
//  Author:     Izaiah Razon
//  Created:    04/14/2026
//  Updated:    04/14/2026
//
//  Description:
//  Controls the interaction prompt UI by updating prompt text, object name,
//  and fade visibility based on the currently targeted interactable.
//
//  Notes:
//  - Receives interaction state from UIManager.
//  - Does not perform any raycasts or gameplay detection.
//  - This script should live on the InteractionPrompt UI object.
//
///////////////////////////////////////////////////////////////////////////////

using TMPro;
using UnityEngine;

public class InteractionPromptUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CanvasGroup interactionPromptGroup;
    [SerializeField] private TMP_Text actionText;
    [SerializeField] private TMP_Text objectNameText;

    [Header("Prompt Settings")]
    [SerializeField] private bool showObjectName = true;
    [SerializeField] private float fadeSpeed = 8f;

    private InteractableBase lastInteractable;

    private void Start()
    {
        if (interactionPromptGroup != null)
        {
            interactionPromptGroup.alpha = 0f;
            interactionPromptGroup.interactable = false;
            interactionPromptGroup.blocksRaycasts = false;
        }
    }

    public void UpdatePrompt(InteractableBase interactable, bool shouldShowPrompt)
    {
        if (interactionPromptGroup == null)
        {
            return;
        }

        if (interactable != lastInteractable)
        {
            RefreshPromptText(interactable);
            lastInteractable = interactable;
        }

        UpdatePromptVisibility(shouldShowPrompt);
    }

    private void RefreshPromptText(InteractableBase interactable)
    {
        if (actionText != null)
        {
            actionText.text = interactable != null
                ? interactable.InteractionPrompt
                : string.Empty;
        }

        if (objectNameText != null)
        {
            bool shouldShowName = showObjectName && interactable != null;
            objectNameText.gameObject.SetActive(shouldShowName);

            if (shouldShowName)
            {
                objectNameText.text = interactable.InteractionName;
            }
        }
    }

    private void UpdatePromptVisibility(bool shouldShow)
    {
        float targetAlpha = shouldShow ? 1f : 0f;

        interactionPromptGroup.alpha = Mathf.Lerp(
            interactionPromptGroup.alpha,
            targetAlpha,
            fadeSpeed * Time.deltaTime
        );

        if (Mathf.Abs(interactionPromptGroup.alpha - targetAlpha) < 0.01f)
        {
            interactionPromptGroup.alpha = targetAlpha;
        }
    }
}