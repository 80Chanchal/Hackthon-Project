# Unity Whiteboard System

A virtual whiteboard system for Unity that allows teachers and users to draw, erase, and save their work in real-time.

## Features

- Real-time drawing and erasing
- Multiple color options
- Adjustable brush and eraser sizes
- Save drawings as PNG files
- Mouse interaction visualization
- Debug mode for development
- Easy setup with WhiteboardSetupGuide

## Requirements

- Unity 2019.4 or later
- Universal Render Pipeline (URP) or Standard Render Pipeline

## Installation

1. Clone this repository
2. Open the project in Unity
3. Add the WhiteboardSetupGuide component to any GameObject in your scene
4. Click "Setup Whiteboard System" in the Inspector

## Usage

### Basic Controls

- **Left Mouse Button**: Draw on the whiteboard
- **Right Mouse Button**: Erase from the whiteboard
- **D key**: Switch to draw mode
- **E key**: Switch to erase mode

### UI Controls

- **Color Buttons**: Select different colors for drawing
- **Clear Button**: Clear the entire whiteboard
- **Save Button**: Save the current drawing as a PNG image
- **Brush Size Slider**: Adjust the size of the drawing brush
- **Eraser Size Slider**: Adjust the size of the eraser
- **Mouse UI Toggle**: Show/hide the mouse interaction indicator
- **Debug Toggle**: Show/hide visual debug information

## Project Structure

```
Assets/
├── Prefabs/
│   ├── Whiteboard.prefab       # The main whiteboard object
│   └── WhiteboardUI.prefab     # UI controls for the whiteboard
├── Scripts/
│   ├── WhiteboardDrawing.cs    # Core drawing functionality
│   ├── WhiteboardUIController.cs # UI management
│   ├── MouseInteractionHandler.cs # Mouse input handling
│   ├── MouseInteractionUI.cs   # Mouse UI visualization
│   └── WhiteboardSetupGuide.cs # Easy setup tool
└── Materials/
    └── WhiteboardMaterial.mat  # Material for the whiteboard
```

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Author

- **80Chanchal** - [GitHub](https://github.com/80Chanchal)

## Acknowledgments

- Unity Technologies for the game engine
- The Unity community for inspiration and resources
