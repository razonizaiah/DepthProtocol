///////////////////////////////////////////////////////////////////////////////
//  Project:    DepthProtocol
//  Script:     PuzzleSwitch.cs
//
//  Author:     Izaiah Razon
//  Created:    04/19/2026
//  Updated:    04/19/2026
//
//  Description:
//  Reusable interactable puzzle switch.
//  Can toggle or activate once, updates its own logical state, notifies an
//  assigned PuzzleManager, and forwards visual updates to a separate visual
//  responder script.
//
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;

public class PuzzleSwitch : InteractableBase
{
    [Header("Puzzle Switch Settings")]
    [SerializeField] private PuzzleManager puzzleManager;
    [SerializeField] private bool startsActive = false;
    [SerializeField] private bool canToggleOff = true;
    [SerializeField] private bool singleUse = false;
    [SerializeField] private bool logStateChanges = false;

    [Header("Visual Behavior")]
    [SerializeField] private PuzzleSwitchVisualBase visualHandler;

    private bool isActive = false;
    private bool hasBeenUsed = false;

    public bool IsActive => isActive;

    private void Awake()
    {
        isActive = startsActive;
        ApplyVisualState();
    }

    private void OnEnable()
    {
        if (puzzleManager != null)
        {
            puzzleManager.RegisterSwitch(this);
        }
    }

    private void OnDisable()
    {
        if (puzzleManager != null)
        {
            puzzleManager.UnregisterSwitch(this);
        }
    }

    protected override void OnInteract()
    {
        if (singleUse && hasBeenUsed)
        {
            return;
        }

        if (isActive)
        {
            if (canToggleOff)
            {
                SetActiveState(false);
            }
        }
        else
        {
            SetActiveState(true);
        }

        hasBeenUsed = true;

        if (singleUse)
        {
            SetCanInteract(false);
        }

        if (puzzleManager != null)
        {
            puzzleManager.NotifySwitchStateChanged();
        }
        else
        {
            Debug.LogWarning($"PuzzleSwitch on {gameObject.name} has no PuzzleManager assigned.");
        }
    }

    public void SetActiveState(bool value)
    {
        isActive = value;
        ApplyVisualState();

        if (logStateChanges)
        {
            Debug.Log($"PuzzleSwitch [{gameObject.name}] Active: {isActive}");
        }
    }

    private void ApplyVisualState()
    {
        if (visualHandler != null)
        {
            visualHandler.ApplyState(isActive);
        }
    }

    public override void SetHighlight(bool isHighlighted)
    {
        if (visualHandler != null && CanInteract)
        {
            visualHandler.ApplyHighlight(isHighlighted);
        }
    }
}