# Endless Car Runner Game

Endless Car Runner Game is a three-lane endless runner built with Unity. The car moves forward through dynamically generated road sections while the player changes lanes, jumps over obstacles, slides under barriers, and collects coins.

## Features

- Three-lane movement with swipe-based controls
- Touch input on mobile and mouse input in the Unity Editor, standalone builds, and WebGL
- Jump and slide actions with matching animations and sound effects
- Randomized road-tile spawning with automatic cleanup behind the player
- Coin collection and an in-game coin counter
- Distance-based scoring that does not decrease
- Game-over UI with replay and return-to-menu actions
- Background music and gameplay sound management
- Curved-world rendering shader

## Controls

| Action | Input |
| --- | --- |
| Start the run | Tap or click |
| Change lane | Swipe or drag left/right |
| Jump | Swipe or drag up |
| Slide | Swipe or drag down |

## Requirements

- Unity `6000.3.8f1`
- Universal Render Pipeline `17.3.0`

The exact editor version is recorded in `ProjectSettings/ProjectVersion.txt`. Unity package versions are locked in `Packages/packages-lock.json`.

## Getting Started

1. Clone the repository.
2. Open Unity Hub and choose **Add project from disk**.
3. Select the repository root.
4. Open the project with Unity `6000.3.8f1` and allow Unity to restore packages and regenerate local cache files.
5. Open `Assets/Scenes/Menu.unity` and enter Play mode.

## Scenes

The enabled build scenes are:

1. `Menu`
2. `Level01`

## Project Structure

```text
Assets/
  Animations/       Car and UI animations
  Materials/        Gameplay materials
  Models/           Car, road, and obstacle models
  Prefebs/          Road tiles, coins, and audio prefabs
  Scenes/           Menu and gameplay scenes
  Scripts/          Gameplay, input, UI, audio, and spawning logic
  Settings/         Universal Render Pipeline settings
  Shaders/          Curved-world shader files
  Sounds/           Music and sound effects
  UI/               User-interface textures
Packages/           Unity package manifest and lock file
ProjectSettings/    Shared Unity project configuration
```

Unity-generated caches, local editor preferences, IDE files, build outputs, and signing material are intentionally excluded from version control. Unity `.meta` files remain tracked because they preserve asset GUIDs and references.

## Notes

- The current application version is `0.1.0`.
- Generated folders such as `Library`, `Temp`, `Obj`, `Logs`, and `UserSettings` do not need to be copied; Unity recreates them locally.
- Before distributing the game, verify that every included audio and visual asset has an appropriate license for the intended use.
