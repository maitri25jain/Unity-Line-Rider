# 2D Line Rider Sandbox

## Overview
A 2D physics sandbox where the player can draw dynamic lines in real-time for a character to ride on. This project demonstrates intermediate 2D game development techniques, focusing on translating screen-space input into dynamic physical objects within the game world.

[Watch Gameplay Video Here](https://drive.google.com/drive/folders/1NN20FKC3eCqP9zH_6rlUDEKUXEcpsTO-?usp=drive_link)

## Technical Features
* **Dynamic Terrain Generation:** Engineered a drawing system using Unity's `LineRenderer`, mapping mouse input from screen coordinates to 2D world coordinates.
* **Procedural Physics:** Implemented dynamic `EdgeCollider2D` components that update their vertices in real-time to match the user-drawn lines, allowing the player's Rigidbody to interact with custom terrain.
* **Custom Camera Controller:** Built a robust 2D camera system supporting multi-directional panning and orthographic zooming.
* **Input Management:** Scripted an input manager to seamlessly handle transitioning between drawing, erasing, and camera manipulation states.

## Technologies Used
* **Engine:** Unity (2D)
* **Language:** C#
