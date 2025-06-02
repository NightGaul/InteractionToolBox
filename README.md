
# Interaction Library for Unity

A modular, extendable interaction-library for Unity developepers. Supports drag & drop, fetch & match, rotate & align, mashing and drawin & cover.

## Features

- Drag & Drop

    Pick up and move elements using the mouse or touch input. Ideal for sorting, arranging, or building mechanics.

- Fetch & Match

    Fetch an object and place it in a corresponding target. Combines movement with logic-based matching—perfect for organizing, combining, or categorizing items.

- Draw & Cover

    Draw or swipe over an area to reveal, highlight, or activate it. Great for scratch cards, hidden object discovery, or marking tasks.

- Mashing

    Rapidly press a key or button to trigger a repeated input. Commonly used in quick-time events, power buildup, or time-sensitive actions.

- Rotate & Align

    Rotate objects until they match a specific orientation. Useful for puzzles, alignment tasks, or visual customization mechanics.
## Using the Library
For each interaction there is a template scene to start from for easier implementation.
The project already contains the Scriptable Objects for the events and settings, but those can be modified if desired. The project also contains simples usage case levels for reference.


## SettingUp - Drag & Drop
1. Create an empty GameObject and add the InputListener script

    1.1 Add the required ScriptableObjects to the Inspector

2. Add your SnapPoints (3D Objects)

3. Add your Draggable 3D Objects and assign them the "Draggable" tag

4. Create an empty GameObject and add the DragAndDropManager script

    4.1 Assign the DragEvent ScriptableObject

    4.2 Set the Snap Tolerance

    4.3 Add your SnapPoints to the SnappableObject array

    4.4 Set your desired Visual Mode and add your Sound Effects



## SettingUp - Fetch & Match
1. Create an empty GameObject and add the InputListener script

    1.1 Add the required ScriptableObjects to the Inspector

2. Create a Player GameObject from the provided Player Prefab

3. Create an empty GameObject and add the FetchAndMatchManager script

    3.1 Add the InteractEvent ScriptableObject to the Inspector

    3.2 Add the Hand Transform of the player to the Inspector

4. Create Goal Object(s) and add a Collider and Rigidbody component

5. Add a DropOffPoint script to the Goal Object(s), and in the Inspector:
    
    5.1 Assign the FetchAndMatchManager GameObject
    
    5.2 Add the corresponding Goal ScriptableObjects to validate correct placement

6. Create Carrying Object(s) and add a Collider and the MatchableObject script
    
    6.1 Add the matching Goal ScriptableObjects in the Inspector for validation
## SettingUp - Draw & Cover
1. Create an empty GameObject and add the InputListener script

    1.1 Add the required ScriptableObjects to the Inspector
2. Create a Plane GameObject
3. Create an empty GameObject and add the CoverageManager

    3.1 Add the RightClick-Event to the Inspector

    3.2 If you use a Custom Cursor add a Canvas, to that canvas add an UI-Image and add the Custom-Cursor Script.
    
    3.3 Set your desired Textures, Brush-Modes and add your Sound Effects

    3.4 Set your coverage Material, best practise is to use the provided DrawAndCoverMaterial (When adding a custom coverage-material make sure it uses the shader: ShaderGraphs/MaskedRevealShader)


## SettingUp - Mash & Mash
1. Create an empty gamobject and add a MashManager Script
2. Choose your MashingMode and customize your MashingSettings
3. Set your desired Visual Mode and add your Sound Effects
## SettingUp - Rotate and Align
1. Create an empty GameObject and add the InputListener script

    1.1 Add the required ScriptableObjects to the Inspector
2. Add your 3D-Objects that are to be rotated (It's recomended to have them in a common parent object)

    2.1 Add a BoyCollider and a RotatablePiece Script (It's recomended to no mess with the variables, unless the auto-scramble isn't a viable option)

3. Create an empty GameObject and add the RotateAndAlignManager Script

    3.1 Add the Scrollevent ScriptableObject

    3.2 Set your prefered variables for the rotation

    3.3 Add your Rotatable-3D Objects to the RotatablePieces Array

    3.4 Set your desired Visual Mode and add your Sound Effects 

    TIP: Use autoscramble to avoid creating unsolvable puzzles

    
## Disclaimer
The following Assets are not created by me and are strictly for showing usage cases:
- Quick Outline(UNITY AssetStor -> Chris Nolet)
- Audio files are from pixabay.com (Royalty-free)
- Low Poly Atmospheric Locations Pack (UNITY AssetStor -> Palmov Island)
- Free Animals - Quirky Series (UNITY AssetStor -> Omabuarts Studio)
- FREE Stylized PBR Textures Pack (UNITY AssetStor -> Lumo-Art 3D)
- Low Poly Food Lite (UNITY AssetStor -> JustCreate)