using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

#if UNITY_EDITOR
[ExecuteInEditMode]
public class WhiteboardSetupGuide : MonoBehaviour
{
    [Header("Required Components")]
    [SerializeField] private GameObject whiteboardPrefab;
    [SerializeField] private GameObject whiteboardUIPrefab;
    
    [Header("Setup Options")]
    [SerializeField] private bool createNewWhiteboard = true;
    [SerializeField] private bool createCanvas = true;
    [SerializeField] private bool createInteractionHandler = true;
    
    [Header("Placement Settings")]
    [SerializeField] private Vector3 whiteboardPosition = new Vector3(0, 1.5f, 2f);
    [SerializeField] private Vector3 whiteboardRotation = Vector3.zero;
    [SerializeField] private Vector3 whiteboardScale = new Vector3(2f, 1.5f, 0.05f);
    
    [Header("References")]
    public WhiteboardDrawing whiteboardDrawing;
    public WhiteboardUIController uiController;
    public MouseInteractionHandler interactionHandler;
    public MouseInteractionUI mouseUI;
    
    public void SetupWhiteboardSystem()
    {
        // 1. Create interaction handler if needed
        if (createInteractionHandler || interactionHandler == null)
        {
            GameObject handlerObj = new GameObject("MouseInteractionHandler");
            interactionHandler = handlerObj.AddComponent<MouseInteractionHandler>();
            Debug.Log("Created MouseInteractionHandler");
        }
        
        // 2. Create whiteboard if needed
        if (createNewWhiteboard || whiteboardDrawing == null)
        {
            GameObject whiteboardObj;
            
            if (whiteboardPrefab != null)
            {
                whiteboardObj = PrefabUtility.InstantiatePrefab(whiteboardPrefab) as GameObject;
            }
            else
            {
                whiteboardObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                whiteboardObj.name = "Whiteboard";
                
                // Create a new material for the whiteboard
                Material whiteboardMat = new Material(Shader.Find("Standard"));
                whiteboardMat.name = "WhiteboardMaterial";
                whiteboardMat.SetFloat("_Glossiness", 0.1f);
                whiteboardMat.SetFloat("_Metallic", 0.0f);
                whiteboardMat.color = Color.white;
                
                whiteboardObj.GetComponent<Renderer>().material = whiteboardMat;
            }
            
            // Position the whiteboard
            whiteboardObj.transform.position = whiteboardPosition;
            whiteboardObj.transform.eulerAngles = whiteboardRotation;
            whiteboardObj.transform.localScale = whiteboardScale;
            
            // Add WhiteboardDrawing component if not present
            if (whiteboardObj.GetComponent<WhiteboardDrawing>() == null)
            {
                whiteboardDrawing = whiteboardObj.AddComponent<WhiteboardDrawing>();
            }
            else
            {
                whiteboardDrawing = whiteboardObj.GetComponent<WhiteboardDrawing>();
            }
            
            Debug.Log("Created Whiteboard");
        }
        
        // 3. Create Canvas and UI if needed
        Canvas canvas = FindObjectOfType<Canvas>();
        if (createCanvas && canvas == null)
        {
            GameObject canvasObj = new GameObject("Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();
            canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();
            Debug.Log("Created Canvas");
        }
        
        if (canvas != null && (uiController == null || createCanvas))
        {
            GameObject uiObj;
            
            if (whiteboardUIPrefab != null)
            {
                uiObj = PrefabUtility.InstantiatePrefab(whiteboardUIPrefab, canvas.transform) as GameObject;
            }
            else
            {
                uiObj = new GameObject("WhiteboardUI");
                uiObj.transform.SetParent(canvas.transform, false);
                uiController = uiObj.AddComponent<WhiteboardUIController>();
                
                // Create a basic UI structure if using a blank object
                CreateBasicUIElements(uiObj.transform);
            }
            
            if (uiObj.GetComponent<WhiteboardUIController>() != null)
            {
                uiController = uiObj.GetComponent<WhiteboardUIController>();
            }
            
            Debug.Log("Created WhiteboardUI");
        }
        
        // 4. Set up MouseInteractionUI if needed
        if (mouseUI == null)
        {
            GameObject mouseUIObj = new GameObject("MouseInteractionUI");
            
            if (canvas != null)
            {
                mouseUIObj.transform.SetParent(canvas.transform, false);
            }
            
            mouseUI = mouseUIObj.AddComponent<MouseInteractionUI>();
            Debug.Log("Created MouseInteractionUI");
        }
        
        // 5. Connect all components
        ConnectComponents();
        
        Debug.Log("Whiteboard system setup complete!");
    }
    
    private void CreateBasicUIElements(Transform parentTransform)
    {
        // Create a simple UI with buttons and sliders
        
        // Panel background
        GameObject panelObj = new GameObject("ControlPanel");
        panelObj.transform.SetParent(parentTransform, false);
        RectTransform panelRect = panelObj.AddComponent<RectTransform>();
        UnityEngine.UI.Image panelImage = panelObj.AddComponent<UnityEngine.UI.Image>();
        panelImage.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);
        
        panelRect.anchorMin = new Vector2(0, 0);
        panelRect.anchorMax = new Vector2(0, 1);
        panelRect.pivot = new Vector2(0, 0.5f);
        panelRect.sizeDelta = new Vector2(100, -20);
        panelRect.anchoredPosition = new Vector2(10, 0);
        
        // Clear Button
        GameObject clearBtn = CreateButton(panelRect, "ClearButton", "Clear", new Vector2(0, 100));
        
        // Save Button
        GameObject saveBtn = CreateButton(panelRect, "SaveButton", "Save", new Vector2(0, 60));
        
        // Draw Mode Button
        GameObject drawBtn = CreateButton(panelRect, "DrawButton", "Draw", new Vector2(0, 20));
        
        // Erase Mode Button
        GameObject eraseBtn = CreateButton(panelRect, "EraseButton", "Erase", new Vector2(0, -20));
        
        // Create color buttons
        CreateColorButtons(panelRect);
        
        // Create save confirmation text
        GameObject confirmObj = new GameObject("SaveConfirmation");
        confirmObj.transform.SetParent(parentTransform, false);
        RectTransform confirmRect = confirmObj.AddComponent<RectTransform>();
        UnityEngine.UI.Text confirmText = confirmObj.AddComponent<UnityEngine.UI.Text>();
        
        confirmRect.anchorMin = new Vector2(0.5f, 1);
        confirmRect.anchorMax = new Vector2(0.5f, 1);
        confirmRect.pivot = new Vector2(0.5f, 1);
        confirmRect.sizeDelta = new Vector2(400, 40);
        confirmRect.anchoredPosition = new Vector2(0, -50);
        
        confirmText.text = "Drawing Saved!";
        confirmText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        confirmText.fontSize = 18;
        confirmText.color = Color.green;
        confirmText.alignment = TextAnchor.MiddleCenter;
        
        confirmObj.SetActive(false);
    }
    
    private GameObject CreateButton(RectTransform parent, string name, string text, Vector2 position)
    {
        GameObject btnObj = new GameObject(name);
        btnObj.transform.SetParent(parent, false);
        RectTransform btnRect = btnObj.AddComponent<RectTransform>();
        UnityEngine.UI.Image btnImage = btnObj.AddComponent<UnityEngine.UI.Image>();
        UnityEngine.UI.Button btn = btnObj.AddComponent<UnityEngine.UI.Button>();
        
        btnRect.anchorMin = new Vector2(0.5f, 0.5f);
        btnRect.anchorMax = new Vector2(0.5f, 0.5f);
        btnRect.pivot = new Vector2(0.5f, 0.5f);
        btnRect.sizeDelta = new Vector2(80, 30);
        btnRect.anchoredPosition = position;
        
        btnImage.color = new Color(0.8f, 0.8f, 0.8f);
        
        // Add text
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(btnRect, false);
        RectTransform textRect = textObj.AddComponent<RectTransform>();
        UnityEngine.UI.Text textComp = textObj.AddComponent<UnityEngine.UI.Text>();
        
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;
        
        textComp.text = text;
        textComp.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        textComp.fontSize = 14;
        textComp.color = Color.black;
        textComp.alignment = TextAnchor.MiddleCenter;
        
        return btnObj;
    }
    
    private void CreateColorButtons(RectTransform parent)
    {
        Color[] colors = new Color[] {
            Color.black,
            Color.white,
            Color.red,
            Color.green,
            Color.blue,
            Color.yellow
        };
        
        GameObject colorsPanel = new GameObject("ColorsPanel");
        colorsPanel.transform.SetParent(parent, false);
        RectTransform colorsRect = colorsPanel.AddComponent<RectTransform>();
        
        colorsRect.anchorMin = new Vector2(0.5f, 0);
        colorsRect.anchorMax = new Vector2(0.5f, 0);
        colorsRect.pivot = new Vector2(0.5f, 0);
        colorsRect.sizeDelta = new Vector2(80, 120);
        colorsRect.anchoredPosition = new Vector2(0, 10);
        
        for (int i = 0; i < colors.Length; i++)
        {
            int row = i / 2;
            int col = i % 2;
            
            GameObject colorBtn = new GameObject("ColorButton" + i);
            colorBtn.transform.SetParent(colorsRect, false);
            RectTransform btnRect = colorBtn.AddComponent<RectTransform>();
            UnityEngine.UI.Image btnImage = colorBtn.AddComponent<UnityEngine.UI.Image>();
            UnityEngine.UI.Button btn = colorBtn.AddComponent<UnityEngine.UI.Button>();
            
            btnRect.anchorMin = new Vector2(0, 0);
            btnRect.anchorMax = new Vector2(1, 1);
            btnRect.pivot = new Vector2(0.5f, 0.5f);
            btnRect.sizeDelta = new Vector2(-40, -100);
            btnRect.anchoredPosition = new Vector2(col * 40 - 20, row * 40 + 20);
            
            btnImage.color = colors[i];
            
            // Add outline for white color button
            if (colors[i] == Color.white)
            {
                GameObject outline = new GameObject("Outline");
                outline.transform.SetParent(btnRect, false);
                RectTransform outlineRect = outline.AddComponent<RectTransform>();
                UnityEngine.UI.Image outlineImage = outline.AddComponent<UnityEngine.UI.Image>();
                
                outlineRect.anchorMin = Vector2.zero;
                outlineRect.anchorMax = Vector2.one;
                outlineRect.sizeDelta = new Vector2(4, 4);
                
                outlineImage.color = Color.black;
            }
        }
    }
    
    private void ConnectComponents()
    {
        if (whiteboardDrawing != null && interactionHandler != null)
        {
            // Connect whiteboard to interaction handler
            SerializedObject serializedWhiteboard = new SerializedObject(whiteboardDrawing);
            SerializedProperty interactionHandlerProp = serializedWhiteboard.FindProperty("interactionHandler");
            interactionHandlerProp.objectReferenceValue = interactionHandler;
            serializedWhiteboard.ApplyModifiedProperties();
            Debug.Log("Connected whiteboard to interaction handler");
        }
        
        if (interactionHandler != null && mouseUI != null)
        {
            // Connect interaction handler to mouseUI
            SerializedObject serializedHandler = new SerializedObject(interactionHandler);
            SerializedProperty uiControllerProp = serializedHandler.FindProperty("uiController");
            uiControllerProp.objectReferenceValue = mouseUI;
            serializedHandler.ApplyModifiedProperties();
            Debug.Log("Connected interaction handler to mouse UI");
        }
        
        if (uiController != null)
        {
            // Connect UI controller to all components
            SerializedObject serializedUI = new SerializedObject(uiController);
            
            if (whiteboardDrawing != null)
            {
                SerializedProperty whiteboardProp = serializedUI.FindProperty("whiteboardDrawing");
                whiteboardProp.objectReferenceValue = whiteboardDrawing;
            }
            
            if (interactionHandler != null)
            {
                SerializedProperty handlerProp = serializedUI.FindProperty("interactionHandler");
                handlerProp.objectReferenceValue = interactionHandler;
            }
            
            if (mouseUI != null)
            {
                SerializedProperty mouseUIProp = serializedUI.FindProperty("mouseUI");
                mouseUIProp.objectReferenceValue = mouseUI;
            }
            
            serializedUI.ApplyModifiedProperties();
            Debug.Log("Connected UI controller to components");
        }
    }
}

[CustomEditor(typeof(WhiteboardSetupGuide))]
public class WhiteboardSetupGuideEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        WhiteboardSetupGuide setupGuide = (WhiteboardSetupGuide)target;
        
        EditorGUILayout.Space();
        
        if (GUILayout.Button("Setup Whiteboard System", GUILayout.Height(30)))
        {
            setupGuide.SetupWhiteboardSystem();
        }
        
        EditorGUILayout.HelpBox(
            "This will create and connect all necessary components for the whiteboard system.\n\n" +
            "1. Add this component to any GameObject\n" +
            "2. Configure the options above\n" +
            "3. Click 'Setup Whiteboard System'\n\n" +
            "You can then delete this component after setup is complete.",
            MessageType.Info);
    }
}
#endif 