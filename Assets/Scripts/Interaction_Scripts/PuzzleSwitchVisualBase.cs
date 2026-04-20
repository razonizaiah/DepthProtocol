///////////////////////////////////////////////////////////////////////////////
//  Project:    DepthProtocol
//  Script:     PuzzleSwitchVisualBase.cs
//
//  Author:     Izaiah Razon
//  Created:    04/19/2026
//  Updated:    04/19/2026
//
//  Description:
//  Base class for puzzle switch visual responders.
//  Allows PuzzleSwitch to notify another script when its active state changes,
//  without hardcoding one specific visual behavior into the switch itself.
//
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;

public abstract class PuzzleSwitchVisualBase : MonoBehaviour
{
    public abstract void ApplyState(bool isActive);
    public virtual void ApplyHighlight(bool isHighlighted) { }
}