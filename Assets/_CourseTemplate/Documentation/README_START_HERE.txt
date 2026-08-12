PHYSICAL COMPUTING – MASTER TEMPLATE README

## OVERVIEW ##

This template serves as the foundation for motion-controlled and physical computing game projects. The framework is designed to support rapid prototyping, gameplay experimentation, and integration with OptiTrack Motive motion capture systems and reactive physical props.
The template separates gameplay systems from hardware/input systems. Students should build and test gameplay using the provided DebugKeyboard mode before integrating motion capture or reactive prop systems.
This project is intentionally simple and modular. The goal is to create a stable interaction framework that can be expanded with additional gameplay mechanics, motion tracking systems, sensors, Arduino systems, and physical computing components.

The current template supports three input modes:
   1 DebugKeyboard
   2 MocapPlaceholder
   3 MocapLive

This allows the project to be tested without mocap hardware, while still being ready for live OptiTrack integration.

--------------------------

## PROJECT STRUCTURE ##

--- SYSTEMS ---
Contains global systems and managers.

   Main Camera
   Directional Light
   EventSystem
   GameManager
   InputManager
   UIManager

--- GAMEPLAY ---
Contains gameplay objects and interaction systems.

   Floor
   TrackedProp_Debug
   MocapTarget
   ScoreZone
   DangerZone

--- OPTITRACK / MOCAP ---
Contains OptiTrack-related scene objects.

Source_OptiTrackRigidBody is the object intended to receive live tracking data from Motive through the OptiTrack Unity plugin. This object should not contain gameplay logic directly.

--- UI ---
Contains all HUD and interface elements.

--- DEBUG ---
Reserved for future testing and debugging objects.

--------------------------

## CORE SYSTEMS ##

--- TrackedProp_Debug ---
The main gameplay prop used for interaction systems. This object represents the Unity/gameplay version of the physical prop.
This is the object that visibly interacts with gameplay zones. It should be treated as the player-controlled prop within the game world.

--- MocapTarget ---
Invisible tracking anchor object.
Input systems move this object, and the gameplay prop follows it. This allows the template to separate raw input data from the actual gameplay object.

In the current system:
   DebugKeyboard moves MocapTarget
   MocapPlaceholder moves MocapTarget
   MocapLive copies Source_OptiTrackRigidBody to MocapTarget
   TrackedProp_Debug follows MocapTarget

Future input systems should update MocapTarget rather than directly controlling TrackedProp_Debug.

--- Source_OptiTrackRigidBody ---
The raw mocap source object. This object is intended to receive live tracking data from the OptiTrack Unity plugin.
This object should be assigned to the Live Mocap Source field on InputManager.
The gameplay prop should not be directly controlled by this object. Instead, InputManager uses it as a source and passes the movement through MocapTarget.

--- OptiTrackClient ---
The scene object responsible for connecting Unity to Motive through the OptiTrack Unity plugin.
This object should contain the OptiTrack streaming client component.

--- InputManager ---
Handles switching between input providers and applying movement/state information to the tracked prop.

Current responsibilities include:
   Switching input modes with M
   Moving MocapTarget
   Reading keyboard input
   Running placeholder mocap movement
   Reading live mocap source transform
   Saving the last selected input mode
   Passing input data to TrackedPropController
   GameManager
Handles score, timer, reset behavior, and round state.

--- UIManager ---
Updates HUD elements including:
   Score
   Timer
   Prop state
   Input mode
   PropState
Stores the current interaction state of the tracked prop.

--- AudioManager ---
Handles simple sound effect playback for gameplay feedback, input mode changes, and reset events.

--------------------------

## INPUT MODES ##

This template currently supports three input modes.

1. DebugKeyboard
   Simulated keyboard-based testing mode.
   This mode should be used first when designing and testing gameplay mechanics.

   Controls
   WASD = Move Prop
   SPACE = Activate Prop
   R = Reset Scene
   M = Toggle Input Mode

   In this mode, keyboard input moves MocapTarget, and TrackedProp_Debug follows it.

2. MocapPlaceholder
   Simulated motion capture mode.
   This mode uses automated circular movement to mimic incoming tracking data. It is useful for testing motion-driven gameplay without needing the mocap lab.

   In this mode:
   MocapTarget moves automatically in a circle
   TrackedProp_Debug follows MocapTarget
   SPACE still controls prop activation

   This mode should remain in the template as a testing tool.

3. MocapLive
   Live motion capture mode.
   This mode is intended for OptiTrack Motive integration.

   In this mode:
   Source_OptiTrackRigidBody provides live mocap position/rotation
   InputManager copies that data to MocapTarget
   TrackedProp_Debug follows MocapTarget

   If no live mocap source is assigned, the template may fall back to placeholder behavior or display a warning depending on the current script version.

   Live mocap mode requires:
      OptiTrack Unity Plugin
      Motive streaming enabled
      A rigid body created in Motive
      OptiTrackClient configured in Unity
      Source_OptiTrackRigidBody assigned as Live Mocap Source

--------------------------

## GAMEPLAY RULES ##

--- Green Score Zones ---
Entering while the prop is ACTIVE increases score.

--- Red Danger Zones ---
Entering while the prop is ACTIVE decreases score.
Inactive props do not interact with gameplay zones.


The gameplay loop intentionally focuses on:
   State-based interaction
   Physical/reactive systems
   Motion-driven gameplay
   Alternative controller design
   Embodied interaction

--------------------------

## PREFABS ##

Gameplay prefabs are located in: Assets/_CourseTemplate/Prefabs/

Current prefabs include:
   TrackedProp_Debug
   ScoreZone
   DangerZone

Students are encouraged to duplicate and modify prefabs rather than rebuilding systems from scratch.

--------------------------

## IMPORTANT DESIGN RULES ##

--- DO ---
   Build gameplay systems using PropState and TrackedPropController.
   Prototype mechanics in DebugKeyboard mode first.
   Use MocapPlaceholder mode to test motion-based gameplay without hardware.
   Use MocapLive only when OptiTrack/Motive is available.
   Keep gameplay systems modular.
   Organize hierarchy objects cleanly.
   Comment scripts when adding new systems.
   Update MocapTarget rather than directly moving gameplay objects.
   Duplicate the template scene before making major changes.

--- DO NOT ---
   Do not directly tie gameplay systems to hardware-specific input.
   Do not modify the MASTER template scene directly.
   Do not delete manager objects without understanding dependencies.
   Do not hardcode gameplay logic directly into input providers.
   Do not add OptiTrack tracking scripts directly to TrackedProp_Debug unless instructed.
   Do not make Source_OptiTrackRigidBody responsible for scoring or gameplay behavior.

--------------------------

## OPTITRACK INTEGRATION NOTES ##

The intended mocap data flow is:
   Physical Prop with IR LEDs
   → OptiTrack Cameras
   → Motive Rigid Body
   → OptiTrack Unity Plugin
   → Source_OptiTrackRigidBody
   → InputManager
   → MocapTarget
   → TrackedProp_Debug
   → Gameplay Zones

The Source_OptiTrackRigidBody object represents raw tracking data.
The MocapTarget object represents the cleaned/mapped input target used by the game.
The TrackedProp_Debug object represents the gameplay prop that interacts with score zones, danger zones, and other mechanics.
This separation allows students to test gameplay without mocap hardware and integrate live tracking later without rewriting the core gameplay systems.

--------------------------

## FUTURE INTEGRATION ##
This template is designed for future expansion into:
   OptiTrack Motive integration
   IR LED tracking systems
   Arduino/reactive prop systems
   Motion-controlled game mechanics
   Embodied interaction systems
   Physical computing interfaces
   Sensor-based controllers
   Wearable controllers
   Tangible interfaces

--------------------------

## RECOMMENDED WORKFLOW ##
1. Duplicate the MASTER template scene.
2. Prototype gameplay in DebugKeyboard mode.
3. Test interactions and scoring systems.
4. Test motion-based interaction using MocapPlaceholder mode.
5. Build and test physical/reactive prop systems.
6. Integrate motion capture tracking using MocapLive mode.
7. Polish gameplay, feedback, UI, and physical interaction.

--------------------------

## KNOWN NOTES ##
Input mode persists after reset.
The tracked prop changes material based on input mode and active state.
MocapPlaceholder mode currently simulates incoming tracking data.
MocapLive mode expects a live mocap source object.
Future tracking systems should update MocapTarget rather than directly controlling gameplay objects.
TrackedProp_Debug is the gameplay object, not the raw mocap source.
Source_OptiTrackRigidBody is the raw mocap source, not the gameplay prop.

--------------------------

### END OF README ###