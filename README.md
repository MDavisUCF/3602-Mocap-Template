<h1>Physical Computing – Motion Capture Game Template</h1>

<h2>Overview</h2>

<p>
  This Unity template serves as the foundation for motion-controlled and physical computing game projects.
  It is designed to support rapid prototyping, gameplay experimentation, and future integration with
  OptiTrack Motive motion capture systems and reactive physical props.
</p>

<p>
  The template separates gameplay systems from hardware/input systems. This allows students to build and
  test gameplay using keyboard controls before integrating motion capture, sensors, Arduino systems, or
  physical props.
</p>

<p>
  The project is intentionally simple and modular. Students are encouraged to expand the framework with
  new mechanics, visuals, audio feedback, physical interactions, and alternative controller designs.
</p>

<hr>

<details open>
  <summary><strong>Unity Version</strong></summary>

  <p>
    This project was created using:
  </p>

  <p>
    <strong>Unity 6.3 LTS</strong>
  </p>

  <p>
    Open the project using the Unity version specified by your instructor. Opening the project in a
    different Unity version may cause compatibility issues.
  </p>
</details>

<hr>

<details open>
  <summary><strong>How to Download This Template</strong></summary>

  <p>
    To download this Unity template from GitHub:
  </p>

  <ol>
    <li>Click the green <strong>Code</strong> button near the top of the GitHub page.</li>
    <li>Select <strong>Download ZIP</strong>.</li>
    <li>Extract/unzip the downloaded folder.</li>
    <li>Open <strong>Unity Hub</strong>.</li>
    <li>Click <strong>Add</strong> or <strong>Add project from disk</strong>.</li>
    <li>Select the extracted project folder.</li>
    <li>Open the project using the Unity version listed for the course.</li>
  </ol>

  <p>
    <strong>Do not open the project directly from inside the ZIP file.</strong><br>
    The folder must be fully extracted first.
  </p>
</details>

<hr>

<details open>
  <summary><strong>Student Workflow Rule</strong></summary>

  <p>
    Do not modify the master template scene directly.
  </p>

  <p>
    Before starting your project:
  </p>

  <ol>
    <li>Open the template project in Unity.</li>
    <li>Open the scene named <code>MASTER_DO_NOT_EDIT</code>.</li>
    <li>Use <strong>File &gt; Save As</strong>.</li>
    <li>Save your copy in: <code>Assets/StudentWork/Scenes/</code></li>
    <li>Rename your scene using this format: <code>TeamName_ProjectName.unity</code></li>
  </ol>

  <p>
    All new student-created assets should go inside the <code>Assets/StudentWork/</code> folder.
  </p>
</details>

<hr>

<details>
  <summary><strong>Project Folder Structure</strong></summary>

  <p>
    The main template files are located in:
  </p>

  <pre><code>Assets/_CourseTemplate/</code></pre>

  <h3>Folder Overview</h3>

  <pre><code>_CourseTemplate
├── Audio
├── Documentation
├── Materials
├── OptiTrack
├── Prefabs
├── Scenes
├── Scripts
└── Sprites_UI</code></pre>

  <h3>Audio</h3>

  <p>
    Contains sound effects used by the template.
  </p>

  <p>Examples:</p>

  <pre><code>SFX_Score
SFX_Danger
SFX_PropActivate
SFX_PropDeactivate
SFX_ModeToggle
SFX_Reset</code></pre>

  <h3>Documentation</h3>

  <p>
    Contains README files, setup notes, and student instructions.
  </p>

  <h3>Materials</h3>

  <p>
    Contains materials for the tracked prop, score zones, danger zones, floor, and visual debugging.
  </p>

  <h3>OptiTrack</h3>

  <p>
    Contains OptiTrack Unity plugin files and mocap-related systems.
  </p>

  <p>
    <strong>Do not edit files in this folder unless instructed.</strong>
  </p>

  <h3>Prefabs</h3>

  <p>
    Contains reusable gameplay objects.
  </p>

  <p>Current prefabs include:</p>

  <pre><code>Player_TrackedProp
ScoreZone
DangerZone</code></pre>

  <p>
    Students are encouraged to duplicate and modify prefabs rather than rebuilding systems from scratch.
  </p>

  <h3>Scenes</h3>

  <p>
    Contains the master template scene and any starter scenes.
  </p>

  <h3>Scripts</h3>

  <p>
    Contains the template scripts, organized by system type.
  </p>

  <pre><code>Scripts
├── Audio
├── Core
├── Gameplay
├── Input
└── UI</code></pre>

  <h3>Sprites_UI</h3>

  <p>
    Reserved for UI images, icons, and visual interface elements.
  </p>
</details>

<hr>

<details>
  <summary><strong>StudentWork Folder</strong></summary>

  <p>
    Students should place their own work in:
  </p>

  <pre><code>Assets/StudentWork/</code></pre>

  <p>Recommended structure:</p>

  <pre><code>StudentWork
├── Audio
├── Materials
├── Prefabs
├── Scenes
└── Scripts</code></pre>

  <p>
    This keeps student-created assets separate from the master template systems.
  </p>
</details>

<hr>

<details>
  <summary><strong>Scene Hierarchy</strong></summary>

  <p>
    The scene is organized into major sections:
  </p>

  <pre><code>MASTER_DO_NOT_EDIT
├── --- SYSTEMS ---
├── --- CAMERA / LIGHTING ---
├── --- GAMEPLAY ---
├── --- OPTITRACK / MOCAP ---
├── --- UI ---
└── --- DEBUG / TESTING ---</code></pre>

  <h3>Systems</h3>

  <p>
    Contains global managers and core systems.
  </p>

  <pre><code>GameManager
InputManager
UIManager
AudioManager
EventSystem</code></pre>

  <h3>Camera / Lighting</h3>

  <p>
    Contains the main camera and scene lighting.
  </p>

  <pre><code>Main Camera
Directional Light</code></pre>

  <h3>Gameplay</h3>

  <p>
    Contains gameplay objects and interaction systems.
  </p>

  <pre><code>Floor
Player_TrackedProp
Target_MocapInput
ScoreZone
DangerZone</code></pre>

  <h3>OptiTrack / Mocap</h3>

  <p>
    Contains motion capture source objects.
  </p>

  <pre><code>Source_OptiTrackRigidBody_DO_NOT_EDIT
OptiTrackClient</code></pre>

  <h3>UI</h3>

  <p>
    Contains all HUD and interface elements.
  </p>

  <pre><code>UI_Canvas</code></pre>

  <h3>Debug / Testing</h3>

  <p>
    Reserved for future testing objects and debugging tools.
  </p>
</details>

<hr>

<details>
  <summary><strong>Core Systems</strong></summary>

  <h3>Player_TrackedProp</h3>

  <p>
    The main gameplay prop used for interaction systems.
  </p>

  <p>
    This object represents the Unity/gameplay version of the physical prop. It is the visible object that
    interacts with score zones, danger zones, and other gameplay mechanics.
  </p>

  <h3>Target_MocapInput</h3>

  <p>
    Invisible tracking anchor object.
  </p>

  <p>
    Input systems move this object, and <code>Player_TrackedProp</code> follows it. This allows the template
    to separate raw input data from the actual gameplay object.
  </p>

  <p>
    Future input systems should update <code>Target_MocapInput</code> rather than directly controlling
    <code>Player_TrackedProp</code>.
  </p>

  <h3>Source_OptiTrackRigidBody_DO_NOT_EDIT</h3>

  <p>
    The raw mocap source object.
  </p>

  <p>
    This object is intended to receive live tracking data from the OptiTrack Unity plugin and Motive.
    It should be assigned to the <code>Live Mocap Source</code> field on <code>InputManager</code>.
  </p>

  <p>
    This object should not contain gameplay logic.
  </p>

  <h3>OptiTrackClient</h3>

  <p>
    The scene object responsible for connecting Unity to Motive through the OptiTrack Unity plugin.
  </p>

  <p>
    This object should contain the OptiTrack streaming client component.
  </p>

  <h3>GameManager</h3>

  <p>Handles:</p>

  <pre><code>Score
Timer
Round state
Reset behavior
Game running state</code></pre>

  <h3>InputManager</h3>

  <p>Handles:</p>

  <pre><code>Input mode switching
Keyboard input
Mocap placeholder movement
Live mocap source movement
Prop activation input
Passing movement/state data to Player_TrackedProp</code></pre>

  <h3>UIManager</h3>

  <p>Updates HUD elements including:</p>

  <pre><code>Score
Timer
Prop state
Input mode</code></pre>

  <h3>AudioManager</h3>

  <p>Handles simple sound effect playback for:</p>

  <pre><code>Score zone interaction
Danger zone interaction
Prop activation
Prop deactivation
Input mode switching
Scene reset</code></pre>

  <h3>PropState</h3>

  <p>
    Stores the current interaction state of the tracked prop.
  </p>
</details>

<hr>

<details open>
  <summary><strong>Input Modes</strong></summary>

  <p>
    This template currently supports three input modes.
  </p>

  <h3>1. DebugKeyboard</h3>

  <p>
    Keyboard-based testing mode.
  </p>

  <p>
    Use this mode first when designing and testing gameplay mechanics.
  </p>

  <p><strong>Controls:</strong></p>

  <pre><code>WASD = Move Prop
SPACE = Activate Prop
R = Reset Scene
M = Toggle Input Mode</code></pre>

  <p>
    In this mode, keyboard input moves <code>Target_MocapInput</code>, and
    <code>Player_TrackedProp</code> follows it.
  </p>

  <h3>2. MocapPlaceholder</h3>

  <p>
    Simulated motion capture mode.
  </p>

  <p>
    This mode uses automated circular movement to mimic incoming tracking data. It is useful for testing
    motion-driven gameplay without needing access to the mocap lab.
  </p>

  <p>In this mode:</p>

  <pre><code>Target_MocapInput moves automatically in a circle
Player_TrackedProp follows Target_MocapInput
SPACE still controls prop activation</code></pre>

  <p>
    This mode should remain in the template as a testing tool.
  </p>

  <h3>3. MocapLive</h3>

  <p>
    Live motion capture mode.
  </p>

  <p>
    This mode is intended for OptiTrack Motive integration.
  </p>

  <p>In this mode:</p>

  <pre><code>Source_OptiTrackRigidBody_DO_NOT_EDIT provides live mocap position/rotation
InputManager copies that data to Target_MocapInput
Player_TrackedProp follows Target_MocapInput</code></pre>

  <p>Live mocap mode requires:</p>

  <pre><code>OptiTrack Unity Plugin
Motive streaming enabled
A rigid body created in Motive
OptiTrackClient configured in Unity
Source_OptiTrackRigidBody_DO_NOT_EDIT assigned as Live Mocap Source</code></pre>
</details>

<hr>

<details>
  <summary><strong>Gameplay Rules</strong></summary>

  <h3>Score Zones</h3>

  <p>
    Green score zones increase the score when the prop enters while active.
  </p>

  <pre><code>Active prop enters ScoreZone = score increases
Inactive prop enters ScoreZone = no effect</code></pre>

  <h3>Danger Zones</h3>

  <p>
    Red danger zones decrease the score when the prop enters while active.
  </p>

  <pre><code>Active prop enters DangerZone = score decreases
Inactive prop enters DangerZone = no effect</code></pre>

  <h3>Prop Activation</h3>

  <p>
    The prop must be active to interact with gameplay zones.
  </p>

  <p>
    In DebugKeyboard and MocapPlaceholder modes, the prop is activated by holding:
  </p>

  <pre><code>SPACE</code></pre>

  <p>
    The prop changes material based on its input mode and active state.
  </p>
</details>

<hr>

<details>
  <summary><strong>Audio Feedback</strong></summary>

  <p>
    The template includes simple sound effect support through <code>AudioManager</code>.
  </p>

  <p>Current audio events include:</p>

  <pre><code>ScoreZone interaction
DangerZone interaction
Prop activated
Prop deactivated
Input mode changed
Scene reset</code></pre>

  <p>
    Audio feedback is intended to help players understand the state of the prop and confirm gameplay interactions.
  </p>

  <p>
    Students may replace the default sound effects with their own, but should keep audio calls connected to the
    appropriate gameplay events.
  </p>
</details>

<hr>

<details>
  <summary><strong>OptiTrack Integration Notes</strong></summary>

  <p>
    The intended mocap data flow is:
  </p>

  <pre><code>Physical Prop with IR LEDs
→ OptiTrack Cameras
→ Motive Rigid Body
→ OptiTrack Unity Plugin
→ Source_OptiTrackRigidBody_DO_NOT_EDIT
→ InputManager
→ Target_MocapInput
→ Player_TrackedProp
→ Gameplay Zones</code></pre>

  <p>
    The <code>Source_OptiTrackRigidBody_DO_NOT_EDIT</code> object represents raw tracking data.
  </p>

  <p>
    The <code>Target_MocapInput</code> object represents the cleaned/mapped input target used by the game.
  </p>

  <p>
    The <code>Player_TrackedProp</code> object represents the gameplay prop that interacts with zones and mechanics.
  </p>

  <p>
    This separation allows students to test gameplay without mocap hardware and integrate live tracking later
    without rewriting core gameplay systems.
  </p>
</details>

<hr>

<details open>
  <summary><strong>Important Design Rules</strong></summary>

  <h3>Do</h3>

  <ul>
    <li>Prototype gameplay in DebugKeyboard mode first.</li>
    <li>Use MocapPlaceholder mode to test motion-based gameplay without hardware.</li>
    <li>Use MocapLive only when OptiTrack/Motive is available.</li>
    <li>Build gameplay systems using <code>PropState</code> and <code>TrackedPropController</code>.</li>
    <li>Update <code>Target_MocapInput</code> rather than directly moving <code>Player_TrackedProp</code>.</li>
    <li>Keep gameplay systems modular.</li>
    <li>Organize hierarchy objects cleanly.</li>
    <li>Comment scripts when adding new systems.</li>
    <li>Duplicate the template scene before making major changes.</li>
  </ul>

  <h3>Do Not</h3>

  <ul>
    <li>Do not modify the <code>MASTER_DO_NOT_EDIT</code> scene directly.</li>
    <li>Do not delete manager objects without understanding dependencies.</li>
    <li>Do not directly tie gameplay systems to hardware-specific input.</li>
    <li>Do not hardcode gameplay logic directly into input providers.</li>
    <li>Do not add OptiTrack tracking scripts directly to <code>Player_TrackedProp</code> unless instructed.</li>
    <li>Do not make <code>Source_OptiTrackRigidBody_DO_NOT_EDIT</code> responsible for gameplay logic.</li>
    <li>Do not edit OptiTrack plugin files unless instructed.</li>
  </ul>
</details>

<hr>

<details open>
  <summary><strong>Recommended Workflow</strong></summary>

  <ol>
    <li>Duplicate the master scene.</li>
    <li>Save your copy in <code>Assets/StudentWork/Scenes/</code>.</li>
    <li>Prototype gameplay in DebugKeyboard mode.</li>
    <li>Test score zones, danger zones, UI, and audio.</li>
    <li>Test motion-based interaction using MocapPlaceholder mode.</li>
    <li>Build and test physical/reactive prop systems.</li>
    <li>Integrate motion capture tracking using MocapLive mode.</li>
    <li>Polish gameplay, feedback, UI, and physical interaction.</li>
  </ol>
</details>

<hr>

<details>
  <summary><strong>Known Notes</strong></summary>

  <ul>
    <li>Input mode persists after reset.</li>
    <li>The tracked prop changes material based on input mode and active state.</li>
    <li>MocapPlaceholder mode simulates incoming tracking data.</li>
    <li>MocapLive mode expects a live mocap source object.</li>
    <li>Future tracking systems should update <code>Target_MocapInput</code> rather than directly controlling <code>Player_TrackedProp</code>.</li>
    <li><code>Player_TrackedProp</code> is the gameplay object, not the raw mocap source.</li>
    <li><code>Source_OptiTrackRigidBody_DO_NOT_EDIT</code> is the raw mocap source, not the gameplay prop.</li>
    <li>Reset reloads the scene after a short delay so the reset sound can play.</li>
  </ul>
</details>

<hr>

<p><strong>End of README</strong></p>
