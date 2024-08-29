# BulletHell

![Projectile Emitter Demonstration](READMEAssets\ProjectileEmitterDemostration.gif)

This Unity project was my first foray into game development and C# programming, created back in 2018 as I aspired to join the I.T. University of Copenhagen. 

## Concept
The project started as an experiment to build a modular projectile system that could be integrated into various games to create challenging and dynamic projectile-based gameplay. The goal was to design a flexible system that allowed for the customization of bullet trajectories and behaviors, which could handle a high volume of projectiles for an engaging bullet hell experience. A playable test scene was later added to showcase the project at my university.

## Features

- **Runtime Configuration**: Easily adjust projectile behaviors and patterns during gameplay.
- **High Performance**: Capable of managing more than 2000 projectiles simultaneously, ensuring smooth performance even under heavy loads.
- **State Machine Integration**: Utilizes a state machine for clear, modular, and reusable code, often seen in AI character controllers.
- **ScriptableObjects**: Reduces memory usage and simplifies projectile management by referencing a central data source rather than duplicating data across instances. This approach makes it easy to reuse and modify projectile setups.


## Potential Enhancements

- **Unity DOTS Integration**: Refactoring the project to leverage Unity's Data-Oriented Technology Stack (DOTS) could significantly improve performance and scalability.
- **Advanced Configuration Interface**: An overhaul of the configuration interface could provide a more intuitive and detailed control system for developers.

## Test Scene

A simple test scene is included where the player character must navigate past an enemy block to reach the goal. The projectile pattern of the enemy can be modified through the associated ScriptableObjects (`ScriptableObjects` -> `Projectile Units` -> `Projectile_2`), demonstrating the module's flexibility.