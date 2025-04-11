# VR Basketball Shooter

A virtual reality basketball shooting experience built using Unity for the Meta Quest 2. This project focuses on realistic physics-based throwing, scoring mechanics, and immersive feedback systems. Developed as part of a final year project.

## Features

- Physics-based basketball throwing with hand-tracked velocity
- Score detection with field goal percentage tracking
- In-game UI with score, timer, and menu

## Technologies Used

- Unity 2022.x
- XR Interaction Toolkit
- OpenXR 
- Blender

## Installation and Setup

> Required: Unity 2022.x.xf1, Meta Quest 2, ADB

1. Clone the repository:

git clone https://github.com/yourusername/vr-basketball-shooter.git

2. Open the project using Unity Hub.

3. Enable XR Plugin Management (Project Settings > XR Plug-in Management) and select OpenXR for Android.

4. Build Settings:
   - Platform: Android
   - Device: Meta Quest 2
   - Run Device: Your Quest 2 (connected via USB or Wi-Fi)

5. Build & Run to the headset, or build an APK and install manually using ADB:

adb install -r path-to-apk.apk


## Gameplay Overview

- Player enters the basketball court environment
- Menu allows selection of game mode or reset
- Ball is spawned into hand or court
- Player shoots using natural throwing motion
- Score updates and stats are tracked in-game
- Game ends when timer runs out or menu is used

## Assets

- Court, hoop, and ball models: Unity Asset Store and Sketchfab
- Custom rim created in Blender for more realistic bounce behavior
- Audio from open license sources such as freesound.org and OpenGameArt

## License

This project is intended for educational use only. All third-party assets are either custom-created or sourced from open license repositories.

## Author

Created by Pío Cañas Toimil
