# Depth Protocol

## Overview

**Depth Protocol** is a first-person exploration game built in Unity.
The game focuses on movement, atmosphere, and environmental interaction inside an underwater cave system with technological ruins.

This project is currently in an early prototype stage.

---

## Current Prototype (v0.1)

This version includes a **playable loop**:

1. Player spawns in a cave environment
2. A locked door blocks the main path
3. Player explores a side path
4. Finds a memory fragment containing a code
5. Returns to the door and unlocks it
6. Progresses forward

---

## Features Implemented

### Player Movement

* Rigidbody-based floating movement (no gravity)
* Camera-relative WASD controls
* Vertical movement (up/down)
* Smooth acceleration and deceleration
* Sprint system with FOV change
* Mouse look with pitch clamp and tilt

---

### Interaction System

* SphereCast-based detection
* Press **E** to interact
* UI prompt system
* Modular interactable system

---

### Gameplay Systems

* Memory fragments that display text and store codes
* Code-locked door system
* Keypad-style input panel
* Modular door motion (hinge-style rotation)

---

### UI

* Center reticle that reacts to interactables
* Interaction prompt (object name + action)
* Memory fragment panel (freezes player)
* Code input panel

---

### Environment (Prototype)

* Blockout-based level design
* Cave environment using asset meshes
* Basic asset lighting

---

## Project Goals

The goal of Depth Protocol is to create a **focused exploration experience** built around:

* Environmental storytelling
* Atmospheric design
* Player-driven discovery

---

## Current Status

This project is in an **early vertical slice prototype**.

Focus so far:

* Core systems
* First playable level
* Basic atmosphere

Next steps:

* Sound design and ambient audio
* Expanded level design
* Improved visual identity
* Gameplay depth (more interactions, puzzles, progression)

---

## Tech Stack

* Unity (C#)
* ProBuilder (level blockout)
* Unity Asset Store (environment assets)

---

## Author

Izaiah Razon
