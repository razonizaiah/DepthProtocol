///////////////////////////////////////////////////////////////////////////////
//  Project:    DepthProtocol
//  Script:     SwitchColorChange.cs
//
//  Author:     Izaiah Razon
//  Created:    04/19/2026
//  Updated:    04/19/2026
//
//  Description:
//  Simple visual responder for PuzzleSwitch.
//  Toggles assigned active/inactive visuals and optional highlight object.
//
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;

public class SwitchColorChange : PuzzleSwitchVisualBase
{
    [Header("State Visuals")]
    [SerializeField] private GameObject activeVisual;
    [SerializeField] private GameObject inactiveVisual;

    [Header("Highlight Visual")]
    [SerializeField] private GameObject highlightVisual;

    public override void ApplyState(bool isActive)
    {
        if (activeVisual != null)
        {
            activeVisual.SetActive(isActive);
        }

        if (inactiveVisual != null)
        {
            inactiveVisual.SetActive(!isActive);
        }
    }

    public override void ApplyHighlight(bool isHighlighted)
    {
        if (highlightVisual != null)
        {
            highlightVisual.SetActive(isHighlighted);
        }
    }
}