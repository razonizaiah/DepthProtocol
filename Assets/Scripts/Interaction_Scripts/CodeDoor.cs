///////////////////////////////////////////////////////////////////////////////
//  Project:    DepthProtocol
//  Script:     CodeDoor.cs
//
//  Author:     Izaiah Razon
//  Created:    04/14/2026
//  Updated:    04/14/2026
//
//  Description:
//  Interactable door that requires the player to enter a numeric access code.
//  Opens when the correct code is submitted through the UIManager.
//
//  Notes:
//  - Inherits from InteractableBase.
//  - Uses UIManager to open and close the code entry panel.
//  - Uses DoorMotionBase to separate door logic from door motion behavior.
//
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;

public class CodeDoor : InteractableBase
{
    [Header("Door Settings")]
    [SerializeField] private string correctCode = "4312";
    [SerializeField] private bool isLocked = true;
    [SerializeField] private bool isOpen = false;

    [Header("References")]
    [SerializeField] private UIManager uiManager;
    [SerializeField] private DoorMotionBase doorMotion;

    private void Start()
    {
        ApplyDoorState();
    }

    protected override void OnInteract()
    {
        if (uiManager == null)
        {
            Debug.LogWarning("UIManager not assigned on CodeDoor.");
            return;
        }

        if (!isLocked)
        {
            ToggleDoor();
            return;
        }

        uiManager.ShowCodeInputPanel(
            "Door Access Panel",
            TryUnlockDoor,
            HandleCodeEntryCancelled
        );
    }

    private bool TryUnlockDoor(string enteredCode)
    {
        if (enteredCode == correctCode)
        {
            isLocked = false;
            ToggleDoor();

            if (uiManager != null)
            {
                uiManager.HideCodeInputPanel();
            }

            return true;
        }

        if (uiManager != null)
        {
            uiManager.SetCodePanelStatus("ACCESS DENIED");
        }

        return false;
    }

    private void HandleCodeEntryCancelled()
    {
        if (uiManager != null)
        {
            uiManager.HideCodeInputPanel();
        }
    }

    private void ToggleDoor()
    {
        isOpen = !isOpen;
        ApplyDoorState();
    }

    private void ApplyDoorState()
    {
        if (doorMotion != null)
        {
            doorMotion.SetOpen(isOpen);
        }
    }
    
        public void UnlockDoor()
    {
        isLocked = false;
    }

    public void OpenDoor()
    {
        isOpen = true;
        ApplyDoorState();
    }

    public void CloseDoor()
    {
        isOpen = false;
        ApplyDoorState();
    }

    public void UnlockAndOpenDoor()
    {
        isLocked = false;
        isOpen = true;
        ApplyDoorState();
    }
}