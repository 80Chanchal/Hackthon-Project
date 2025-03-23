using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class WhiteboardSetup : MonoBehaviour
{
    [Header("Whiteboard")]
    [SerializeField] private GameObject whiteboardPrefab;
    [SerializeField] private Vector3 whiteboardPosition = new Vector3(0, 1.5f, 2.0f);
    [SerializeField] private Vector3 whiteboardRotation = new Vector3(0, 180, 0);
    [SerializeField] private Vector3 whiteboardScale = new Vector3(2.0f, 1.2f, 0.05f);
    
    [Header("XR Rig")]
    [SerializeField] private GameObject xrRigPrefab;
    [SerializeField] private Vector3 xrRigPosition = new Vector3(0, 0, 0);
    
    [Header("UI")]
    [SerializeField] private GameObject whiteboardUIPrefab;
    
    private GameObject whiteboardInstance;
    private GameObject xrRigInstance;
    private GameObject uiInstance;
    
    private void Start()
    {
        SetupXRRig();
        SetupWhiteboard();
        SetupUI();
        ConnectComponents();
    }
    
    private void SetupXRRig()
    {
        if (xrRigPrefab != null)
        {
            xrRigInstance = Instantiate(xrRigPrefab, xrRigPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("XR Rig prefab not assigned, creating a basic camera");
            
            // Create a simple camera if no XR rig is provided
            var cameraObject = new GameObject("Main Camera");
            cameraObject.tag = "MainCamera";
            cameraObject.AddComponent<Camera>();
            cameraObject.transform.position = new Vector3(0, 1.6f, -2.0f);
            cameraObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            xrRigInstance = cameraObject;
        }
    }
    
    private void SetupWhiteboard()
    {
        if (whiteboardPrefab != null)
        {
            whiteboardInstance = Instantiate(whiteboardPrefab, whiteboardPosition, Quaternion.Euler(whiteboardRotation));
            whiteboardInstance.transform.localScale = whiteboardScale;
        }
        else
        {
            // Create a basic whiteboard if no prefab is provided
            whiteboardInstance = GameObject.CreatePrimitive(PrimitiveType.Quad);
            whiteboardInstance.name = "Whiteboard";
            whiteboardInstance.transform.position = whiteboardPosition;
            whiteboardInstance.transform.rotation = Quaternion.Euler(whiteboardRotation);
            whiteboardInstance.transform.localScale = whiteboardScale;
            
            // Add whiteboard component
            whiteboardInstance.AddComponent<Whiteboard>();
            
            // Setup material
            Renderer renderer = whiteboardInstance.GetComponent<Renderer>();
            renderer.material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            renderer.material.color = Color.white;
        }
    }
    
    private void SetupUI()
    {
        if (whiteboardUIPrefab != null)
        {
            uiInstance = Instantiate(whiteboardUIPrefab);
        }
        else
        {
            // Create a basic UI canvas
            uiInstance = new GameObject("UI Canvas");
            Canvas canvas = uiInstance.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            uiInstance.AddComponent<UnityEngine.UI.CanvasScaler>();
            uiInstance.AddComponent<UnityEngine.UI.GraphicRaycaster>();
            
            // Position the canvas next to the whiteboard
            uiInstance.transform.position = whiteboardPosition + new Vector3(-whiteboardScale.x * 0.75f, 0, 0);
            uiInstance.transform.rotation = Quaternion.Euler(0, 90, 0);
            uiInstance.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            
            // Add UI component
            uiInstance.AddComponent<WhiteboardUI>();
        }
    }
    
    private void ConnectComponents()
    {
        // Get the whiteboard component
        Whiteboard whiteboard = whiteboardInstance.GetComponent<Whiteboard>();
        
        // Get or add the whiteboard controller
        WhiteboardController controller = whiteboardInstance.GetComponent<WhiteboardController>();
        if (controller == null)
        {
            controller = whiteboardInstance.AddComponent<WhiteboardController>();
        }
        
        // Get the UI component
        WhiteboardUI ui = uiInstance.GetComponent<WhiteboardUI>();
        
        // Connect UI to whiteboard
        if (ui != null && whiteboard != null)
        {
            // Use reflection or serialized field to set the whiteboard reference
            var whiteboardField = ui.GetType().GetField("whiteboard", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (whiteboardField != null)
            {
                whiteboardField.SetValue(ui, whiteboard);
            }
        }
        
        // Find XR controllers/interactors
        if (xrRigInstance != null)
        {
            var controllers = xrRigInstance.GetComponentsInChildren<XRController>();
            var rayInteractors = xrRigInstance.GetComponentsInChildren<XRRayInteractor>();
            
            if (rayInteractors.Length > 0 && controller != null)
            {
                // Use reflection or serialized field to set the ray interactor
                var rayInteractorField = controller.GetType().GetField("rayInteractor", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (rayInteractorField != null)
                {
                    rayInteractorField.SetValue(controller, rayInteractors[0]);
                }
            }
        }
    }
} 