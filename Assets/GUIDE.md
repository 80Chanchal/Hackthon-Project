# WebXR Whiteboard Guide

This guide explains how to set up and use the interactive WebXR whiteboard.

## Quick Setup

1. Open the Unity project named "whiteboard"
2. Go to the **Scenes** folder in Assets
3. Open the **WhiteboardScene** scene
4. Press Play to test in desktop mode

## Using the Whiteboard

### Desktop Mode
- **Left Click**: Draw on the whiteboard
- **Right Click**: Erase from the whiteboard
- Use the UI panel to:
  - Change pen size
  - Select different colors
  - Clear the whiteboard

### VR Mode
- **Trigger Button**: Draw on the whiteboard
- **Secondary Button (B/Y)**: Erase from the whiteboard  
- Use the controller to point at UI elements and press Trigger to interact

## Building for WebXR

To build the project for WebXR:

1. Go to **File > Build Settings**
2. Select **WebGL** as the platform (install if not already installed)
3. Click **Switch Platform**
4. Click **Player Settings**
5. Under **Publishing Settings**:
   - Enable "Compression Format: Disabled" for development builds
   - Select "WebGL 2.0" for "WebGL Template"
6. Add WebXR template if it's not already in the list (See [WebXR Exporter](https://github.com/De-Panther/unity-webxr-export) for details)
7. Click **Build** and select a folder to save the WebGL build
8. Host the built files on a web server with HTTPS (required for WebXR)

## Troubleshooting

### Common Issues:

1. **Whiteboard not showing up**: Make sure the SceneSetup GameObject is in the scene with its script enabled
2. **Cannot draw on whiteboard**: Check that raycasts from the camera/controllers hit the whiteboard
3. **WebXR not working**: Ensure you're:
   - Using a WebXR-compatible browser (Chrome, Edge, Firefox)
   - Serving the content over HTTPS
   - Using VR headset compatible with WebXR

### If materials appear pink:
- URP materials may not be properly set up
- In Scene view, select the whiteboard
- Look for missing materials in the Inspector
- Use the dropdown to assign a Standard shader temporarily

## Creating a New Scene

If you want to create a new scene from scratch:

1. Create a new scene
2. Add a GameObject named "SceneSetup"
3. Add the **SceneSetup** component to it
4. Make sure the following options are enabled:
   - Create Whiteboard
   - Setup WebXR
   - Create Room Environment (optional)
5. Save your scene

## Customization Options

You can customize the whiteboard by modifying:

- **Whiteboard size**: Change the whiteboard scale in WhiteboardSceneSetup
- **Pen colors**: Add more colors to the penColors array in WhiteboardSceneSetup
- **Room dimensions**: Adjust roomSize in the SceneSetup component
- **Pen texture quality**: Adjust textureWidth and textureHeight in the Whiteboard component

## Extending the Project

Some ideas to extend the functionality:

1. Add multiplayer capability for collaborative whiteboarding
2. Implement save/load functionality for whiteboard content
3. Add different brush types (marker, highlighter, etc.)
4. Include image import capability to draw over existing images
5. Add text input functionality for adding text to the whiteboard 