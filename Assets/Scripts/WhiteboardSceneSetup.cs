using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;
using TMPro;

public class WhiteboardSceneSetup : MonoBehaviour
{
    [Header("Whiteboard Settings")]
    [SerializeField] private Vector3 whiteboardPosition = new Vector3(0, 1.5f, 2.0f);
    [SerializeField] private Vector3 whiteboardRotation = new Vector3(0, 180, 0);
    [SerializeField] private Vector3 whiteboardScale = new Vector3(2.0f, 1.2f, 0.05f);
    [SerializeField] private Color backgroundColor = Color.white;
    
    [Header("UI Settings")]
    [SerializeField] private Vector3 uiPosition = new Vector3(-1.5f, 1.5f, 2.0f);
    [SerializeField] private Vector3 uiRotation = new Vector3(0, 90, 0);
    [SerializeField] private Vector3 uiScale = new Vector3(0.01f, 0.01f, 0.01f);
    
    [Header("Pen Settings")]
    [SerializeField] private float defaultPenSize = 5.0f;
    [SerializeField] private Color[] penColors = new Color[] 
    { 
        Color.black, 
        Color.red, 
        Color.green, 
        Color.blue, 
        Color.yellow 
    };
    
    private GameObject whiteboardObject;
    private GameObject uiCanvas;
    
    void Start()
    {
        CreateWhiteboard();
        CreateUI();
        ConnectComponents();
    }
    
    private void CreateWhiteboard()
    {
        // Create whiteboard object
        whiteboardObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
        whiteboardObject.name = "Whiteboard";
        whiteboardObject.transform.position = whiteboardPosition;
        whiteboardObject.transform.rotation = Quaternion.Euler(whiteboardRotation);
        whiteboardObject.transform.localScale = whiteboardScale;
        
        // Add Whiteboard component
        Whiteboard whiteboard = whiteboardObject.AddComponent<Whiteboard>();
        WhiteboardController controller = whiteboardObject.AddComponent<WhiteboardController>();
        
        // Setup material
        Renderer renderer = whiteboardObject.GetComponent<Renderer>();
        Material material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        if (material == null)
        {
            // Fallback to standard shader if URP is not available
            material = new Material(Shader.Find("Standard"));
        }
        
        material.color = backgroundColor;
        renderer.material = material;
    }
    
    private void CreateUI()
    {
        // Create UI Canvas
        uiCanvas = new GameObject("WhiteboardUI");
        Canvas canvas = uiCanvas.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        uiCanvas.AddComponent<CanvasScaler>();
        uiCanvas.AddComponent<GraphicRaycaster>();
        
        // Position the canvas
        uiCanvas.transform.position = uiPosition;
        uiCanvas.transform.rotation = Quaternion.Euler(uiRotation);
        uiCanvas.transform.localScale = uiScale;
        
        // Add WhiteboardUI component
        WhiteboardUI whiteboardUI = uiCanvas.AddComponent<WhiteboardUI>();
        
        // Create UI panel background
        GameObject panel = CreateUIPanel(uiCanvas.transform, "Panel", new Vector2(0, 0), new Vector2(400, 600));
        
        // Create Clear Button
        GameObject clearButton = CreateButton(panel.transform, "ClearButton", "Clear", new Vector2(0, 250), new Vector2(200, 50));
        
        // Create Pen Size Controls
        GameObject penSizeLabel = CreateText(panel.transform, "PenSizeLabel", "Pen Size:", new Vector2(0, 180), new Vector2(200, 40));
        
        GameObject decreaseSizeBtn = CreateButton(panel.transform, "DecreaseSizeBtn", "-", new Vector2(-80, 130), new Vector2(50, 50));
        GameObject increaseSizeBtn = CreateButton(panel.transform, "IncreaseSizeBtn", "+", new Vector2(80, 130), new Vector2(50, 50));
        
        GameObject penSizeSlider = CreateSlider(panel.transform, "PenSizeSlider", new Vector2(0, 130), new Vector2(100, 20));
        GameObject penSizeText = CreateText(panel.transform, "PenSizeText", defaultPenSize.ToString("F1"), new Vector2(0, 130), new Vector2(40, 30));
        
        // Create Color Buttons
        GameObject colorsLabel = CreateText(panel.transform, "ColorsLabel", "Colors:", new Vector2(0, 50), new Vector2(200, 40));
        
        GameObject[] colorButtons = new GameObject[penColors.Length];
        float buttonWidth = 350f / penColors.Length;
        
        for (int i = 0; i < penColors.Length; i++)
        {
            float xPos = -175 + (i * buttonWidth) + (buttonWidth / 2);
            colorButtons[i] = CreateColorButton(panel.transform, $"ColorBtn_{i}", penColors[i], new Vector2(xPos, 0), new Vector2(buttonWidth - 10, 40));
        }
        
        // Create color preview
        GameObject colorPreview = CreateColorPreview(panel.transform, "ColorPreview", Color.black, new Vector2(0, -60), new Vector2(100, 40));
        
        // Store references in the WhiteboardUI component
        whiteboardUI.SetupReferences(
            clearButton.GetComponent<Button>(),
            decreaseSizeBtn.GetComponent<Button>(),
            increaseSizeBtn.GetComponent<Button>(),
            penSizeSlider.GetComponent<Slider>(),
            penSizeText.GetComponent<TMP_Text>(),
            colorPreview.GetComponent<Image>(),
            GetButtonArray(colorButtons));
    }
    
    private GameObject CreateUIPanel(Transform parent, string name, Vector2 position, Vector2 size)
    {
        GameObject panel = new GameObject(name);
        panel.transform.SetParent(parent, false);
        
        RectTransform rt = panel.AddComponent<RectTransform>();
        rt.sizeDelta = size;
        rt.anchoredPosition = position;
        
        Image image = panel.AddComponent<Image>();
        image.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);
        
        return panel;
    }
    
    private GameObject CreateButton(Transform parent, string name, string text, Vector2 position, Vector2 size)
    {
        GameObject buttonObj = new GameObject(name);
        buttonObj.transform.SetParent(parent, false);
        
        RectTransform rt = buttonObj.AddComponent<RectTransform>();
        rt.sizeDelta = size;
        rt.anchoredPosition = position;
        
        Image image = buttonObj.AddComponent<Image>();
        image.color = new Color(0.8f, 0.8f, 0.8f, 1f);
        
        Button button = buttonObj.AddComponent<Button>();
        ColorBlock colors = button.colors;
        colors.normalColor = new Color(0.8f, 0.8f, 0.8f);
        colors.highlightedColor = new Color(0.9f, 0.9f, 0.9f);
        colors.pressedColor = new Color(0.7f, 0.7f, 0.7f);
        button.colors = colors;
        
        GameObject textObj = CreateText(buttonObj.transform, "Text", text, Vector2.zero, size);
        textObj.GetComponent<TMP_Text>().color = Color.black;
        
        return buttonObj;
    }
    
    private GameObject CreateText(Transform parent, string name, string text, Vector2 position, Vector2 size)
    {
        GameObject textObj = new GameObject(name);
        textObj.transform.SetParent(parent, false);
        
        RectTransform rt = textObj.AddComponent<RectTransform>();
        rt.sizeDelta = size;
        rt.anchoredPosition = position;
        
        TMP_Text tmpText = textObj.AddComponent<TextMeshProUGUI>();
        tmpText.text = text;
        tmpText.color = Color.white;
        tmpText.fontSize = 24;
        tmpText.alignment = TextAlignmentOptions.Center;
        
        return textObj;
    }
    
    private GameObject CreateSlider(Transform parent, string name, Vector2 position, Vector2 size)
    {
        GameObject sliderObj = new GameObject(name);
        sliderObj.transform.SetParent(parent, false);
        
        RectTransform rt = sliderObj.AddComponent<RectTransform>();
        rt.sizeDelta = size;
        rt.anchoredPosition = position;
        
        // Background
        GameObject background = new GameObject("Background");
        background.transform.SetParent(sliderObj.transform, false);
        RectTransform bgRT = background.AddComponent<RectTransform>();
        bgRT.anchorMin = new Vector2(0, 0.25f);
        bgRT.anchorMax = new Vector2(1, 0.75f);
        bgRT.sizeDelta = Vector2.zero;
        Image bgImage = background.AddComponent<Image>();
        bgImage.color = new Color(0.3f, 0.3f, 0.3f);
        
        // Fill Area
        GameObject fillArea = new GameObject("Fill Area");
        fillArea.transform.SetParent(sliderObj.transform, false);
        RectTransform fillRT = fillArea.AddComponent<RectTransform>();
        fillRT.anchorMin = new Vector2(0, 0.25f);
        fillRT.anchorMax = new Vector2(1, 0.75f);
        fillRT.sizeDelta = new Vector2(-20, 0);
        fillRT.anchoredPosition = new Vector2(-5, 0);
        
        // Fill
        GameObject fill = new GameObject("Fill");
        fill.transform.SetParent(fillArea.transform, false);
        RectTransform fillRectRT = fill.AddComponent<RectTransform>();
        fillRectRT.anchorMin = new Vector2(0, 0);
        fillRectRT.anchorMax = new Vector2(0.5f, 1);
        fillRectRT.sizeDelta = Vector2.zero;
        Image fillImage = fill.AddComponent<Image>();
        fillImage.color = new Color(0.6f, 0.6f, 0.6f);
        
        // Handle Slide Area
        GameObject handleSlideArea = new GameObject("Handle Slide Area");
        handleSlideArea.transform.SetParent(sliderObj.transform, false);
        RectTransform handleSlideRT = handleSlideArea.AddComponent<RectTransform>();
        handleSlideRT.anchorMin = new Vector2(0, 0);
        handleSlideRT.anchorMax = new Vector2(1, 1);
        handleSlideRT.sizeDelta = new Vector2(-20, 0);
        handleSlideRT.anchoredPosition = new Vector2(-10, 0);
        
        // Handle
        GameObject handle = new GameObject("Handle");
        handle.transform.SetParent(handleSlideArea.transform, false);
        RectTransform handleRT = handle.AddComponent<RectTransform>();
        handleRT.anchorMin = new Vector2(0.5f, 0);
        handleRT.anchorMax = new Vector2(0.5f, 1);
        handleRT.sizeDelta = new Vector2(20, 0);
        handleRT.anchoredPosition = Vector2.zero;
        Image handleImage = handle.AddComponent<Image>();
        handleImage.color = new Color(1f, 1f, 1f);
        
        // Slider Component
        Slider slider = sliderObj.AddComponent<Slider>();
        slider.fillRect = fillRectRT;
        slider.handleRect = handleRT;
        slider.targetGraphic = handleImage;
        slider.direction = Slider.Direction.LeftToRight;
        slider.minValue = 1f;
        slider.maxValue = 20f;
        slider.value = defaultPenSize;
        
        return sliderObj;
    }
    
    private GameObject CreateColorButton(Transform parent, string name, Color color, Vector2 position, Vector2 size)
    {
        GameObject buttonObj = new GameObject(name);
        buttonObj.transform.SetParent(parent, false);
        
        RectTransform rt = buttonObj.AddComponent<RectTransform>();
        rt.sizeDelta = size;
        rt.anchoredPosition = position;
        
        Image image = buttonObj.AddComponent<Image>();
        image.color = color;
        
        Button button = buttonObj.AddComponent<Button>();
        ColorBlock colors = button.colors;
        colors.normalColor = color;
        colors.highlightedColor = new Color(
            Mathf.Min(color.r + 0.1f, 1f),
            Mathf.Min(color.g + 0.1f, 1f),
            Mathf.Min(color.b + 0.1f, 1f));
        colors.pressedColor = new Color(
            Mathf.Max(color.r - 0.1f, 0f),
            Mathf.Max(color.g - 0.1f, 0f),
            Mathf.Max(color.b - 0.1f, 0f));
        button.colors = colors;
        
        return buttonObj;
    }
    
    private GameObject CreateColorPreview(Transform parent, string name, Color color, Vector2 position, Vector2 size)
    {
        GameObject previewObj = new GameObject(name);
        previewObj.transform.SetParent(parent, false);
        
        RectTransform rt = previewObj.AddComponent<RectTransform>();
        rt.sizeDelta = size;
        rt.anchoredPosition = position;
        
        Image image = previewObj.AddComponent<Image>();
        image.color = color;
        
        return previewObj;
    }
    
    private Button[] GetButtonArray(GameObject[] buttonObjects)
    {
        Button[] buttons = new Button[buttonObjects.Length];
        for (int i = 0; i < buttonObjects.Length; i++)
        {
            buttons[i] = buttonObjects[i].GetComponent<Button>();
        }
        return buttons;
    }
    
    private void ConnectComponents()
    {
        // Get components
        Whiteboard whiteboard = whiteboardObject.GetComponent<Whiteboard>();
        WhiteboardUI whiteboardUI = uiCanvas.GetComponent<WhiteboardUI>();
        
        // Connect whiteboard to UI
        whiteboardUI.SetWhiteboard(whiteboard);
        
        // Add WebXR settings
        GameObject webXRSetup = new GameObject("WebXRSetup");
        webXRSetup.AddComponent<WebXRSettings>();
        
        // Add WebXR adapter
        webXRSetup.AddComponent<WebXRAdapter>();
    }
} 