using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class WhiteboardUIController : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private WhiteboardDrawing whiteboardDrawing;
    [SerializeField] private MouseInteractionHandler interactionHandler;
    [SerializeField] private MouseInteractionUI mouseUI;
    
    [Header("UI Elements")]
    [SerializeField] private Button clearButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button[] colorButtons;
    [SerializeField] private Slider brushSizeSlider;
    [SerializeField] private Slider eraserSizeSlider;
    [SerializeField] private Toggle mouseUIToggle;
    [SerializeField] private Toggle debugToggle;
    
    [Header("Color Palette")]
    [SerializeField] private Color[] drawColors = new Color[] {
        Color.black,
        Color.white,
        Color.red,
        Color.green,
        Color.blue,
        Color.yellow,
        new Color(1f, 0.5f, 0f), // Orange
        new Color(0.5f, 0f, 0.5f)  // Purple
    };
    
    private string saveFolder;
    
    private void Start()
    {
        // Find references if not assigned
        if (whiteboardDrawing == null)
        {
            whiteboardDrawing = FindObjectOfType<WhiteboardDrawing>();
        }
        
        if (interactionHandler == null)
        {
            interactionHandler = FindObjectOfType<MouseInteractionHandler>();
        }
        
        if (mouseUI == null)
        {
            mouseUI = FindObjectOfType<MouseInteractionUI>();
        }
        
        // Set up save folder
        saveFolder = Path.Combine(Application.persistentDataPath, "Drawings");
        if (!Directory.Exists(saveFolder))
        {
            Directory.CreateDirectory(saveFolder);
        }
        
        // Set up UI buttons
        SetupUICallbacks();
        
        // Initialize color buttons
        SetupColorButtons();
        
        // Initialize with default color
        if (whiteboardDrawing != null && drawColors.Length > 0)
        {
            whiteboardDrawing.SetDrawColor(drawColors[0]);
        }
    }
    
    private void SetupUICallbacks()
    {
        // Clear button
        if (clearButton != null)
        {
            clearButton.onClick.AddListener(ClearDrawing);
        }
        
        // Save button
        if (saveButton != null)
        {
            saveButton.onClick.AddListener(SaveDrawing);
        }
        
        // Brush size slider
        if (brushSizeSlider != null && whiteboardDrawing != null)
        {
            brushSizeSlider.value = whiteboardDrawing.GetBrushSize();
            brushSizeSlider.onValueChanged.AddListener(SetBrushSize);
        }
        
        // Eraser size slider
        if (eraserSizeSlider != null && whiteboardDrawing != null)
        {
            eraserSizeSlider.value = whiteboardDrawing.GetEraserSize();
            eraserSizeSlider.onValueChanged.AddListener(SetEraserSize);
        }
        
        // Mouse UI toggle
        if (mouseUIToggle != null && mouseUI != null)
        {
            mouseUIToggle.isOn = true;
            mouseUIToggle.onValueChanged.AddListener(mouseUI.ToggleVisibility);
        }
        
        // Debug toggle
        if (debugToggle != null && interactionHandler != null)
        {
            debugToggle.isOn = false;
            debugToggle.onValueChanged.AddListener(interactionHandler.ToggleDebugDraw);
        }
    }
    
    private void SetupColorButtons()
    {
        if (colorButtons == null || colorButtons.Length == 0 || whiteboardDrawing == null)
            return;
        
        int numColors = Mathf.Min(colorButtons.Length, drawColors.Length);
        
        for (int i = 0; i < numColors; i++)
        {
            Button button = colorButtons[i];
            Color color = drawColors[i];
            
            // Set button color
            Image buttonImage = button.GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.color = color;
            }
            
            // Set button callback
            int colorIndex = i; // Capture index for lambda
            button.onClick.AddListener(() => SelectColor(colorIndex));
        }
    }
    
    private void SelectColor(int colorIndex)
    {
        if (whiteboardDrawing == null || colorIndex < 0 || colorIndex >= drawColors.Length)
            return;
        
        whiteboardDrawing.SetDrawColor(drawColors[colorIndex]);
        
        // Switch to draw mode
        if (interactionHandler != null)
        {
            interactionHandler.SetDrawMode();
        }
    }
    
    private void ClearDrawing()
    {
        if (whiteboardDrawing != null)
        {
            whiteboardDrawing.ClearDrawing();
        }
    }
    
    private void SaveDrawing()
    {
        if (whiteboardDrawing != null)
        {
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string filename = Path.Combine(saveFolder, $"Drawing_{timestamp}.png");
            whiteboardDrawing.SaveDrawing(filename);
            
            // Show a confirmation message
            Debug.Log($"Drawing saved to {filename}");
            ShowSaveConfirmation(filename);
        }
    }
    
    private void ShowSaveConfirmation(string path)
    {
        // Find the save confirmation text if it exists
        Text saveConfirmText = transform.Find("SaveConfirmation")?.GetComponent<Text>();
        if (saveConfirmText != null)
        {
            saveConfirmText.text = $"Saved to: {Path.GetFileName(path)}";
            saveConfirmText.gameObject.SetActive(true);
            
            // Hide after a delay
            Invoke("HideSaveConfirmation", 3f);
        }
    }
    
    private void HideSaveConfirmation()
    {
        Text saveConfirmText = transform.Find("SaveConfirmation")?.GetComponent<Text>();
        if (saveConfirmText != null)
        {
            saveConfirmText.gameObject.SetActive(false);
        }
    }
    
    private void SetBrushSize(float size)
    {
        if (whiteboardDrawing != null)
        {
            whiteboardDrawing.SetBrushSize(size);
        }
    }
    
    private void SetEraserSize(float size)
    {
        if (whiteboardDrawing != null)
        {
            whiteboardDrawing.SetEraserSize(size);
        }
    }
    
    public void SwitchToDrawMode()
    {
        if (interactionHandler != null)
        {
            interactionHandler.SetDrawMode();
        }
    }
    
    public void SwitchToEraseMode()
    {
        if (interactionHandler != null)
        {
            interactionHandler.SetEraseMode();
        }
    }
    
    public void ToggleMouseUI(bool visible)
    {
        if (mouseUI != null)
        {
            mouseUI.ToggleVisibility(visible);
        }
    }
    
    public void ToggleDebugVisuals(bool visible)
    {
        if (interactionHandler != null)
        {
            interactionHandler.ToggleDebugDraw(visible);
        }
    }
} 