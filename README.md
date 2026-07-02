> [!NOTE]
> ### Okay, **listen**... 
> Yes, the 'README' was written by AI but just check the project out, it really is cool. It's highly scalable and highly configurable. Has like cool systems in it. I am particularly in love with the StateMachine. The stuff that are not exactly 'base' are highly modular. And although the 'README' smells AI, it actually summarized the project pretty good.

# Base 2D Unity Platformer

A highly scalable, decoupled, and performance-driven 2D platformer template for Unity. Built around a custom node-based State Machine and an Event-Driven architecture, this foundation eliminates rigid inheritance chains and tightly coupled logic. It leverages custom Object Pooling to prevent runtime garbage collection spikes, utilizes Unity's new Input System, and fully embraces ScriptableObjects for a data-driven configuration workflow.

> [!NOTE]
> ### Save locations 
> Save files will be located at following directories at default:
> Linux:
> /home/[YourUsername]/.config/unity3d/PSEMOSoftware/Base2DUnityPlatformer/
> Windows:
> C:\Users\\[YourUsername]\AppData\LocalLow\PSEMOSoftware\Base2DUnityPlatformer\

## Core Features & Architecture

### **State Machine Driven**
The project relies on a flexible, custom-built State Machine architecture (using Nodes and Predicate-based Transitions). This is utilized heavily in two main areas:
- **Player Logic**: Handles complex state transitions seamlessly (Idle, Run, Jump, Fall, Dash).
- **UI Management**: Governs screen flows, ensuring accurate transitions between menus, settings, and gameplay states.

### **Event-Driven & Decoupled**
- **Centralized Events Hub**: A static `Events.cs` script delegates major game occurrences (Player Death, Checkpoints, Collectibles) via C# Actions, meaning systems don't require direct hard-coded references to one another.
- **Custom Object Pooling**: Built-in object pooling manager (`Instantiator`) and `IPoolable` interface to prevent expensive `Instantiate` and `Destroy` operations during gameplay, used heavily for projectiles, hazards, and audio sources.

### **Data-Driven Configuration**
Game configurations are cleanly centralized into `ScriptableObjects`, making it trivial for designers to tweak parameters without touching code:
- **PlayerSO**: Movement speed, jump counts, forces, coyote time, and jump buffering.
- **CameraSO**: Follow speeds, smoothing, and level boundaries.
- **AudioSO & AllAudioSOs**: Individual track properties (looping, base volumes) and group lists.
- **CollectibleSO**: Display name and metadata for pick-ups.
- **UISO**: General UI parameters.

---

## Advanced Player Controller
The `PlayerController` uses Unity's **New Input System** alongside `Rigidbody2D` physics to deliver a tight and highly responsive platforming feel.
- **Modern Platforming Quality of Life**: Features essential mechanics like **Coyote Time** (allowing jumps slightly after walking off a ledge) and **Jump Buffering** (queueing jumps right before hitting the ground).
- **Dynamic Jumping**: Supports multi-jumps (e.g., double jumps) and variable jump heights (releasing the jump button early cuts vertical momentum to a given percentage).
- **Dash Mechanic**: Suspends gravity temporarily to shoot the player forward.

---

## Environment & Interactions
The `Environment` scripts offer modular, drag-and-drop solutions for level design:
- **Velocity Inheritance**: Interfaces like `IVelocityOffsettable` and `IMover` allow the player to seamlessly ride moving platforms, conveyor belts, or follow paths (`PathFollower`) while retaining their own local movement.
- **Hazards & Checkpoints**: Ready-to-use `KillBox` scripts for spikes/pits, `CheckPoint` systems for respawning, and `Faller` objects for dropping hazards.
- **Interactive Elements**: `IInteractable` allows NPCs, switches, or signs to be triggered by the player. Dynamic enablers (`EnableOnContact`, `EnableOnInteract`, `EnableWhileContact`) allow you to easily create doors, pressure plates, and levers.
- **Spawners**: Periodic generation of objects leveraging the built-in pooling system.

---

## Camera System
- **Multi-Target Follow**: The `CameraManager` can calculate the weighted average position of multiple targets to dynamically frame scenes.
- **Smooth Dampening**: Employs `Vector3.SmoothDamp` for elegant trailing.
- **Level Bounds**: Hard clamp support to ensure the camera never pans outside of the designated level boundaries.

---

## Audio Management
- **Dedicated Audio Manager**: Singleton that segregates background music and sound effects.
- **Dynamic SFX Pooling**: Automatically generates and cycles through a pool of `AudioSource` components, preventing cut-offs when multiple SFX play simultaneously.
- **Volume Settings**: Separate sliders for Master, Music, and SFX volumes, properly calculated and permanently saved via `PlayerPrefs`.

---

## UI & Menus
- **State Machine Flow**: The `UIManager` switches actively rendered `CanvasGroup` panels (Main Menu, Settings, Gameplay UI).
- **Pause & Countdown System**: Leverages `Time.timeScale` to freeze the game, and gracefully reintroduces gameplay using unscaled time (`UnpauseCountdownUI`) to count down before restoring control.
- **Collectible Tracking**: Updates the UI efficiently in response to the static event hub whenever items are picked up, categorizing them by group.

---

## Editor & Gizmos
- Built-in Gizmo scripts (`BoxGizmos`, `CircleGizmos`) to visualize invisible level geometry like spawn points, triggers, and collider bounds directly in the Unity Scene view with custom colors and labels.

---

## Attributions

* Sprites:
    * https://kenney.nl/assets/platformer-pack-remastered
* SFX
    * https://kenney.nl/assets/impact-sounds
* Music
    * https://opengameart.org/content/next-to-you
    * https://opengameart.org/content/happy-adventure-loop