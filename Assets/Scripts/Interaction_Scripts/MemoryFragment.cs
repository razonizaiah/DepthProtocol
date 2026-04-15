///////////////////////////////////////////////////////////////////////////////
//  Project:    Depth Protocol
//  Script:     MemoryFragment.cs
//
//  Author:     Izaiah Razon
//  Created:    2026-04-13
//  Updated:    2026-04-13
//
//  Description:
//  Simple interactable memory fragment.
//  When interacted with, logs a collection message and can optionally
//  disable itself.
//
//  Notes:
//  - Inherits from InteractableBase.
//  - Serves as the first real interactable object for testing and expansion.
//
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;

public class MemoryFragment : InteractableBase
{
    [Header("Memory Fragment Settings")]
    [SerializeField] private string fragmentID = "Fragment_01";
    [SerializeField] private bool disableAfterInteraction = true;
    [SerializeField] private GameObject highlightVisual;
    [SerializeField] private string fragmentTitle = "Memory Fragment";
    [SerializeField] [TextArea] private string fragmentText;
    [SerializeField] private UIManager uiManager;

    protected override void OnInteract()
    {
        if (uiManager == null)
        {
            Debug.LogWarning("UIManager not assigned.");
            return;
        }

        uiManager.ShowMemoryFragment(fragmentTitle, fragmentText, () =>
        {
            Debug.Log($"Memory Fragment collected: {fragmentID}");

            if (disableAfterInteraction)
            {
                SetCanInteract(false);
                gameObject.SetActive(false);
            }
        });
    }

    public override void SetHighlight(bool isHighlighted)
    {
        if (highlightVisual != null && CanInteract)
        {
            highlightVisual.SetActive(isHighlighted);
        }
    }
}