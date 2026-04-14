///////////////////////////////////////////////////////////////////////////////
//  Project:    Depth Protocol
//  Script:     InteractableBase.cs
//
//  Author:     Izaiah Razon
//  Created:    2026-04-13
//  Updated:    2026-04-13
//
//  Description:
//  Base class for all interactable objects in the game.
//  Provides shared interaction rules and a standard interaction entry point.
//
//  Notes:
//  - Implements IInteractable so derived objects can be detected by the
//    InteractionController.
//  - Child classes override OnInteract() to define custom behavior.
//
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;

public abstract class InteractableBase : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private string interactionName = "Interactable";
    [SerializeField] private bool canInteract = true;
    [SerializeField] private bool logInteraction = false;

    public string InteractionName => interactionName;
    public bool CanInteract => canInteract;

    public void Interact()
    {
        if (!canInteract)
        {
            return;
        }

        if (logInteraction)
        {
            Debug.Log($"Interacted with {interactionName}.");
        }

        OnInteract();
    }

    protected abstract void OnInteract();

    protected void SetCanInteract(bool value)
    {
        canInteract = value;
    }
}