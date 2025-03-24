# Whiteboard System for Unity

This whiteboard system allows teachers or users to draw on a virtual whiteboard in a Unity application.

## Setup Instructions

1. **Add the Whiteboard prefab** to your scene. This is the actual whiteboard object that users will draw on.

2. **Add a Canvas to your scene** if you don't already have one:
   - GameObject > UI > Canvas

3. **Add the WhiteboardUI prefab** to your canvas. This prefab contains the UI elements for controlling the whiteboard.

4. **Add a MouseInteractionHandler** component to your scene (GameObject > Create Empty, then add the component) or use the existing one if available.

5. **Connect the components**:
   - In the Whiteboard prefab, assign the MouseInteractionHandler to the "Interaction Handler" field
   - In the WhiteboardUI prefab, assign:
     - The WhiteboardDrawing component from the Whiteboard to the "Whiteboard Drawing" field
     - The MouseInteractionHandler to the "Interaction Handler" field
     - The MouseInteractionUI to the "Mouse UI" field

## Usage

### Drawing Controls

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

## Customization

You can customize the whiteboard by adjusting the following settings:

- **Drawing Settings**: Change default colors, texture size, brush and eraser sizes in the WhiteboardDrawing component
- **UI Settings**: Modify the UI layout and appearance through the Canvas and individual UI components
- **Mouse Interaction**: Adjust sensitivity and behavior in the MouseInteractionHandler component

## For Teachers

To use the whiteboard as a teacher:

1. Position your camera facing the whiteboard
2. Use the left mouse button to draw and the right mouse button to erase
3. Use the UI controls to change colors, clear the board, or save your drawings
4. Drawings are saved to the persistent data path in a "Drawings" folder

## Troubleshooting

If you encounter issues with the whiteboard:

- Ensure all components are properly connected in the Inspector
- Check that the whiteboard has a valid Mesh Renderer and Collider
- Verify that the whiteboard is within the camera's view and not blocked by other objects
- Make sure the whiteboard has a proper material assigned that can display textures 