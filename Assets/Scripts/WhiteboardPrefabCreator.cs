using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
public class WhiteboardPrefabCreator : MonoBehaviour
{
    [MenuItem("GameObject/WebXR/Create Whiteboard Setup")]
    public static void CreateWhiteboardSetup()
    {
        // Create the setup GameObject
        GameObject setupObject = new GameObject("WhiteboardSetup");
        setupObject.AddComponent<WhiteboardSetup>();
        
        // Create the whiteboard
        GameObject whiteboard = GameObject.CreatePrimitive(PrimitiveType.Quad);
        whiteboard.name = "Whiteboard";
        whiteboard.transform.position = new Vector3(0, 1.5f, 2.0f);
        whiteboard.transform.rotation = Quaternion.Euler(0, 180, 0);
        whiteboard.transform.localScale = new Vector3(2.0f, 1.2f, 0.05f);
        whiteboard.AddComponent<Whiteboard>();
        whiteboard.AddComponent<WhiteboardController>();
        
        // Create basic material
        Renderer renderer = whiteboard.GetComponent<Renderer>();
        if (renderer != null)
        {
            Material material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            material.color = Color.white;
            renderer.material = material;
        }
        
        // Create UI canvas
        GameObject uiCanvas = new GameObject("WhiteboardUI");
        Canvas canvas = uiCanvas.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        uiCanvas.AddComponent<UnityEngine.UI.CanvasScaler>();
        uiCanvas.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        
        // Position the canvas
        uiCanvas.transform.position = new Vector3(-1.5f, 1.5f, 2.0f);
        uiCanvas.transform.rotation = Quaternion.Euler(0, 90, 0);
        uiCanvas.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        
        // Add WhiteboardUI component
        WhiteboardUI whiteboardUI = uiCanvas.AddComponent<WhiteboardUI>();
        
        // Create a basic button for clearing the board
        GameObject clearButton = new GameObject("ClearButton");
        clearButton.transform.SetParent(uiCanvas.transform, false);
        
        UnityEngine.UI.Button button = clearButton.AddComponent<UnityEngine.UI.Button>();
        UnityEngine.UI.Image buttonImage = clearButton.AddComponent<UnityEngine.UI.Image>();
        buttonImage.color = new Color(0.8f, 0.8f, 0.8f);
        
        // Add text to button
        GameObject buttonText = new GameObject("Text");
        buttonText.transform.SetParent(clearButton.transform, false);
        
        TMPro.TextMeshProUGUI tmpText = buttonText.AddComponent<TMPro.TextMeshProUGUI>();
        tmpText.text = "Clear";
        tmpText.color = Color.black;
        tmpText.alignment = TMPro.TextAlignmentOptions.Center;
        tmpText.fontSize = 24;
        
        // Position and size button
        RectTransform clearButtonRT = clearButton.GetComponent<RectTransform>();
        clearButtonRT.sizeDelta = new Vector2(160, 50);
        clearButtonRT.anchoredPosition = new Vector2(0, 0);
        
        // Position text
        RectTransform textRT = buttonText.GetComponent<RectTransform>();
        textRT.sizeDelta = new Vector2(160, 50);
        textRT.anchoredPosition = Vector2.zero;
        
        // Connect components
        System.Type whiteboardUIType = typeof(WhiteboardUI);
        var clearButtonField = whiteboardUIType.GetField("clearButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (clearButtonField != null)
        {
            clearButtonField.SetValue(whiteboardUI, button);
        }
        
        var whiteboardField = whiteboardUIType.GetField("whiteboard", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (whiteboardField != null)
        {
            whiteboardField.SetValue(whiteboardUI, whiteboard.GetComponent<Whiteboard>());
        }
        
        // Create the prefab if it doesn't exist
        if (!AssetDatabase.IsValidFolder("Assets/Prefabs"))
        {
            AssetDatabase.CreateFolder("Assets", "Prefabs");
        }
        
        // Create the prefab
        setupObject.transform.position = Vector3.zero;
        setupObject.transform.rotation = Quaternion.identity;
        setupObject.transform.localScale = Vector3.one;
        
        // Make whiteboard and UI children of the setup
        whiteboard.transform.parent = setupObject.transform;
        uiCanvas.transform.parent = setupObject.transform;
        
        // Save the prefab
        string prefabPath = "Assets/Prefabs/WhiteboardSetup.prefab";
        bool prefabExists = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath) != null;
        
        if (prefabExists)
        {
            // Replace existing prefab
            PrefabUtility.SaveAsPrefabAsset(setupObject, prefabPath);
            Debug.Log("Updated WhiteboardSetup prefab at " + prefabPath);
        }
        else
        {
            // Create new prefab
            PrefabUtility.SaveAsPrefabAsset(setupObject, prefabPath);
            Debug.Log("Created WhiteboardSetup prefab at " + prefabPath);
        }
        
        // Clean up the scene instance
        DestroyImmediate(setupObject);
        
        // Open the prefab
        GameObject prefabAsset = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        if (prefabAsset != null)
        {
            Selection.activeObject = prefabAsset;
        }
    }
}
#endif 