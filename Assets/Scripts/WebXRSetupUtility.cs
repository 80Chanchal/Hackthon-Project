using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
public class WebXRSetupUtility : EditorWindow
{
    private bool setupWebXRCamera = true;
    private bool setupControllers = true;
    private bool setupWhiteboard = true;

    [MenuItem("WebXR/Setup WebXR Scene")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(WebXRSetupUtility), false, "WebXR Setup");
    }

    void OnGUI()
    {
        GUILayout.Label("WebXR Whiteboard Setup", EditorStyles.boldLabel);
        
        EditorGUILayout.Space();
        
        setupWebXRCamera = EditorGUILayout.Toggle("Setup WebXR Camera", setupWebXRCamera);
        setupControllers = EditorGUILayout.Toggle("Setup Controllers", setupControllers);
        setupWhiteboard = EditorGUILayout.Toggle("Setup Whiteboard", setupWhiteboard);
        
        EditorGUILayout.Space();
        
        if (GUILayout.Button("Create WebXR Setup"))
        {
            CreateWebXRSetup();
        }
        
        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("This will set up the necessary components for WebXR in your current scene.", MessageType.Info);
    }
    
    private void CreateWebXRSetup()
    {
        GameObject webXRRoot = new GameObject("WebXRRoot");
        
        // Setup WebXR Camera if needed
        if (setupWebXRCamera)
        {
            // Create WebXR camera rig
            GameObject cameraRig = new GameObject("WebXRCameraRig");
            cameraRig.transform.SetParent(webXRRoot.transform);
            
            // Add the WebXR official components
            if (TypeExists("WebXR.WebXRManager"))
            {
                var webXRManagerType = System.Type.GetType("WebXR.WebXRManager, Unity.WebXR");
                if (webXRManagerType != null)
                {
                    cameraRig.AddComponent(webXRManagerType);
                }
                else
                {
                    Debug.LogWarning("WebXRManager type found but couldn't be instantiated. Make sure WebXR package is properly installed.");
                }
            }
            else
            {
                Debug.LogWarning("WebXR.WebXRManager type not found. Make sure WebXR package is installed.");
            }
            
            // Add camera
            GameObject camera = new GameObject("MainCamera");
            camera.transform.SetParent(cameraRig.transform);
            camera.tag = "MainCamera";
            Camera cameraComponent = camera.AddComponent<Camera>();
            cameraComponent.nearClipPlane = 0.01f;
            cameraComponent.clearFlags = CameraClearFlags.SolidColor;
            cameraComponent.backgroundColor = Color.black;
            
            // Add our custom WebXR configuration script
            cameraRig.AddComponent<WebXRConfiguration>();
        }
        
        // Setup controllers if needed
        if (setupControllers)
        {
            GameObject controllersRoot = new GameObject("Controllers");
            controllersRoot.transform.SetParent(webXRRoot.transform);
            
            // Create left controller
            GameObject leftController = new GameObject("LeftController");
            leftController.transform.SetParent(controllersRoot.transform);
            SetupController(leftController, MyWebXR.WebXRControllerHandedness.LEFT);
            
            // Create right controller
            GameObject rightController = new GameObject("RightController");
            rightController.transform.SetParent(controllersRoot.transform);
            SetupController(rightController, MyWebXR.WebXRControllerHandedness.RIGHT);
            
            // Add WebXR adapter to bridge between WebXR and our custom implementation
            controllersRoot.AddComponent<WebXRAdapter>();
        }
        
        // Setup whiteboard if needed
        if (setupWhiteboard)
        {
            GameObject whiteboardRoot = new GameObject("WhiteboardSetup");
            whiteboardRoot.transform.SetParent(webXRRoot.transform);
            whiteboardRoot.transform.position = new Vector3(0, 1.2f, 2f); // Position in front of user
            
            // Add whiteboard setup component
            whiteboardRoot.AddComponent<WhiteboardSetup>();
        }
        
        Debug.Log("WebXR setup created successfully!");
    }
    
    private void SetupController(GameObject controller, MyWebXR.WebXRControllerHandedness handedness)
    {
        // Add visual representation (primitive cube for now)
        GameObject visual = GameObject.CreatePrimitive(PrimitiveType.Cube);
        visual.transform.SetParent(controller.transform);
        visual.transform.localScale = new Vector3(0.05f, 0.05f, 0.15f);
        visual.transform.localPosition = new Vector3(0, 0, 0.075f);
        
        // Add our custom controller component
        MyWebXR.WebXRController controllerComponent = controller.AddComponent<MyWebXR.WebXRController>();
        controllerComponent.controllerHandedness = handedness;
        
        // Add XR ray interactor components
        if (TypeExists("UnityEngine.XR.Interaction.Toolkit.XRRayInteractor"))
        {
            var interactorType = System.Type.GetType("UnityEngine.XR.Interaction.Toolkit.XRRayInteractor, Unity.XR.Interaction.Toolkit");
            if (interactorType != null)
            {
                controller.AddComponent(interactorType);
            }
        }
    }
    
    private bool TypeExists(string typeName)
    {
        var type = System.Type.GetType(typeName);
        return type != null;
    }
}
#endif 