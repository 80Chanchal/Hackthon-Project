using UnityEngine;

public class SceneSetup : MonoBehaviour
{
    [SerializeField] private bool createWhiteboard = true;
    [SerializeField] private bool setupWebXR = true;
    [SerializeField] private bool createRoomEnvironment = true;
    
    [Header("Room Settings")]
    [SerializeField] private Vector3 roomSize = new Vector3(10, 3, 10);
    [SerializeField] private Color floorColor = new Color(0.3f, 0.3f, 0.3f);
    [SerializeField] private Color wallColor = new Color(0.8f, 0.8f, 0.8f);
    
    void Start()
    {
        if (createRoomEnvironment)
        {
            SetupEnvironment();
        }
        
        if (createWhiteboard)
        {
            CreateWhiteboardSetup();
        }
        
        if (setupWebXR)
        {
            SetupWebXR();
        }
    }
    
    private void SetupEnvironment()
    {
        GameObject environmentParent = new GameObject("Environment");
        
        // Create floor
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        floor.name = "Floor";
        floor.transform.SetParent(environmentParent.transform);
        floor.transform.position = new Vector3(0, -0.5f, 0);
        floor.transform.localScale = new Vector3(roomSize.x, 1, roomSize.z);
        
        // Set floor material
        Renderer floorRenderer = floor.GetComponent<Renderer>();
        floorRenderer.material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        if (floorRenderer.material != null)
        {
            floorRenderer.material.color = floorColor;
        }
        
        // Create walls
        GameObject wallsParent = new GameObject("Walls");
        wallsParent.transform.SetParent(environmentParent.transform);
        
        // Back wall (behind whiteboard)
        CreateWall(wallsParent.transform, "BackWall", 
                  new Vector3(0, roomSize.y / 2 - 0.5f, roomSize.z / 2), 
                  new Vector3(roomSize.x, roomSize.y, 0.1f), 
                  wallColor);
        
        // Left wall
        CreateWall(wallsParent.transform, "LeftWall", 
                  new Vector3(-roomSize.x / 2, roomSize.y / 2 - 0.5f, 0), 
                  new Vector3(0.1f, roomSize.y, roomSize.z), 
                  wallColor);
        
        // Right wall
        CreateWall(wallsParent.transform, "RightWall", 
                  new Vector3(roomSize.x / 2, roomSize.y / 2 - 0.5f, 0), 
                  new Vector3(0.1f, roomSize.y, roomSize.z), 
                  wallColor);
        
        // Front wall (optional, can be removed for more open space)
        /*
        CreateWall(wallsParent.transform, "FrontWall", 
                  new Vector3(0, roomSize.y / 2 - 0.5f, -roomSize.z / 2), 
                  new Vector3(roomSize.x, roomSize.y, 0.1f), 
                  wallColor);
        */
        
        // Create ceiling (optional)
        /*
        GameObject ceiling = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ceiling.name = "Ceiling";
        ceiling.transform.SetParent(environmentParent.transform);
        ceiling.transform.position = new Vector3(0, roomSize.y - 0.5f, 0);
        ceiling.transform.localScale = new Vector3(roomSize.x, 0.1f, roomSize.z);
        
        Renderer ceilingRenderer = ceiling.GetComponent<Renderer>();
        ceilingRenderer.material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        if (ceilingRenderer.material != null)
        {
            ceilingRenderer.material.color = new Color(0.9f, 0.9f, 0.9f);
        }
        */
    }
    
    private void CreateWall(Transform parent, string name, Vector3 position, Vector3 scale, Color color)
    {
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.name = name;
        wall.transform.SetParent(parent);
        wall.transform.position = position;
        wall.transform.localScale = scale;
        
        Renderer wallRenderer = wall.GetComponent<Renderer>();
        wallRenderer.material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        if (wallRenderer.material != null)
        {
            wallRenderer.material.color = color;
        }
    }
    
    private void CreateWhiteboardSetup()
    {
        GameObject whiteboardSetupObj = new GameObject("WhiteboardSetup");
        WhiteboardSceneSetup whiteboardSetup = whiteboardSetupObj.AddComponent<WhiteboardSceneSetup>();
    }
    
    private void SetupWebXR()
    {
        // Create WebXR Manager if not already in the scene
        if (FindObjectOfType<WebXR.WebXRManager>() == null)
        {
            GameObject webXRManager = new GameObject("WebXRManager");
            webXRManager.AddComponent<WebXR.WebXRManager>();
            
            // Add controllers
            GameObject leftController = new GameObject("LeftController");
            leftController.transform.SetParent(webXRManager.transform);
            WebXR.WebXRController leftWebXRController = leftController.AddComponent<WebXR.WebXRController>();
            leftWebXRController.controllerHandedness = WebXR.WebXRControllerHandedness.LEFT;
            
            GameObject rightController = new GameObject("RightController");
            rightController.transform.SetParent(webXRManager.transform);
            WebXR.WebXRController rightWebXRController = rightController.AddComponent<WebXR.WebXRController>();
            rightWebXRController.controllerHandedness = WebXR.WebXRControllerHandedness.RIGHT;
        }
        
        // Add WebXR settings
        GameObject webXRSettings = new GameObject("WebXRSettings");
        webXRSettings.AddComponent<WebXRSettings>();
        
        // Add WebXR Adapter
        webXRSettings.AddComponent<WebXRAdapter>();
    }
} 