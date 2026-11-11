# Physical Computing: Motion Capture Game Template

UPDATED 11/11/2026

A Unity starter project for creating games controlled by a **physical tracked prop** using **OptiTrack Motive**.

This template provides a working motion capture system, gameplay interactions, scoring, a timer, and basic UI. Your job is to build on that foundation to create your own physical computing game.

**Course:** DIG 3602 Physical Computing  
**Unity Version:** 6.3 LTS

---

## Getting Started

1. Open the project in Unity.
2. Find the scene named `MASTER_DO_NOT_EDIT`.
3. **Duplicate the scene** and rename your copy for your project.
4. Open your duplicated scene.
5. Press Play to test the starter game.
6. Use `DebugKeyboard` to develop and test your gameplay before connecting physical hardware.

**Do not modify the master scene directly.** It exists as a working reference in case you need to restore something.

The red and green gameplay zones are examples. You are welcome to redesign or replace them.

---

## Controls & Input Modes

The template supports two input modes.

| Control | Function |
|---|---|
| **WASD** | Move the prop in keyboard mode |
| **SPACE** | Activate the prop in keyboard mode |
| **M** | Switch between keyboard and motion capture |
| **R** | Reset the current scene |

### DebugKeyboard

This mode allows you to test your game without motion capture hardware.

- WASD moves the gameplay prop.
- Holding SPACE makes the prop active.
- Releasing SPACE makes the prop inactive.
- The prop remains visible even when inactive.

Use this mode while developing and testing mechanics outside the mocap lab.

### MocapLive

This mode uses live tracking data from OptiTrack Motive.

The physical prop controls the position and rotation of the gameplay prop in Unity.

**Activation is automatic:**

- **Tracking detected:** The gameplay prop appears and becomes active.
- **Tracking lost:** The gameplay prop disappears and becomes inactive.
- **Tracking restored:** The gameplay prop reappears and becomes active again.

When the prop loses tracking, its gameplay colliders are disabled so it cannot trigger scoring or penalties.

You do not need to press SPACE while using MocapLive.

**Important:** The template currently uses the age of received tracking data to determine whether the prop is tracked. Test deliberate tracking loss in Motive to verify this behaves correctly with the current streaming configuration.

---

## Understanding the Scene

The template separates motion capture input from gameplay interactions.

| GameObject | Purpose |
|---|---|
| `GameManager` | Handles scoring, timer, and resetting |
| `InputManager` | Handles keyboard and live mocap input |
| `UIManager` | Updates the HUD |
| `AudioManager` | Handles gameplay sound effects |
| `Player_TrackedProp` | Main gameplay object representing the physical prop |
| `Target_MocapInput` | Invisible target that receives movement information |
| `Source_OptiTrackRigidBody_DO_NOT_EDIT` | Receives raw Motive tracking data |
| `OptiTrackClient` | Connects Unity to the Motive stream |
| `ScoreZone` | Example positive scoring area |
| `DangerZone` | Example negative scoring area |
| `Floor` | Playable floor surface |
| `PlayArea_Bounds` | Visual representation of the physical play area |
| `UI_Canvas` | Contains the HUD |

### How tracking reaches the game

```text
Physical Prop
     ↓
OptiTrack Cameras
     ↓
Motive Rigid Body
     ↓
OptiTrackClient
     ↓
Source_OptiTrackRigidBody_DO_NOT_EDIT
     ↓
InputManager
     ↓
Target_MocapInput
     ↓
Player_TrackedProp
     ↓
Gameplay Interactions
```

This structure allows you to develop your game using keyboard controls and transition to motion capture without rebuilding the gameplay system.

---

## Physical Play Area

The template uses the approximate usable dimensions of the motion capture space established by the Piñata project.

| Dimension | Unity Units |
|---|---|
| Width (X) | 9.31 |
| Depth (Z) | 4.95 |

`PlayArea_Bounds` shows the approximate playable region.

These boundaries are visual guides, not physical barriers. They should not have colliders that prevent the tracked prop from moving.

Design your gameplay around what players can physically reach within the capture space.

**Remember:** The virtual boundaries must be aligned with the actual motion capture coordinates. Matching their dimensions alone does not guarantee alignment.

---

## Connecting Your Physical Prop to a 3D Model

Your game uses two different representations of the prop:

1. **Physical Prop:** The real object equipped with IR LEDs or markers and tracked by Motive.
2. **Virtual Prop:** The 3D model players see and interact with inside Unity.

The physical prop controls the virtual prop through the existing tracking system.

### Step 1: Import Your 3D Model

Import your model into the Unity project's `Assets` folder.

Your model may be an FBX, prefab, or another supported 3D asset.

In the Hierarchy, locate:

`Player_TrackedProp`

**Drag your imported model underneath `Player_TrackedProp` so it becomes a child object.**

Your Hierarchy should look something like:

```text
Player_TrackedProp
    └── YourModel
```

Do not replace the entire `Player_TrackedProp` GameObject with your imported asset.

### Step 2: Position and Align Your Model

Select your imported child model.

Start with these local Transform values:

| Property | Value |
|---|---|
| Position | (0, 0, 0) |
| Rotation | (0, 0, 0) |
| Scale | (1, 1, 1) |

Then adjust the child's local position, rotation, and scale as necessary.

**The model should be aligned relative to the physical prop's tracked origin.**

For example, if your physical controller is a sword, position the virtual sword so its handle and orientation make sense relative to where Motive tracks the physical object.

This is important because rotating the physical prop should cause the virtual model to rotate naturally.

### Step 3: Hide the Original Purple Cube

The starter gameplay prop includes a simple purple cube.

Once your model is positioned correctly:

1. Select `Player_TrackedProp`.
2. Find its Mesh Renderer component.
3. Disable the Mesh Renderer.

This hides the starter cube while allowing your custom child model to remain visible.

**Do not disable or delete `Player_TrackedProp`.**

That GameObject contains the scripts responsible for the gameplay prop's movement and active state.

### Step 4: Configure Colliders

Your custom model may have a different shape than the starter cube.

Check the existing collider on `Player_TrackedProp`.

You can resize or replace it with colliders that better represent your virtual prop.

Make sure the gameplay prop and interaction zones have the appropriate Collider, Trigger, and Rigidbody configuration for Unity physics.

The template uses colliders to detect interactions with gameplay zones.

When tracking is lost in MocapLive, `TrackedPropController` disables the gameplay prop's colliders until tracking returns.

### Step 5: Connect the Physical Prop in Motive

Your physical prop must be configured as a **Rigid Body** in OptiTrack Motive.

1. Attach your IR LEDs or tracking markers securely to the physical prop.
2. Create a Rigid Body in Motive.
3. Verify that Motive recognizes the prop and tracks its movement and rotation.
4. Find the Rigid Body's **Streaming ID**.

In Unity, select:

`Source_OptiTrackRigidBody_DO_NOT_EDIT`

Under the `OptitrackRigidBody` component:

- Set **Streaming Client** to `OptiTrackClient`.
- Set **Rigid Body ID** to match the ID configured in Motive.

Next, select `InputManager` and confirm:

- **Live Mocap Source:** `Source_OptiTrackRigidBody_DO_NOT_EDIT` Transform
- **Live Rigid Body:** `Source_OptiTrackRigidBody_DO_NOT_EDIT` OptitrackRigidBody component

You should not need to modify any of the core tracking scripts to connect your own physical prop.

### Step 6: Test Your Model

Enable streaming in Motive and press Play in Unity.

Press **M** to switch to MocapLive.

Test the following:

- Move the physical prop left and right.
- Move it forward and backward.
- Raise and lower it.
- Rotate it in different directions.
- Temporarily block its markers until Motive loses tracking.

Your virtual model should follow the physical prop's movement and rotation.

When tracking is lost, the model should disappear and become unable to interact with gameplay zones.

When tracking returns, it should reappear automatically.

If the model faces the wrong direction, adjust the **child model's local rotation** or the mocap rotation offset in `InputManager`.

If the tracked position needs adjustment, use the position offset in `InputManager`.

Do not reposition the raw OptiTrack source to compensate for an incorrectly aligned model.

### Important: Where Your Model Belongs

Your custom model should be a child of:

**`Player_TrackedProp`**

Do not attach it directly to:

- `Source_OptiTrackRigidBody_DO_NOT_EDIT`
- `OptiTrackClient`
- `Target_MocapInput`

Those objects are part of the tracking and input system.

Attaching your model to the wrong object may cause it to move without properly responding to activation, visibility, or collision behavior.

**Note:** `TrackedPropController` identifies child renderers and colliders when the scene starts. Add your custom model and gameplay colliders before pressing Play.

---

## Setting Up OptiTrack Streaming

The project already contains the OptiTrack Unity plugin and the necessary scene objects.

At the mocap station:

1. Open Motive.
2. Confirm your physical prop is recognized as a Rigid Body.
3. Enable streaming in Motive.
4. Verify that the `OptiTrackClient` settings in Unity match the streaming configuration in Motive.
5. Verify the Rigid Body ID matches.
6. Press Play in Unity and switch to MocapLive.

When Motive and Unity are running on the same computer, a local Unicast configuration using `127.0.0.1` may be used when both applications are configured accordingly.

**Do not disable or delete the OptiTrack streaming objects.**

These objects may be invisible during gameplay, but they are required for live tracking.

---

## Starter Gameplay

The template includes two basic gameplay zones.

### ScoreZone

Entering this area while the prop is active increases the player's score.

### DangerZone

Entering this area while the prop is active decreases the player's score.

Inactive props cannot trigger either interaction.

The starter game also includes:

- A 60-second round timer
- Score tracking
- Activation feedback
- Sound effects
- Reset functionality

These systems demonstrate how motion and prop activation can influence gameplay.

**You are not required to keep these mechanics.**

You can redesign the scoring system, replace the zones, create different objectives, or build completely new interactions.

The existing zones use trigger-entry events, so simply remaining inside a zone does not continuously add or subtract points.

---

## What You Can Customize

This is a starting framework, not a completed game.

You are encouraged to modify and expand:

- The virtual prop and its appearance
- Gameplay objectives and mechanics
- Score and danger zones
- Level design and environment
- UI and visual feedback
- Sound effects
- Game timer and scoring behavior
- Additional scripts and systems

You may also add other physical computing components and interactions as appropriate for your project.

### What Should Stay Connected

The core tracking system should remain functional.

Avoid deleting or disconnecting:

- `InputManager`
- `Target_MocapInput`
- `Source_OptiTrackRigidBody_DO_NOT_EDIT`
- `OptiTrackClient`
- The gameplay components on `Player_TrackedProp`

Keep raw tracking data separate from gameplay logic.

When extending the project, use the existing gameplay prop and its active state rather than putting scoring or interaction logic directly on the OptiTrack source.

---

## Troubleshooting

| Problem | What to Check |
|---|---|
| Prop does not move in MocapLive | Motive streaming, connection settings, Rigid Body ID, and InputManager references |
| Prop is always invisible | Confirm tracking is working and check the tracking timeout |
| Model moves but doesn't interact | Verify colliders, trigger settings, and Rigidbody configuration |
| Model faces the wrong direction | Adjust the model's local rotation or mocap rotation offset |
| Model appears in the wrong position | Check the model's local position and mocap position offset |
| Small cube remains visible | Disable Mesh Renderer on the unused tracking helper or starter cube |
| Score does not change | Confirm the prop is active and entering a valid trigger zone |
| Input controls seem incorrect | Check the current input mode and press M to switch |

If something is not working, test the starter gameplay using `DebugKeyboard` first.

This helps determine whether the issue is related to gameplay logic or motion capture tracking.

---

## Recommended Workflow

1. Duplicate the master scene.
2. Develop and test your gameplay using DebugKeyboard.
3. Import and attach your custom 3D model.
4. Adjust its alignment and collision geometry.
5. Build and configure your physical tracked prop.
6. Verify the rigid body in Motive.
7. Connect and test MocapLive.
8. Confirm tracking loss and interaction behavior.
9. Test your game with another player.
10. Polish and finalize the experience.

---

## Final Reminder

The purpose of this template is to provide a working technical foundation for motion-controlled gameplay.

You should not feel limited by the sample scoring zones, interface, or starter visuals.

**Use the existing motion capture system to create your own physical interaction and gameplay experience.**