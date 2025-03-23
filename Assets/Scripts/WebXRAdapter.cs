using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using MyWebXR;

public class WebXRAdapter : MonoBehaviour
{
    [Header("WebXR Components")]
    [SerializeField] private MyWebXR.WebXRManager webXRManager;
    [SerializeField] private MyWebXR.WebXRController leftWebXRController;
    [SerializeField] private MyWebXR.WebXRController rightWebXRController;
    
    [Header("XR Interaction Toolkit Components")]
    [SerializeField] private Transform xrOrigin; // Changed to Transform to avoid dependency issues
    [SerializeField] private XRController leftXRController;
    [SerializeField] private XRController rightXRController;
    
    [Header("Whiteboard Reference")]
    [SerializeField] private WhiteboardController whiteboardController;
    
    private bool inXRSession = false;
    
    private void Start()
    {
        // Find references if not set
        if (webXRManager == null)
        {
            webXRManager = FindObjectOfType<MyWebXR.WebXRManager>();
        }
        
        if (xrOrigin == null)
        {
            var origin = FindObjectOfType<MonoBehaviour>(); // Find any appropriate object to use as origin
            if (origin != null)
                xrOrigin = origin.transform;
        }
        
        if (leftWebXRController == null || rightWebXRController == null)
        {
            MyWebXR.WebXRController[] controllers = FindObjectsOfType<MyWebXR.WebXRController>();
            foreach (MyWebXR.WebXRController controller in controllers)
            {
                if (controller.HandednessType == MyWebXR.WebXRControllerHandedness.LEFT)
                {
                    leftWebXRController = controller;
                }
                else if (controller.HandednessType == MyWebXR.WebXRControllerHandedness.RIGHT)
                {
                    rightWebXRController = controller;
                }
            }
        }
        
        if (leftXRController == null || rightXRController == null)
        {
            XRController[] controllers = FindObjectsOfType<XRController>();
            foreach (XRController controller in controllers)
            {
                if (controller.controllerNode == XRNode.LeftHand)
                {
                    leftXRController = controller;
                }
                else if (controller.controllerNode == XRNode.RightHand)
                {
                    rightXRController = controller;
                }
            }
        }
        
        if (whiteboardController == null)
        {
            whiteboardController = FindObjectOfType<WhiteboardController>();
        }
        
        // Register WebXR events
        if (webXRManager != null)
        {
            webXRManager.OnXRChange += OnXRChange;
            webXRManager.OnControllerUpdate += OnControllerUpdate;
        }
    }
    
    private void OnDestroy()
    {
        // Unregister WebXR events
        if (webXRManager != null)
        {
            webXRManager.OnXRChange -= OnXRChange;
            webXRManager.OnControllerUpdate -= OnControllerUpdate;
        }
    }
    
    private void OnXRChange(MyWebXR.WebXRState state, int viewsCount, Rect leftRect, Rect rightRect)
    {
        inXRSession = state != MyWebXR.WebXRState.NORMAL;
        
        if (inXRSession)
        {
            // XR mode activated
            EnableXRInteractionToolkit(true);
        }
        else
        {
            // Non-XR mode
            EnableXRInteractionToolkit(false);
        }
    }
    
    private void OnControllerUpdate(MyWebXR.WebXRControllerData controllerData)
    {
        // Map WebXR controller inputs to XR Interaction Toolkit inputs
        if (controllerData.handedness == MyWebXR.WebXRControllerHandedness.LEFT)
        {
            MapControllerInputs(controllerData, leftXRController);
        }
        else if (controllerData.handedness == MyWebXR.WebXRControllerHandedness.RIGHT)
        {
            MapControllerInputs(controllerData, rightXRController);
        }
    }
    
    private void MapControllerInputs(MyWebXR.WebXRControllerData webXRData, XRController xrController)
    {
        if (xrController == null) return;
        
        // Update controller positions and rotations
        xrController.transform.localPosition = webXRData.position;
        xrController.transform.localRotation = webXRData.rotation;
        
        // Map WebXR controller buttons to XR Interaction Toolkit input actions
        if (webXRData.trigger)
        {
            // Left mouse button for drawing
            InputSimulator.SimulateMouseDown(0);
        }
        else
        {
            InputSimulator.SimulateMouseUp(0);
        }
        
        if (webXRData.buttonB || webXRData.buttonY)
        {
            // Right mouse button for erasing
            InputSimulator.SimulateMouseDown(1);
        }
        else
        {
            InputSimulator.SimulateMouseUp(1);
        }
        
        // Update the whiteboard controller with the ray
        if (whiteboardController != null && xrController.GetComponentInChildren<XRRayInteractor>() != null)
        {
            // The WhiteboardController will handle input based on the ray position
        }
    }
    
    private void EnableXRInteractionToolkit(bool enable)
    {
        if (xrOrigin != null)
        {
            xrOrigin.gameObject.SetActive(enable);
        }
        
        if (leftXRController != null)
        {
            leftXRController.gameObject.SetActive(enable);
        }
        
        if (rightXRController != null)
        {
            rightXRController.gameObject.SetActive(enable);
        }
    }
} 