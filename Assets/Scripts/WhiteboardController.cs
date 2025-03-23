using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class WhiteboardController : MonoBehaviour
{
    [Header("XR References")]
    [SerializeField] private XRNode controllerNode = XRNode.RightHand;
    [SerializeField] private XRRayInteractor rayInteractor;
    [SerializeField] private Whiteboard whiteboard;
    [SerializeField] private float raycastDistance = 10f;

    private bool triggerPressed = false;
    private bool secondaryButtonPressed = false;
    private InputDevice targetDevice;
    private bool deviceInitialized = false;

    private void Start()
    {
        if (whiteboard == null)
        {
            whiteboard = FindObjectOfType<Whiteboard>();
        }

        if (rayInteractor == null)
        {
            rayInteractor = GetComponentInChildren<XRRayInteractor>();
            if (rayInteractor == null)
            {
                Debug.LogWarning("WhiteboardController requires an XRRayInteractor. Automatically creating one.");
                
                // Create a ray interactor if needed
                GameObject rayInteractorObject = new GameObject("RayInteractor");
                rayInteractorObject.transform.SetParent(transform);
                rayInteractorObject.transform.localPosition = Vector3.zero;
                rayInteractorObject.transform.localRotation = Quaternion.identity;
                
                rayInteractor = rayInteractorObject.AddComponent<XRRayInteractor>();
            }
        }
    }

    private void Update()
    {
        if (!deviceInitialized)
        {
            InitializeDevice();
        }

        if (deviceInitialized)
        {
            CheckControllerInput();
        }
    }

    private void InitializeDevice()
    {
        targetDevice = InputDevices.GetDeviceAtXRNode(controllerNode);
        if (targetDevice.isValid)
        {
            deviceInitialized = true;
            Debug.Log($"Controller initialized: {targetDevice.name}");
        }
    }

    private void CheckControllerInput()
    {
        // Check trigger button (for drawing)
        targetDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerValue);
        
        // Check secondary button (often B or Y) for erasing
        targetDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryValue);

        bool triggerJustPressed = !triggerPressed && triggerValue;
        bool triggerJustReleased = triggerPressed && !triggerValue;
        
        bool secondaryJustPressed = !secondaryButtonPressed && secondaryValue;
        bool secondaryJustReleased = secondaryButtonPressed && !secondaryValue;

        // Handle drawing
        if (triggerJustPressed)
        {
            // Simulate left mouse down for drawing
            InputSimulator.SimulateMouseDown(0);
        }
        else if (triggerJustReleased)
        {
            // Simulate left mouse up
            InputSimulator.SimulateMouseUp(0);
        }

        // Handle erasing
        if (secondaryJustPressed)
        {
            // Simulate right mouse down for erasing
            InputSimulator.SimulateMouseDown(1);
        }
        else if (secondaryJustReleased)
        {
            // Simulate right mouse up
            InputSimulator.SimulateMouseUp(1);
        }

        // Update button states
        triggerPressed = triggerValue;
        secondaryButtonPressed = secondaryValue;

        // Update the simulated mouse position based on ray
        UpdateSimulatedMousePosition();
    }

    private void UpdateSimulatedMousePosition()
    {
        if (rayInteractor != null && whiteboard != null)
        {
            if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            {
                if (hit.transform == whiteboard.transform)
                {
                    // Convert the world position to screen position
                    Vector3 screenPoint = Camera.main.WorldToScreenPoint(hit.point);
                    InputSimulator.SimulateMousePosition(screenPoint);
                }
            }
        }
    }
} 