# Unity Stack Collection Game

A mobile-friendly 3D collection game built with Unity, featuring dynamic stacking mechanics, bonus systems, and engaging gameplay loops.

***Note:** This project is only an internship project prepared in a few days. It is not a completed game. It may contain errors and performance issues.*

## Overview

This project is a color-matching stacking game where players collect objects of the same color, stack them vertically, and navigate through various gameplay modes including a special "Fewer Mode" and bonus calculation sequences.

## Key Features

### Core Gameplay
- **Dynamic Stacking System**: Collect objects of matching colors to build vertical stacks
- **Color Matching Mechanics**: First collected object determines the target color
- **Progressive Speed Increase**: Movement speed increases with successful collections
- **Penalty System**: Wrong color collections result in losing the top stacked item

### Advanced Systems
- **Event-Driven Architecture**: Centralized event management for loose coupling
- **Dependency Injection**: Uses Zenject for clean dependency management
- **Object Following System**: Smooth item following with sway physics
- **Camera Control**: Cinemachine integration for dynamic camera movements

## Technical Architecture

### Design Patterns Used
- **Singleton Pattern**: GameManager, SceneManager, FewerModeManager
- **Observer Pattern**: Event-driven communication via EventManager
- **Dependency Injection**: Zenject container for component binding
- **State Pattern**: GameState enum for managing different game phases

### Core Components

#### Managers
- **GameManager**: Central game state management and score tracking
- **EventManager**: Static event hub for decoupled communication
- **FewerModeManager**: Special mode activation and timing
- **SceneManager**: Scene loading and transition management

#### Player Systems
- **PlayerInputController**: New Input System integration for touch and keyboard input
- **PlayerMovementController**: Physics-based movement with boundary constraints
- **PlayerInteractionManager**: Collision detection and collection logic
- **PlayerBonusController**: Bonus accumulation and decay system
- **PlayerAnimationController**: Character animation state management

#### Collectible System
- **ICollectible Interface**: Contract for collectible objects
- **Collectible Component**: Individual item behavior with follow mechanics
- **CollectibleType Enum**: Different types (Color, ColorChanger, Gold, etc.)
- **ColorType System**: Red, Green, Blue, Yellow color categorization

#### UI System
- **Modular UI Components**: Score, Gold, Bonus, Fewer Mode displays
- **Win/Lose Screens**: DOTween animations for smooth transitions
- **Pause System**: Game state management with overlay UI

## Dependencies

### Unity Packages
- **Unity Input System**: Modern input handling
- **Cinemachine**: Camera control and management
- **DOTween**: Smooth UI animations and transitions
- **Zenject**: Dependency injection framework

### Technical Requirements
- Unity 2022.3+ LTS recommended
- .NET Standard 2.1
- Mobile platform optimization

## Installation & Setup

1. Clone the repository
2. Open in Unity 2022.3+ LTS
3. Install required packages via Package Manager:
   - Input System
   - Cinemachine
   - DOTween (via Asset Store)
   - Zenject (via Asset Store)
4. Configure build settings for target platform
5. Set up input action assets for desired control scheme

---
