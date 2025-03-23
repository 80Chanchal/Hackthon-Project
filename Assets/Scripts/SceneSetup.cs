using UnityEngine;
using MyWebXR;

public class SceneSetup : MonoBehaviour
{
    [SerializeField] private bool createWhiteboard = true;
    [SerializeField] private bool setupWebXR = true;
    [SerializeField] private bool createRoomEnvironment = true;
    
    [Header("Room Settings")]
    [SerializeField] private float roomSize = 10f;
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
        // Create parent for all environment objects
        GameObject environment = new GameObject("Environment");
        
        // Create floor
        CreateFloor(environment.transform, "Floor", Vector3.zero, new Vector3(roomSize, 0.1f, roomSize), floorColor);
        
        // Create walls
        float halfSize = roomSize / 2f;
        float wallHeight = 3f;
        
        CreateWall(environment.transform, "WallNorth", new Vector3(0, wallHeight / 2, halfSize), new Vector3(roomSize, wallHeight, 0.1f), wallColor);
        CreateWall(environment.transform, "WallSouth", new Vector3(0, wallHeight / 2, -halfSize), new Vector3(roomSize, wallHeight, 0.1f), wallColor);
        CreateWall(environment.transform, "WallEast", new Vector3(halfSize, wallHeight / 2, 0), new Vector3(0.1f, wallHeight, roomSize), wallColor);
        CreateWall(environment.transform, "WallWest", new Vector3(-halfSize, wallHeight / 2, 0), new Vector3(0.1f, wallHeight, roomSize), wallColor);
        
        // Create ceiling if needed
        // CreateWall(environment.transform, "Ceiling", new Vector3(0, wallHeight, 0), new Vector3(roomSize, 0.1f, roomSize), wallColor);
        
        // Add lighting
        if (FindObjectOfType<Light>() == null)
        {
            GameObject lightObj = new GameObject("Directional Light");
            Light light = lightObj.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.0f;
            light.color = Color.white;
            lightObj.transform.rotation = Quaternion.Euler(50f, -30f, 0);
        }
    }

    private void CreateFloor(Transform parent, string name, Vector3 position, Vector3 scale, Color color)
    {
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        floor.name = name;
        floor.transform.SetParent(parent);
        
        floor.transform.localPosition = position;
        floor.transform.localScale = scale;
        
        // Apply material
        Renderer renderer = floor.GetComponent<Renderer>();
        Material material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        material.color = color;
        renderer.material = material;
    }
    
    private void CreateWall(Transform parent, string name, Vector3 position, Vector3 scale, Color color)
    {
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.name = name;
        wall.transform.SetParent(parent);
        
        wall.transform.localPosition = position;
        wall.transform.localScale = scale;
        
        // Apply material
        Renderer renderer = wall.GetComponent<Renderer>();
        Material material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        material.color = color;
        renderer.material = material;
    }
    
    private void CreateWhiteboardSetup()
    {
        if (FindObjectOfType<Whiteboard>() == null)
        {
            // Create whiteboard gameobject
            GameObject whiteboardObj = new GameObject("WhiteboardSetup");
            whiteboardObj.AddComponent<WhiteboardSetup>();
        }
    }
    
    private void SetupWebXR()
    {
        // Create WebXR Manager if not already in the scene
        if (FindObjectOfType<MyWebXR.WebXRManager>() == null)
        {
            GameObject webXRManager = new GameObject("WebXRManager");
            webXRManager.AddComponent<MyWebXR.WebXRManager>();
            
            // Add controllers
            GameObject leftController = new GameObject("LeftController");
            leftController.transform.SetParent(webXRManager.transform);
            MyWebXR.WebXRController leftWebXRController = leftController.AddComponent<MyWebXR.WebXRController>();
            leftWebXRController.controllerHandedness = MyWebXR.WebXRControllerHandedness.LEFT;
            
            GameObject rightController = new GameObject("RightController");
            rightController.transform.SetParent(webXRManager.transform);
            MyWebXR.WebXRController rightWebXRController = rightController.AddComponent<MyWebXR.WebXRController>();
            rightWebXRController.controllerHandedness = MyWebXR.WebXRControllerHandedness.RIGHT;
        }
        
        // Add WebXR settings
        if (FindObjectOfType<WebXRSettings>() == null)
        {
            GameObject webXRSettings = new GameObject("WebXRSettings");
            webXRSettings.AddComponent<WebXRSettings>();
        }
    }
} 