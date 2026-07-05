> ### Okay, **listen**... 
> Yes, the 'README' was written by AI but just check the project out, it really is cool. It's highly scalable and highly configurable. Has like cool systems in it. I am particularly in love with the StateMachine. The stuff that are not exactly 'base' are highly modular. And although the 'README' smells AI, it actually summarized the project pretty good ^^

# Base 2D Unity Platformer
A production-ready, highly scalable, and decoupled 2D platformer template for Unity. Serving as a robust foundation for advanced games, it leverages modern design patterns and best practices to ensure clean separation of concerns. A custom node-based State Machine and Event-Driven architecture eliminate rigid inheritance and tightly coupled logic. The framework features a capable persistence system, custom Object Pooling to prevent GC spikes, Unity's new Input System, and embraces ScriptableObjects

## Key Highlights
### 1. Highly Modular & Decoupled Architecture
The architecture is designed with a strict separation of concerns, heavily relying on **Interface-Driven Design** (`IPersistable`, `IState`, `IMover`, `IPoolable`, `IInteractable`). This allows for individual subsystems to operate independently, making the codebase highly testable and extensible.

### 2. Event-Driven Communication
To maintain loose coupling, the project utilizes an extensive event-based communication system. Static C# `Action` and `Func` events (such as `PoolingEvents`, `PlayerEvents`, and `UIEvents`) ensure that systems can broadcast and listen to state changes without hard dependencies.

### 3. Advanced State Machine System
Character behaviors and game states are governed by a powerful, generic `StateMachine` graph. 
* **State Nodes**: Encapsulate specific logic for different states.
* **Transitions**: Driven by `ITransition` interfaces and evaluated through `IPredicate` logic, ensuring clean and deterministic state flow.

### 4. Robust Persistence System
The save/load system is highly flexible, multi-threaded, and centered around the `PersistenceManager`.
* **IPersistable**: Components implement this interface to easily hook into the save system.
* **Scene vs. Global Persistence**: Cleanly differentiates between **scene-based** data and **global** data, ensuring seamless loading across level transitions.
* **Serialization**: Utilizes a custom `SerializableDictionary` (wrapper for dual lists) to handle complex data structures, serialized cleanly via `JsonUtility` (e.g., `PlayerSaveData`).
* **Multi-Threaded Operations**: File I/O operations for saving and loading run asynchronously on separate threads, preventing frame drops during autosaves.

### 5. ScriptableObject-Driven Configuration
`ScriptableObject`s are used extensively throughout the project not just for static configuration data, but also as a core part of the architecture, further promoting decoupling and making it incredibly easy for designers to tweak parameters without touching code.

### 6. Centralized Object Pooling
Performance is prioritized with a highly optimized object pooling system. An `Instantiator` manages pools based on `Pooler` component group names, minimizing garbage collection spikes and ensuring smooth runtime performance during heavy instantiation events.

### 7. State-Machine-Driven UI
The user interface is treated as a first-class citizen. A state-machine-driven `UIManager` seamlessly manages `Panel` views, cleanly toggling `CanvasGroup` properties and handling transition states.

### 8. Editor Tooling & Best Practices
The project utilizes `UNITY_EDITOR` preprocessor directives to create custom development-only tools, such as `BoxGizmos` for visualizing colliders and `PersisterEditor` for debugging persistence logic directly in the Unity Editor, ensuring the production build remains lightweight.

### 9. Advanced Audio System
Features a dedicated, Singleton-based audio manager that utilizes scriptable data containers for sound definitions. It fully supports dynamic crossfading between music tracks and manages spatial/impact sound effects effortlessly with proper logarithmic volume scaling.

### 10. Runtime Input Rebinding
Leveraging Unity's new Input System, the project includes a complete `RebindManager`. This allows for seamless, JSON-backed runtime key rebinding that automatically saves and loads player control preferences.

### 11. Modular Environment Mechanics
Level design is supported by highly modular movement and hazard components. Interfaces like `IMover` and `IVelocityOffsettable` drive moving platforms that correctly inherit velocity to the player, while abstract hazard classes provide a clean foundation for traps and obstacles. Components like `Rotater` are fully featured, capable of syncing states across scenes, utilizing Rigidbodies, and maintaining persistence effortlessly.

### 12. Asynchronous Scene Management
Scene transitions are fully decoupled and asynchronous, seamlessly integrating with UI states to display optimized and responsive loading screens without blocking the main Unity thread.

### 13. Built-in Diagnostics Tools
Equipped with runtime diagnostic utilities, such as a built-in Lag Detector, allowing developers to easily monitor frame hitches and optimize performance profiles dynamically.

## Practical Positive Aspects
* **Developer Experience**: The clean architecture and clear separation of concerns make onboarding and expanding the codebase a breeze.
* **Performance**: Built-in object pooling and efficient state management ensure high frame rates even on lower-end devices.
* **Designer Friendly**: Heavy use of ScriptableObjects empowers level designers and game balancers to make meaningful changes without requiring a programmer.
* **Production Ready**: From the robust persistence system to the editor-only debugging tools, this base is built to ship.

## Attributions
* Sprites:
    * https://kenney.nl/assets/platformer-pack-remastered
* SFX
    * https://kenney.nl/assets/impact-sounds
* Music
    * https://opengameart.org/content/next-to-you
    * https://opengameart.org/content/happy-adventure-loop