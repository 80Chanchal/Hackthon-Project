using UnityEngine;

namespace MyWebXR
{
    public class WebXRController : MonoBehaviour
    {
        [SerializeField] public WebXRControllerHandedness controllerHandedness = WebXRControllerHandedness.RIGHT;
        [SerializeField] private WebXRManager webXRManager;
        
        [Header("Visual References")]
        [SerializeField] private Transform visualTransform;
        
        [Header("Button Debug")]
        [SerializeField] private bool debugMode = false;
        [SerializeField] private bool trigger = false;
        [SerializeField] private bool squeeze = false;
        [SerializeField] private bool buttonA = false;
        [SerializeField] private bool buttonB = false;
        [SerializeField] private bool buttonX = false;
        [SerializeField] private bool buttonY = false;

        private WebXRControllerData controllerData = new WebXRControllerData();
        
        // Renamed property to avoid conflict with the field
        public WebXRControllerHandedness HandednessType => controllerHandedness;
        
        private void Start()
        {
            if (webXRManager == null)
            {
                webXRManager = FindObjectOfType<WebXRManager>();
            }
            
            if (visualTransform == null)
            {
                visualTransform = transform;
            }
            
            controllerData.handedness = this.controllerHandedness;
        }
        
        private void Update()
        {
            if (debugMode && webXRManager != null)
            {
                // Update controller data from debug values
                controllerData.position = transform.localPosition;
                controllerData.rotation = transform.localRotation;
                controllerData.trigger = trigger;
                controllerData.squeeze = squeeze;
                controllerData.buttonA = buttonA;
                controllerData.buttonB = buttonB;
                controllerData.buttonX = buttonX;
                controllerData.buttonY = buttonY;
                
                // Send the data to the WebXRManager
                webXRManager.UpdateControllerData(controllerData);
            }
        }
        
        // Method to update controller data from external sources (e.g., native WebXR API)
        public void UpdateControllerData(Vector3 position, Quaternion rotation, 
                                         bool triggerValue, bool squeezeValue,
                                         bool buttonAValue, bool buttonBValue, 
                                         bool buttonXValue, bool buttonYValue)
        {
            // Update the controller's visual representation
            if (visualTransform != null)
            {
                visualTransform.localPosition = position;
                visualTransform.localRotation = rotation;
            }
            
            // Update controller data
            controllerData.position = position;
            controllerData.rotation = rotation;
            controllerData.trigger = triggerValue;
            controllerData.squeeze = squeezeValue;
            controllerData.buttonA = buttonAValue;
            controllerData.buttonB = buttonBValue;
            controllerData.buttonX = buttonXValue;
            controllerData.buttonY = buttonYValue;
            
            // Send the data to the WebXRManager
            if (webXRManager != null)
            {
                webXRManager.UpdateControllerData(controllerData);
            }
        }
    }
    
    public enum WebXRControllerHandedness { LEFT, RIGHT }
    
    [System.Serializable]
    public class WebXRControllerData
    {
        public WebXRControllerHandedness handedness;
        public Vector3 position;
        public Quaternion rotation;
        public bool trigger;
        public bool squeeze;
        public bool buttonA;
        public bool buttonB;
        public bool buttonX;
        public bool buttonY;
    }
} 