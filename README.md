> [!NOTE]
> ### Okay, **listen**... 
> Yes, the 'README' was written by AI but just check the project out, it really is cool. It's highly scalable and highly configurable. Has like cool systems in it. I am particularly in love with the StateMachine. The stuff that are not exactly 'base' are highly modular. And although the 'README' smells AI, it actually summarized the project pretty good.

# Base 2D Unity Platformer

A highly scalable, decoupled, and performance-driven 2D platformer template for Unity. Built around a custom node-based State Machine and an Event-Driven architecture, this foundation eliminates rigid inheritance chains and tightly coupled logic. It leverages custom Object Pooling to prevent runtime garbage collection spikes, utilizes Unity's new Input System, and fully embraces ScriptableObjects for a data-driven configuration workflow.

## Features

### 1. Advanced Player Controller
The player controller includes modern, "feel-good" platforming mechanics right out of the box:
- **Coyote Time & Jump Buffering**: Ensures inputs feel responsive and forgiving.
- **Variable Jump Height**: Jump height scales dynamically based on how long the jump button is held.
- **Fluid Movement States**: Built-in support for running, falling, and dashing.
- **Moving Platforms Compatibility**: Full support for inheriting external velocities (e.g., from moving platforms) via the `IVelocityOffsettable` interface.

### 2. Node-Based State Machine
Say goodbye to spaghetti code! The player logic is driven by a highly modular **State Machine** (`StateMachine`, `StateNode`, `ITransition`).
- Easily add new states (like Wall Slide, Attack, Crouch) by simply creating a new class and defining its transitions via functional predicates.
- Enforces single-responsibility principle for character behaviors.

### 3. High Performance Object Pooling
Instantiation can cause severe frame drops. This project includes a custom-built **Object Pooling System** (`Instantiator`, `IPoolable`).
- Automatically handles recycling GameObjects.
- Re-initializes objects seamlessly using the `ResetObject()` interface method.

### 4. Data-Driven Design (Scriptable Objects)
Code and configuration are strictly separated to make the project highly designer-friendly:
- **PlayerSO**: Tweak speed, jump forces, layers, and input tolerances instantly without recompiling.
- **CameraSO**: Adjust camera smoothing, max speeds, and clamping bounds.
- **AudioSO**: Organize music and SFX easily in the inspector.

### 5. Decoupled Architecture & Events
The architecture relies on an **Event-Driven approach** (`Events.cs`). 
- Audio, UI, and Game State changes are entirely decoupled from gameplay logic.
- A central `GameManager` and `UIManager` listen to events (like Player Death or Checkpoint Reached) to trigger menus, screen fades, and logic resets smoothly.

### 6. Dynamic Camera System
The `CameraManager` doesn't just blindly follow a target—it supports **weighted multi-target tracking** allowing the camera to pan dynamically between the player and important points of interest, complete with smooth dampening and defined boundaries.

## Getting Started
1. Open the project in Unity.
2. Open the main scene located in `Assets/Scenes`.
3. Check the `PlayerSO` in the `Assets/SOs` folder to tune the player controller to your liking.
4. Press Play and enjoy the smooth movement!

## Project Structure
- **Core**: Contains the foundational systems (State Machine, Object Pooling, Game Manager, Predicates).
- **Player**: The player controller and configuration.
- **Audio**: Audio Manager and Scriptable Object definitions for sound effects and music.
- **Camera**: Multi-target camera follow script and bounds settings.
- **Environment**: Modular scripts for interactables, moving platforms, checkpoints, and hazard zones.
- **UI**: State-driven UI manager and fading canvas panels.
