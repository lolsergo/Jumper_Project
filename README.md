# <Game Title>

An endless 2D runner with progressive global speed, reactive jump physics, procedural obstacle/pickup spawning, player profiles, currency & health systems, rewarded revive, and fully data‑driven configuration via ScriptableObjects + Zenject DI.

## Table of Contents
- [Overview](#overview)
- [Core Features](#core-features)
- [Gameplay Loop](#gameplay-loop)
- [Controls](#controls)
- [Player Systems](#player-systems)
- [Level & World Systems](#level--world-systems)
- [Game State Machine](#game-state-machine)
- [Audio System](#audio-system)
- [Input System & Rebinding](#input-system--rebinding)
- [Profiles, Saves & Economy](#profiles-saves--economy)
- [Ads & Monetization](#ads--monetization)
- [Architecture & Dependency Injection](#architecture--dependency-injection)
- [Project Structure (Key Folders)](#project-structure-key-folders)
- [ScriptableObjects & Config](#scriptableobjects--config)
- [Extending the Game](#extending-the-game)
- [Development & Tech Stack](#development--tech-stack)
- [Build & Run](#build--run)
- [Troubleshooting](#troubleshooting)
- [Roadmap](#roadmap)
- [Contributing](#contributing)
- [License](#license)

## Overview
You control a continuously moving character. Global speed increases over time, raising difficulty. Avoid obstacles, collect coins and hearts, preserve health, and extend runs via rewarded revive. Multiple user profiles and persistent progression are supported.

## Core Features
- Progressive speed scaling (GameSpeedManager + distance tracking via UniRx ReactiveProperty)
- Variable jump height (JumpSystem + GravitySystem + GravityConfig)
- Adaptive gravity: normal, jump, fast-fall, float-fall clamped descent
- Procedural obstacle & pickup spawning with pooled objects
- Player health & currency systems (hearts & coins)
- Rewarded ad revive flow
- Multi-profile save system (UserProfileService, SaveSystem)
- Input abstraction with runtime rebinding & multiple schemes
- Central game state machine (Gameplay, Pause, Game Over, Post-Lose Choice)
- Audio pooling (SFX + Music) with configurable library & category settings
- Scene-level DI installers (Bootstrap, Level, Menu, Profile, Ads, Audio)
- Clean separation of data (ScriptableObjects) vs logic (services/components)

## Gameplay Loop
1. Start in Menu -> choose or create profile.
2. Enter Game scene: speed begins at base, increases at timed intervals.
3. Player jumps to avoid obstacles and collect pickups.
4. Collisions reduce health; hearts restore, coins increase currency.
5. On death: optional rewarded revive -> continue or game over.
6. Distance and resources persist back to profile save.

## Controls
(Default; replace if different)
- Jump: Keyboard Space / Gamepad South (A)
- Pause: Esc / Start
- (Rebind supported through InputRebindService UI layer)
Touch & mobile adaptation can map jump to a tap (future extension).

## Player Systems
- PlayerController: orchestrates input caching, ground detection, jump triggering, gravity selection.
- JumpSystem: time-based force ramp from MinJumpForce to MaxJumpForce over MaxJumpTime.
- GravitySystem: sets gravityScale and enforces float-fall vertical speed cap.
- GroundChecker: raycast below player for grounded & near-ground states (animation anticipation).
- PlayerHealth / PlayerHealthService: track HP, handle damage & healing (hearts), death events raising state transitions.
- PlayerCurrency: accumulates coins; interfaces with PriceService for cost evaluation (shop/economy extensions).

## Level & World Systems
- LevelObject, Obstacle, Coin, HealthHeart: pooled runtime entities.
- LevelObjectPool & PoolCleanupHandler: memory-friendly spawning & cleanup.
- LevelObjectGenerator: spawns obstacles/pickups based on generation rules (extendable).
- SpawnPoint markers define spatial origin(s).
- BackgroundScroller & FloorScroller: parallax / looping environment synced to GameSpeed.
- GameSpeedManager: 
  - Tracks GameSpeed (base + incremental increases using increaseGameSpeedMultiplier)
  - Emits CurrentDistance via ReactiveProperty<float>
  - Provides Increase / Decrease / Reset logic

## Game State Machine
Classes: GameState (base), GameplayState, PausedState, GameOverState, ChooseOnLoseState.
Responsibilities:
- Encapsulate transitions (e.g., pause/resume)
- Bind/unbind input layers (pause handling)
- Invoke UI and ad revive flows
Extend by adding new state deriving from GameState and registering in state machine initialization.

## Audio System
Components:
- AudioManager: high-level API for playing SFX & music
- AudioSourcePool / AudioPool / AudioPoolRegistry / AudioSourcePool: pooled AudioSources to avoid instantiation spikes
- AudioLibrary: catalog of clips keyed by enum/string
- GameAudioSettings: global volume categories (Music, SFX) and persistence
- MusicManager: handles background track loop / switching
- PlayerAnimAudio, ButtonSound: event-driven contextual sounds
Usage:
1. Register clip groups in AudioLibrary.
2. Request playback through AudioManager (pool chooses/returns source).
3. Adjust volumes via settings service (persisted in SaveData).

## Input System & Rebinding
- InputController: wraps Unity Input System actions -> ActionEvent -> InputActionState
- InputSchemeManager & SceneInputSchemeHandler: load appropriate control scheme per scene
- InputRebindService (IInputRebindService): runtime remap & persistence
- PauseInputHandler: isolates pause toggling independent of gameplay actions
Adding a new action:
1. Add to enum InputController.InputActionType
2. Expose in inspector list (avoid duplicate types, OnValidate checks)
3. Bind logic in consumer (e.g., PlayerController or new system)
4. Add UI rebind element referencing action reference

## Profiles, Saves & Economy
- SaveSystem: low-level serialization (likely JSON / PlayerPrefs)
- SaveData: aggregate persistent info (profiles, settings, progress)
- UserProfileService: manages active profile, switching, creation
- PriceConfigSO + PriceService: centralized price lookup & future scaling logic
- IMoneyService abstraction for currency handling (loosely coupled to UI/purchase flows)

## Ads & Monetization
- AdMobService: initialization & rewarded ad request/loading
- RewardedReviveController: consume rewarded ad to trigger revive (bypass GameOver once)
- AdsInitializer / AdsInstaller: DI setup & environment-specific initialization
Flow:
1. On player death -> ChooseOnLoseState prompts revive
2. If ad watched successfully -> restore / partial restore health & resume gameplay
3. Otherwise -> GameOverState

## Architecture & Dependency Injection
Zenject installers:
- BootstrapInstaller: root composition (global singletons)
- LevelInstaller, MenuInstaller, ProfileSceneInstaller: scene-scope bindings
- AudioInstaller, AdsInstaller, InputRebindInstaller: feature-specific
- SceneInjectionHandler + SceneInjectionConfig: ensure correct installers run on load
Benefits:
- Loose coupling
- Testability (services can be mocked)
- Controlled scene transitions (SceneLoader + enum SceneType { Menu, Profiles, Game })

## Project Structure (Key Folders)
- Assets/Scripts/Infrastructure (Installers, initializers)
- Assets/Scripts/Managers (GameManager, MenuManager, Speed, Settings)
- Assets/Scripts/GameStateMachine (state pattern)
- Assets/Scripts/Player (controller, gravity, jump, stats)
- Assets/Scripts/Level Objects (obstacles, pickups, pooling, generation)
- Assets/Scripts/Audio System (Runtime, Library, Pools, Action sounds, Installer)
- Assets/Scripts/Input System (controller, rebind, schemes)
- Assets/Scripts/Scene Helpers (SceneLoader, injection)
- Assets/Scripts/Level Settings (GravityConfig, PriceConfigSO)
- Assets/Scripts/Save System (SaveData, SaveSystem, UserProfileService)
- Assets/Plugins/UniRx (Reactive extensions)

## ScriptableObjects & Config
- GravityConfig: Min/MaxJumpForce, MaxJumpTime, gravity scales, FloatFallSpeed
- PriceConfigSO: central pricing (extend for shop scaling)
- AudioLibrary: clip sets
Add new config: create SO, reference through installer or manager, inject where needed.

## Extending the Game
Add a new obstacle:
1. Derive from LevelObject or Obstacle
2. Create LevelObjectData for spawn weights/settings
3. Register in LevelObjectGenerator list/pool
4. Provide collision handling (damage, currency, etc.)

Add a new pickup type:
1. Derive from Collectibles or similar base
2. Implement OnCollect -> modify service (currency, health, buff)
3. Add prefab to pool & generator config

Add a new game state:
1. Create class : GameState
2. Inject dependencies (UI, services)
3. Register in GameStateMachine initialization map
4. Trigger transitions from existing states

Add a new input action:
1. Update InputAction asset (Unity Input System)
2. Add enum entry & inspector reference
3. Consume via InputController.GetAction / GetState

## Build & Run
1. Clone: git clone <repo-url>
2. Open in Unity <version>. Allow packages to restore (Input System, Zenject, UniRx, Ads SDK).
3. Open Menu scene (SceneType.Menu) and press Play.
4. Create/select profile -> start run -> test obstacles & pickups.
5. For mobile builds: configure AdMob IDs in AdMobService or project settings.
6. For desktop: ensure placeholder ad logic (if any) does not block flow.

## Troubleshooting
No jump variation:
- Check GravityConfig Min/MaxJumpForce & MaxJumpTime
- Ensure Input action (Jump) uses Press+Hold or proper interaction (not Tap only)

Player not running animation:
- Verify GameSpeed reaches _startRunSpeed threshold
- Animator parameters (IsRunning, VerticalSpeed, IsGroundNear) present

Revive not offered:
- Confirm RewardedReviveController bound & AdMobService initialized
- Check state transition goes through ChooseOnLoseState

Audio not playing:
- AudioLibrary entries assigned?
- AudioPool capacity sufficient?
- Global mute/volume in GameAudioSettings?

Input rebinding not persisting:
- Ensure SaveSystem writes after rebind
- Confirm InputRebindService applied mappings on scene load

Distance not increasing:
- GameSpeedManager active in scene
- Time.deltaTime not paused (e.g., check timescale adjustments in pause state)

## Roadmap
- Advanced difficulty curves (distance-based spawn weights)
- Additional pickup types (shield, magnet)
- Cosmetic shop using PriceService
- Daily challenges & streak bonuses
- Cloud sync for profiles
- Analytics events (distance milestones, revive usage)
- Localization (UI & tutorial)

## Acknowledgments
- Zenject for DI pattern inspiration
- UniRx for reactive distance handling
- Unity Input System team
- AdMob for rewarded ads infrastructure

## Quick Reference (Animator Parameters)
- IsGroundNear (bool)
- IsRunning (bool) – threshold-based
- VerticalSpeed (float)

## Key Data Flow
Input -> InputController (ActionState) -> PlayerController -> Jump/Gravity -> Rigidbody2D  
GameSpeedManager -> Distance (ReactiveProperty) -> UI & difficulty scaling  
Collisions -> PlayerHealthService / PlayerCurrency -> SaveSystem (on session end)  
Death -> StateMachine (ChooseOnLoseState -> Reward flow) -> GameOverState 
