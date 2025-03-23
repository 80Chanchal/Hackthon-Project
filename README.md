# WebXR Whiteboard

A virtual whiteboard application built with Unity and WebXR, allowing users to draw in both desktop and VR environments.

## Features

- Real-time drawing in both 2D (desktop) and 3D (VR) environments
- Pen color and size customization
- Eraser functionality
- Compatible with WebXR headsets
- Cross-platform accessibility via web browsers

## Requirements

- Unity 2020.3 or newer
- WebXR Export Template for Unity
- Compatible WebXR-enabled browsers (Chrome, Firefox, Edge with WebXR support)
- VR headset compatible with WebXR (optional)

## How to Use

### Desktop Mode:
- Left-click to draw
- Right-click to erase
- Use the color palette to change colors
- Adjust pen size with the slider

### VR Mode:
- Trigger button to draw
- B/Y button to erase
- Use UI panel to change colors and pen size

## Implementation

The project uses:
- Unity's XR Interaction Toolkit
- Custom WebXR implementation
- Custom drawing system that works across platforms

## Building and Deployment

1. Open the project in Unity
2. Go to File > Build Settings
3. Select WebGL as the platform
4. Click "Switch Platform" if needed
5. Configure Player Settings for WebXR
6. Build the project
7. Host on a server that supports HTTPS
8. Access via WebXR-compatible browser

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Acknowledgments

- Built by Chanchal (GitHub: @80Chanchal)
- WebXR API and standards
- Unity Technologies 