using UnityEngine;

public class DemoScene : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject whiteboardSetupPrefab;
    
    [Header("Room Settings")]
    [SerializeField] private Vector3 roomSize = new Vector3(10, 3, 10);
    [SerializeField] private Material floorMaterial;
    [SerializeField] private Material wallMaterial;
    
    private void Start()
    {
        CreateRoom();
        SetupWhiteboard();
        SetupLighting();
    }
    
    private void CreateRoom()
    {
        // Create floor
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        floor.name = "Floor";
        floor.transform.position = new Vector3(0, -0.5f, 0);
        floor.transform.localScale = new Vector3(roomSize.x, 1, roomSize.z);
        
        // Create walls
        GameObject backWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        backWall.name = "BackWall";
        backWall.transform.position = new Vector3(0, roomSize.y / 2 - 0.5f, roomSize.z / 2);
        backWall.transform.localScale = new Vector3(roomSize.x, roomSize.y, 0.1f);
        
        GameObject leftWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        leftWall.name = "LeftWall";
        leftWall.transform.position = new Vector3(-roomSize.x / 2, roomSize.y / 2 - 0.5f, 0);
        leftWall.transform.localScale = new Vector3(0.1f, roomSize.y, roomSize.z);
        
        GameObject rightWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        rightWall.name = "RightWall";
        rightWall.transform.position = new Vector3(roomSize.x / 2, roomSize.y / 2 - 0.5f, 0);
        rightWall.transform.localScale = new Vector3(0.1f, roomSize.y, roomSize.z);
        
        // Apply materials
        if (floorMaterial != null)
        {
            floor.GetComponent<Renderer>().material = floorMaterial;
        }
        
        if (wallMaterial != null)
        {
            backWall.GetComponent<Renderer>().material = wallMaterial;
            leftWall.GetComponent<Renderer>().material = wallMaterial;
            rightWall.GetComponent<Renderer>().material = wallMaterial;
        }
        
        // Group room objects
        GameObject roomParent = new GameObject("Room");
        floor.transform.parent = roomParent.transform;
        backWall.transform.parent = roomParent.transform;
        leftWall.transform.parent = roomParent.transform;
        rightWall.transform.parent = roomParent.transform;
    }
    
    private void SetupWhiteboard()
    {
        if (whiteboardSetupPrefab != null)
        {
            // Instantiate the whiteboard setup from prefab
            Instantiate(whiteboardSetupPrefab, Vector3.zero, Quaternion.identity);
        }
        else
        {
            // Create whiteboard setup
            GameObject setupObject = new GameObject("WhiteboardSetup");
            WhiteboardSetup setup = setupObject.AddComponent<WhiteboardSetup>();
        }
    }
    
    private void SetupLighting()
    {
        // Add ambient light
        RenderSettings.ambientLight = new Color(0.2f, 0.2f, 0.2f);
        
        // Create main directional light
        GameObject lightObject = new GameObject("DirectionalLight");
        Light light = lightObject.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1.0f;
        light.color = Color.white;
        lightObject.transform.rotation = Quaternion.Euler(50, -30, 0);
        
        // Create fill light
        GameObject fillLightObject = new GameObject("FillLight");
        Light fillLight = fillLightObject.AddComponent<Light>();
        fillLight.type = LightType.Directional;
        fillLight.intensity = 0.3f;
        fillLight.color = new Color(0.8f, 0.9f, 1.0f); // Slightly blue tint
        fillLightObject.transform.rotation = Quaternion.Euler(30, 60, 0);
    }
} 