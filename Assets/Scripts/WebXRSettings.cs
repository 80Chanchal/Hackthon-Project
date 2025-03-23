using UnityEngine;
using MyWebXR;

public class WebXRSettings : MonoBehaviour
{
    [Header("XR References")]
    [SerializeField] private MyWebXR.WebXRManager webXRManager;
    [SerializeField] private MyWebXR.WebXRController leftController;
    [SerializeField] private MyWebXR.WebXRController rightController;
    
    [Header("Settings")]
    [SerializeField] private bool enableVR = true;
    [SerializeField] private bool enableAR = false;
    [SerializeField] private bool autoStartSession = true;
    
    private void Start()
    {
        if (webXRManager == null)
        {
            webXRManager = FindObjectOfType<MyWebXR.WebXRManager>();
        }
        
        if (leftController == null || rightController == null)
        {
            MyWebXR.WebXRController[] controllers = FindObjectsOfType<MyWebXR.WebXRController>();
            
            foreach (MyWebXR.WebXRController controller in controllers)
            {
                if (controller.HandednessType == MyWebXR.WebXRControllerHandedness.LEFT)
                {
                    leftController = controller;
                }
                else if (controller.HandednessType == MyWebXR.WebXRControllerHandedness.RIGHT)
                {
                    rightController = controller;
                }
            }
        }
        
        // Apply settings to WebXR Manager
        if (webXRManager != null)
        {
            webXRManager.VRSupported = enableVR;
            webXRManager.ARSupported = enableAR;
            webXRManager.AutoStartSession = autoStartSession;
        }
        
        // Register event listeners
        if (webXRManager != null)
        {
            webXRManager.OnXRCapabilitiesUpdate += OnXRCapabilitiesUpdate;
            webXRManager.OnXRChange += OnXRChange;
            webXRManager.OnControllerUpdate += OnControllerUpdate;
        }
    }
    
    private void OnDestroy()
    {
        // Unregister event listeners
        if (webXRManager != null)
        {
            webXRManager.OnXRCapabilitiesUpdate -= OnXRCapabilitiesUpdate;
            webXRManager.OnXRChange -= OnXRChange;
            webXRManager.OnControllerUpdate -= OnControllerUpdate;
        }
    }
    
    private void OnXRCapabilitiesUpdate(MyWebXR.WebXRDisplayCapabilities capabilities)
    {
        Debug.Log($"XR Capabilities: VR supported: {capabilities.canPresentVR}, AR supported: {capabilities.canPresentAR}");
    }
    
    private void OnXRChange(MyWebXR.WebXRState state, int viewsCount, Rect leftRect, Rect rightRect)
    {
        Debug.Log($"XR State Change: {state}, Views: {viewsCount}");
        
        if (state == MyWebXR.WebXRState.VR)
        {
            // VR mode activated
            Debug.Log("VR mode activated");
        }
        else if (state == MyWebXR.WebXRState.AR)
        {
            // AR mode activated
            Debug.Log("AR mode activated");
        }
        else
        {
            // Non-XR mode
            Debug.Log("Exited XR mode");
        }
    }
    
    private void OnControllerUpdate(MyWebXR.WebXRControllerData controllerData)
    {
        // Handle controller data updates if needed
        if (controllerData.handedness == MyWebXR.WebXRControllerHandedness.LEFT && leftController != null)
        {
            // Update left controller-specific functionality
        }
        else if (controllerData.handedness == MyWebXR.WebXRControllerHandedness.RIGHT && rightController != null)
        {
            // Update right controller-specific functionality
        }
    }
} 