using UnityEngine;

namespace MyWebXR
{
    public class WebXRManager : MonoBehaviour
    {
        [SerializeField] private bool vrSupported = true;
        [SerializeField] private bool arSupported = false;
        [SerializeField] private bool autoStartSession = true;

        public bool VRSupported 
        { 
            get => vrSupported; 
            set => vrSupported = value; 
        }
        
        public bool ARSupported 
        { 
            get => arSupported; 
            set => arSupported = value; 
        }
        
        public bool AutoStartSession 
        { 
            get => autoStartSession; 
            set => autoStartSession = value; 
        }

        public event XRCapabilitiesUpdate OnXRCapabilitiesUpdate;
        public event XRChange OnXRChange;
        public event ControllerUpdate OnControllerUpdate;

        private WebXRState currentState = WebXRState.NORMAL;
        private WebXRDisplayCapabilities capabilities = new WebXRDisplayCapabilities();

        private void Start()
        {
            // Initialize capabilities
            capabilities.canPresentVR = vrSupported;
            capabilities.canPresentAR = arSupported;
            
            // Notify listeners about capabilities
            OnXRCapabilitiesUpdate?.Invoke(capabilities);
            
            // Auto-start VR session if enabled
            if (autoStartSession && vrSupported)
            {
                StartVRSession();
            }
        }

        public void StartVRSession()
        {
            if (!vrSupported)
            {
                Debug.LogWarning("VR is not supported!");
                return;
            }
            
            currentState = WebXRState.VR;
            OnXRChange?.Invoke(currentState, 2, new Rect(0, 0, 0.5f, 1), new Rect(0.5f, 0, 0.5f, 1));
            
            Debug.Log("Started VR session");
        }

        public void StartARSession()
        {
            if (!arSupported)
            {
                Debug.LogWarning("AR is not supported!");
                return;
            }
            
            currentState = WebXRState.AR;
            OnXRChange?.Invoke(currentState, 1, new Rect(0, 0, 1, 1), new Rect());
            
            Debug.Log("Started AR session");
        }

        public void EndXRSession()
        {
            currentState = WebXRState.NORMAL;
            OnXRChange?.Invoke(currentState, 1, new Rect(0, 0, 1, 1), new Rect());
            
            Debug.Log("Ended XR session");
        }

        // Call this method to simulate controller updates
        public void UpdateControllerData(WebXRControllerData data)
        {
            if (currentState != WebXRState.NORMAL)
            {
                OnControllerUpdate?.Invoke(data);
            }
        }
    }
    
    public enum WebXRState { NORMAL, VR, AR }
    
    public class WebXRDisplayCapabilities
    {
        public bool canPresentVR;
        public bool canPresentAR;
    }
    
    public delegate void XRCapabilitiesUpdate(WebXRDisplayCapabilities capabilities);
    public delegate void XRChange(WebXRState state, int viewsCount, Rect leftRect, Rect rightRect);
    public delegate void ControllerUpdate(WebXRControllerData controllerData);
} 