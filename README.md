<h1 align="center">2D Platformer Study</h1>

<p align="center">
  <img src="https://img.shields.io/badge/Unity-2021.3.5f1-blue.svg" />
</p>

## About

This project is a 2D platformer game developed using Unity. It includes game development and architecture concepts such as Singleton and Listening design patterns. It also utilizes code architecture that uses C# delegate functions to centralize commands such as jumping, life updates, and more. The game includes mechanics such as double jumping, gliding, wall jumping, moving platforms, and unstable platforms that break under pressure. The project also have a Parallax Effect using layers of background sprite and interaction features such as dialog.

<p align="center">
  <img src="https://live.staticflickr.com/65535/52734593959_2e78b2c599_b.jpg" style="width: 80%" />
</p>

## Getting Started

1. Clone the repository
2. Open the project in Unity
3. Play the game in the Unity editor or build the game for your desired platform

## Usage

Turn on the mechanics following at Hierarchy the object Singletons/GameProgress:

- Double jumping - Boolean
- Wall jumping - Boolean
- Gliding - Boolean

## Code Architecture

The code architecture uses C# delegate functions to centralize commands such as jumping, life updates, and more. It also uses design patterns such as Singleton and Listening to efficiently manage game objects and events.
