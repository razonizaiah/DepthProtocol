///////////////////////////////////////////////////////////////////////////////
//  Project:    DepthProtocol
//  Script:     ReticleUI.cs
//
//  Author:     Izaiah Razon
//  Created:    04/14/2026
//  Updated:    04/14/2026
//
//  Description:
//  Controls the center-screen reticle by adjusting its alpha and scale
//  depending on whether the player is currently aiming at an interactable.
//
//  Notes:
//  - Receives hover state from UIManager.
//  - Does not perform its own interaction checks.
//  - This script should live on the Reticle UI object.
//
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;

public class ReticleUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CanvasGroup reticleGroup;
    [SerializeField] private RectTransform reticleTransform;

    [Header("State Settings")]
    [SerializeField] private float idleAlpha = 0.45f;
    [SerializeField] private float hoverAlpha = 1f;
    [SerializeField] private float idleScale = 1f;
    [SerializeField] private float hoverScale = 1.2f;

    [Header("Animation Settings")]
    [SerializeField] private float transitionSpeed = 10f;

    public void UpdateReticle(bool isHoveringInteractable)
    {
        if (reticleGroup == null || reticleTransform == null)
        {
            return;
        }

        float targetAlpha = isHoveringInteractable ? hoverAlpha : idleAlpha;
        float targetScale = isHoveringInteractable ? hoverScale : idleScale;

        reticleGroup.alpha = Mathf.Lerp(
            reticleGroup.alpha,
            targetAlpha,
            transitionSpeed * Time.deltaTime
        );

        Vector3 desiredScale = Vector3.one * targetScale;
        reticleTransform.localScale = Vector3.Lerp(
            reticleTransform.localScale,
            desiredScale,
            transitionSpeed * Time.deltaTime
        );
    }
}