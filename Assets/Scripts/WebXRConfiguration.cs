using UnityEngine;
using UnityEngine.XR;
#if UNITY_WEBGL
using WebXR;
#endif

public class WebXRConfiguration : MonoBehaviour
{
    [Header("WebXR Settings")]
    [SerializeField] private bool enableVR = true;
    [SerializeField] private bool enableAR = false;
    [SerializeField] private bool autoStartSession = true;
    
    [Header("Integration References")]
    [SerializeField] private MyWebXR.WebXRManager customWebXRManager;
    [SerializeField] private WebXRAdapter webXRAdapter;
    
    private void Awake()
    {
        // Find references if not set
        if (customWebXRManager == null)
        {
            customWebXRManager = FindObjectOfType<MyWebXR.WebXRManager>();
        }
        
        if (webXRAdapter == null)
        {
            webXRAdapter = FindObjectOfType<WebXRAdapter>();
        }
        
#if UNITY_WEBGL
        // Set up official WebXR package
        WebXRManager webXRManager = FindObjectOfType<WebXRManager>();
        
        if (webXRManager != null)
        {
            // Configure WebXR settings
            webXRManager.VRRequiredFeatures = new string[] { "local-floor" };
            webXRManager.VROptionalFeatures = new string[] { "hand-tracking", "hit-test" };
            webXRManager.ARRequiredFeatures = new string[] { "hit-test" };
            webXRManager.AROptionalFeatures = new string[] { "dom-overlay" };
            
            // Set up event listeners to bridge to our custom implementation
            webXRManager.OnXRCapabilitiesUpdate += OnXRCapabilitiesUpdate;
            webXRManager.OnXRChange += OnXRChange;
            webXRManager.OnControllerUpdate += OnControllerUpdate;
        }
        else
        {
            Debug.LogWarning("No WebXRManager found in the scene. Install and configure the WebXR package.");
        }
#else
        Debug.Log("WebXR is only available in WebGL builds. Using fallback implementation for development.");
        SetupFallbackImplementation();
#endif
    }
    
    private void SetupFallbackImplementation()
    {
        if (customWebXRManager != null)
        {
            customWebXRManager.VRSupported = enableVR;
            customWebXRManager.ARSupported = enableAR;
            customWebXRManager.AutoStartSession = autoStartSession;
        }
    }
    
#if UNITY_WEBGL
    private void OnXRCapabilitiesUpdate(WebXRDisplayCapabilities capabilities)
    {
        if (customWebXRManager != null)
        {
            var customCapabilities = new MyWebXR.WebXRDisplayCapabilities
            {
                canPresentVR = capabilities.canPresentVR,
                canPresentAR = capabilities.canPresentAR
            };
            
            // Forward event to our custom implementation
            // This would need to be reflected in our custom implementation to support
        }
    }
    
    private void OnXRChange(WebXRState state, int viewsCount, Rect leftRect, Rect rightRect)
    {
        if (customWebXRManager != null)
        {
            MyWebXR.WebXRState customState = MyWebXR.WebXRState.NORMAL;
            
            // Map WebXR state to our custom state
            switch (state)
            {
                case WebXRState.AR:
                    customState = MyWebXR.WebXRState.AR;
                    break;
                case WebXRState.VR:
                    customState = MyWebXR.WebXRState.VR;
                    break;
                case WebXRState.NORMAL:
                default:
                    customState = MyWebXR.WebXRState.NORMAL;
                    break;
            }
            
            // Forward event to our custom implementation
            // This would need event reflection mechanism on the custom manager
        }
    }
    
    private void OnControllerUpdate(WebXRControllerData controllerData)
    {
        if (customWebXRManager != null)
        {
            // Map controller data to our custom data structure
            var customControllerData = new MyWebXR.WebXRControllerData
            {
                handedness = controllerData.hand == WebXRHandedness.LEFT ? 
                             MyWebXR.WebXRControllerHandedness.LEFT : 
                             MyWebXR.WebXRControllerHandedness.RIGHT,
                position = controllerData.position,
                rotation = controllerData.rotation,
                trigger = controllerData.triggerPressed,
                squeeze = controllerData.squeezePressed,
                buttonA = controllerData.buttonAPressed,
                buttonB = controllerData.buttonBPressed,
                // For X and Y buttons, we can map them from extra buttons in the WebXR controller
                buttonX = controllerData.buttonXPressed,
                buttonY = controllerData.buttonYPressed
            };
            
            // Forward data to our custom implementation
            customWebXRManager.UpdateControllerData(customControllerData);
        }
    }
#endif

    private void OnDestroy()
    {
#if UNITY_WEBGL
        WebXRManager webXRManager = FindObjectOfType<WebXRManager>();
        
        if (webXRManager != null)
        {
            // Remove event listeners
            webXRManager.OnXRCapabilitiesUpdate -= OnXRCapabilitiesUpdate;
            webXRManager.OnXRChange -= OnXRChange;
            webXRManager.OnControllerUpdate -= OnControllerUpdate;
        }
#endif
    }
} 