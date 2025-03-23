# Interactive WebXR Whiteboard for Unity

This project implements an interactive whiteboard for WebXR applications in Unity. The whiteboard allows teachers to write and erase content using mouse inputs (left click to draw, right click to erase) or VR controllers in an immersive environment.

## Features

- Interactive whiteboard with drawing and erasing capabilities
- Left mouse button / VR controller trigger for drawing
- Right mouse button / VR controller secondary button for erasing
- Adjustable pen size and color
- Works in both desktop and VR modes
- Simple UI for controlling whiteboard functions

## Requirements

- Unity 2022.3 or newer
- XR Interaction Toolkit package
- Universal Render Pipeline (URP)

## Installation

1. Open the Unity project
2. Ensure you have the following packages installed:
   - XR Interaction Toolkit
   - Universal Render Pipeline (URP)
   - Input System
   - TextMeshPro (for UI elements)

To install the packages through the Package Manager:
1. Open Window > Package Manager
2. Click the "+" button and select "Add package by name"
3. Add the following packages:
   - com.unity.xr.interaction.toolkit
   - com.unity.render-pipelines.universal
   - com.unity.inputsystem
   - com.unity.textmeshpro

## Setup

### Quick Setup

1. Drag the `WhiteboardSetup` prefab into your scene
2. Press Play to test the whiteboard functionality

### Manual Setup

1. Create a new scene or use an existing one
2. Add a quad mesh to represent the whiteboard
3. Add the `Whiteboard` script to the quad
4. Setup an XR Rig with controllers (for VR), or use a standard camera for desktop testing
5. Add the `WhiteboardController` script to the whiteboard
6. Setup UI elements and add the `WhiteboardUI` script

## Usage

### Desktop Mode
- Left click and drag to draw on the whiteboard
- Right click and drag to erase content
- Use the UI buttons to change pen size, color, or clear the board

### VR Mode
- Point your controller at the whiteboard
- Press and hold the trigger to draw
- Press and hold the secondary button (B/Y) to erase
- Use the UI by pointing and clicking with your controller

## Scripts Overview

- `Whiteboard.cs`: Main whiteboard functionality for drawing and erasing
- `WhiteboardController.cs`: Handles VR controller inputs
- `InputSimulator.cs`: Helper class for simulating input between VR and desktop
- `WhiteboardUI.cs`: UI controls for the whiteboard
- `WhiteboardSetup.cs`: Automatic setup of the whiteboard environment

## Customization

You can customize the whiteboard by modifying the following properties:
- Texture resolution (default is 2048x1024)
- Pen size and colors
- Whiteboard size and position
- UI layout and controls

## Troubleshooting

If you encounter issues:
1. Make sure all required packages are installed
2. Check that the material on the whiteboard has a proper shader assigned
3. Ensure the WhiteboardController has a valid reference to the XR Ray Interactor
4. For WebXR builds, ensure WebXR export settings are correctly configured

## License

This project is available for free use in educational and personal projects. 