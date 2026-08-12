PHYSICAL COMPUTING – UNITY TEMPLATE README

## PURPOSE ##

This Unity project is a starter template for motion-controlled and physical computing game projects.

The template is designed so gameplay can be tested with keyboard controls first, then later connected to motion capture, sensors, Arduino systems, or physical props.

The main idea is:

   Input System
   → Target_MocapInput
   → Player_TrackedProp
   → Gameplay Zones

Students should build gameplay around the provided systems instead of rewriting the core input framework.

--------------------------

## DO NOT EDIT THE MASTER SCENE ##

Do not modify the master scene directly.

Before starting your project:

   1. Open MASTER_DO_NOT_EDIT.
   2. Use File > Save As.
   3. Save your copy in:

      Assets/StudentWork/Scenes/

   4. Rename the scene using this format:

      TeamName_ProjectName.unity

All new student-created assets should go inside:

   Assets/StudentWork/

--------------------------

## CONTROLS ##

WASD = Move prop in DebugKeyboard mode
SPACE = Activate prop
M = Toggle input mode
R = Reset scene

--------------------------

## INPUT MODES ##

This template currently supports three input modes.

1. DebugKeyboard
   Keyboard testing mode.

   Use this mode first when building and testing gameplay.

   In this mode:
      WASD moves Target_MocapInput
      Player_TrackedProp follows Target_MocapInput
      SPACE activates the prop

2. MocapPlaceholder
   Simulated motion capture mode.

   This mode moves the prop automatically in a circle to mimic incoming tracking data. Use this to test motion-based mechanics without the mocap lab.

   In this mode:
      Target_MocapInput moves automatically
      Player_TrackedProp follows Target_MocapInput
      SPACE still activates the prop

3. MocapLive
   Live OptiTrack/Motive mode.

   This mode is intended for use with the mocap system.

   In this mode:
      Source_OptiTrackRigidBody_DO_NOT_EDIT
      → InputManager
      → Target_MocapInput
      → Player_TrackedProp

   Do not use MocapLive unless the OptiTrack system is available and configured.

--------------------------

## SCENE OBJECTS ##

--- Player_TrackedProp ---
The visible gameplay prop.

This object interacts with score zones, danger zones, and other gameplay mechanics.

--- Target_MocapInput ---
The invisible input anchor.

Input systems move this object. The player prop follows it.

Do not put gameplay logic on this object.

--- Source_OptiTrackRigidBody_DO_NOT_EDIT ---
The raw OptiTrack tracking source.

This object is intended to receive live tracking data from Motive through the OptiTrack Unity plugin.

Do not add gameplay logic to this object.

--- GameManager ---
Handles score, timer, reset behavior, and round state.

--- InputManager ---
Handles keyboard input, placeholder mocap movement, live mocap input, prop activation, and input mode switching.

--- UIManager ---
Updates HUD text for score, timer, prop state, and input mode.

--- AudioManager ---
Handles simple sound effect playback for score, danger, activation, deactivation, input mode changes, and reset.

--------------------------

## GAMEPLAY RULES ##

--- Score Zones ---
Green zones increase the score when the prop enters while active.

   Active prop enters ScoreZone = score increases
   Inactive prop enters ScoreZone = no effect

--- Danger Zones ---
Red zones decrease the score when the prop enters while active.

   Active prop enters DangerZone = score decreases
   Inactive prop enters DangerZone = no effect

The prop must be active to interact with gameplay zones.

--------------------------

## AUDIO ##

The template includes simple sound feedback through AudioManager.

Current audio events include:

   Score zone interaction
   Danger zone interaction
   Prop activation
   Prop deactivation
   Input mode change
   Scene reset

Students may replace sound effects, but should keep sounds connected to the correct gameplay events.

--------------------------

## RECOMMENDED WORKFLOW ##

1. Duplicate the master scene.
2. Save your scene in Assets/StudentWork/Scenes/.
3. Prototype gameplay in DebugKeyboard mode.
4. Test scoring, danger zones, UI, and audio.
5. Test motion-based interaction using MocapPlaceholder mode.
6. Add custom visuals, mechanics, and feedback.
7. Test physical/reactive prop systems.
8. Use MocapLive only when working with OptiTrack/Motive.

--------------------------

## IMPORTANT RULES ##

--- DO ---
   Build gameplay systems around Player_TrackedProp.
   Use PropState and TrackedPropController for prop interaction logic.
   Prototype with DebugKeyboard first.
   Use MocapPlaceholder to test motion-driven gameplay.
   Keep student-created files inside Assets/StudentWork/.
   Comment scripts when adding new systems.

--- DO NOT ---
   Do not edit MASTER_DO_NOT_EDIT directly.
   Do not delete manager objects.
   Do not add gameplay logic to Source_OptiTrackRigidBody_DO_NOT_EDIT.
   Do not add gameplay logic to Target_MocapInput.
   Do not directly tie gameplay systems to hardware-specific input.
   Do not edit OptiTrack plugin files unless instructed.

--------------------------

## KNOWN NOTES ##

Input mode persists after reset.
The prop changes material based on input mode and active state.
MocapPlaceholder simulates incoming tracking data.
MocapLive expects a live mocap source object.
Reset reloads the scene after a short delay so the reset sound can play.

--------------------------

## QUICK TROUBLESHOOTING ##

--- The prop is not moving ---
Check that:
   InputManager exists in the scene.
   Tracked Prop is assigned.
   Mocap Target is assigned.
   The current input mode is DebugKeyboard.
   The game is running.

--- The prop moves but zones do not work ---
Check that:
   The prop has TrackedPropController.
   The prop has PropState.
   Zone colliders are set to Is Trigger.
   The prop is active while entering the zone.

--- Sounds do not play ---
Check that:
   AudioManager exists in the scene.
   Audio clips are assigned in the Inspector.
   The Audio Source volume is above 0.
   The sound is being triggered after the correct gameplay condition.

--- MocapLive does not work ---
Check that:
   Motive is open.
   Motive is streaming rigid body data.
   The OptiTrack Unity plugin is configured.
   Source_OptiTrackRigidBody_DO_NOT_EDIT is receiving data.
   Live Mocap Source is assigned on InputManager.

--------------------------

### END OF README ###