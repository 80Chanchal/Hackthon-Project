# WebXR Whiteboard Guide

This document provides instructions for setting up and using WebXR with your whiteboard project.

## Setup Instructions

1. **Install WebXR in Unity:**
   - The WebXR package should already be installed through the package manifest
   - If not, it will be automatically added when you open the project in Unity

2. **Set up your scene:**
   - Go to the Unity menu and select `WebXR > Setup WebXR Scene`
   - This will open a utility window where you can choose which components to add
   - Click "Create WebXR Setup" to add WebXR components to your scene

3. **Configure WebXR settings:**
   - Go to the Unity menu and select `WebXR > Configure WebGL Settings`
   - This will automatically set up the proper WebGL settings for WebXR

## Building for WebXR

1. **Switch to WebGL platform:**
   - Go to `File > Build Settings`
   - Select WebGL and click "Switch Platform"
   - Make sure "Development Build" is NOT checked for final builds

2. **Configure Player Settings:**
   - Click "Player Settings" in the Build Settings window
   - Under "Resolution and Presentation", ensure template is set to "WebXR"
   - Other settings should be automatically configured

3. **Build the project:**
   - Click "Build" and choose a location for your build
   - Upload all files to a web server with HTTPS support (required for WebXR)

## Using the WebXR Whiteboard

### Desktop Mode (Non-VR):
- Use a mouse to control the whiteboard
- Left-click to draw
- Right-click to erase
- Use UI controls to change colors and pen size

### VR Mode:
- Access via a WebXR-compatible browser (Chrome, Firefox, or Edge)
- Click the "Enter VR" button on the page
- Use the trigger button on your controllers to draw
- Use the grip or B/Y buttons to erase
- Use the UI panel to change colors and pen sizes

## Troubleshooting

If you encounter issues:

1. **Ensure your browser supports WebXR**
   - Check compatibility at [WebXR Browser Compatibility](https://developer.mozilla.org/en-US/docs/Web/API/WebXR_Device_API#browser_compatibility)

2. **Verify HTTPS connection**
   - WebXR requires a secure connection (HTTPS)
   - For local testing, you can use localhost or a tool like ngrok

3. **Check console for errors**
   - Open browser developer tools (F12) and check for WebXR-related errors

4. **Verify WebXR is enabled**
   - In Chrome, go to `chrome://flags` and ensure WebXR is enabled

## Additional Resources

- [WebXR Developer Guide](https://developer.mozilla.org/en-US/docs/Web/API/WebXR_Device_API)
- [Unity WebXR Exporter Documentation](https://github.com/De-Panther/unity-webxr-export)
- [WebXR Samples](https://immersive-web.github.io/webxr-samples/) 