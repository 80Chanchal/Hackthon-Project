using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public static class InputSimulator
{
    private static Vector3 simulatedMousePosition = Vector3.zero;
    private static bool[] mouseButtons = new bool[3]; // 0: left, 1: right, 2: middle

    // Current simulated mouse position in screen coordinates
    public static Vector3 MousePosition => simulatedMousePosition;

    // Update simulated mouse position
    public static void SimulateMousePosition(Vector3 screenPosition)
    {
        simulatedMousePosition = screenPosition;
    }

    // Simulate mouse button down
    public static void SimulateMouseDown(int buttonIndex)
    {
        if (buttonIndex >= 0 && buttonIndex < mouseButtons.Length)
        {
            mouseButtons[buttonIndex] = true;
        }
    }

    // Simulate mouse button up
    public static void SimulateMouseUp(int buttonIndex)
    {
        if (buttonIndex >= 0 && buttonIndex < mouseButtons.Length)
        {
            mouseButtons[buttonIndex] = false;
        }
    }

    // Check if a specific mouse button is down
    public static bool GetMouseButton(int buttonIndex)
    {
        if (buttonIndex >= 0 && buttonIndex < mouseButtons.Length)
        {
            return mouseButtons[buttonIndex];
        }
        return false;
    }

    // Reset all inputs (useful when switching scenes or modes)
    public static void Reset()
    {
        simulatedMousePosition = Vector3.zero;
        for (int i = 0; i < mouseButtons.Length; i++)
        {
            mouseButtons[i] = false;
        }
    }
} 