///////////////////////////////////////////////////////////////////////////////
//  Project:    DepthProtocol
//  Script:     DoorMotionBase.cs
//
//  Author:     Izaiah Razon
//  Created:    04/14/2026
//  Updated:    04/14/2026
//
//  Description:
//  Base class for door motion behaviors.
//  Defines a common interface for opening and closing doors visually.
//
//  Notes:
//  - CodeDoor uses this class so door logic stays separate from motion logic.
//  - Different motion types can inherit from this base class.
//  - Examples: rotation, sliding, Animator-driven movement.
//
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;

public abstract class DoorMotionBase : MonoBehaviour
{
    public abstract void SetOpen(bool isOpen);
}