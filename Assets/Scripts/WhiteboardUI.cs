using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WhiteboardUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Whiteboard whiteboard;
    
    [Header("UI Elements")]
    [SerializeField] private Button clearButton;
    [SerializeField] private Button increasePenSizeButton;
    [SerializeField] private Button decreasePenSizeButton;
    [SerializeField] private Slider penSizeSlider;
    [SerializeField] private TMP_Text penSizeText;
    [SerializeField] private Image penColorPreview;
    [SerializeField] private Button[] colorButtons;
    
    [Header("Pen Settings")]
    [SerializeField] private float minPenSize = 1f;
    [SerializeField] private float maxPenSize = 20f;
    [SerializeField] private float penSizeStep = 1f;
    [SerializeField] private Color[] availableColors;
    
    private float currentPenSize = 5f;
    private Color currentPenColor = Color.black;
    
    private void Start()
    {
        if (whiteboard == null)
        {
            Debug.LogWarning("WhiteboardUI has no whiteboard reference. Please assign one or call SetWhiteboard.");
        }
        
        // Initialize UI
        SetupButtons();
        UpdatePenSizeUI();
        UpdateColorUI();
    }
    
    public void SetWhiteboard(Whiteboard board)
    {
        whiteboard = board;
        
        if (whiteboard != null)
        {
            // Initialize whiteboard with current settings
            whiteboard.SetPenSize(currentPenSize);
            whiteboard.SetPenColor(currentPenColor);
        }
    }
    
    public void SetupReferences(Button clear, Button decreaseSize, Button increaseSize, 
                                Slider sizeSlider, TMP_Text sizeText, Image colorPreview, 
                                Button[] colors)
    {
        clearButton = clear;
        decreasePenSizeButton = decreaseSize;
        increasePenSizeButton = increaseSize;
        penSizeSlider = sizeSlider;
        penSizeText = sizeText;
        penColorPreview = colorPreview;
        colorButtons = colors;
        
        // Set available colors from buttons
        if (colorButtons != null && colorButtons.Length > 0)
        {
            availableColors = new Color[colorButtons.Length];
            for (int i = 0; i < colorButtons.Length; i++)
            {
                if (colorButtons[i] != null && colorButtons[i].GetComponent<Image>() != null)
                {
                    availableColors[i] = colorButtons[i].GetComponent<Image>().color;
                }
                else
                {
                    availableColors[i] = Color.white;
                }
            }
        }
        
        // Initialize UI
        SetupButtons();
        UpdatePenSizeUI();
        UpdateColorUI();
    }
    
    private void SetupButtons()
    {
        if (clearButton != null)
        {
            clearButton.onClick.AddListener(ClearWhiteboard);
        }
        
        if (increasePenSizeButton != null)
        {
            increasePenSizeButton.onClick.AddListener(IncreasePenSize);
        }
        
        if (decreasePenSizeButton != null)
        {
            decreasePenSizeButton.onClick.AddListener(DecreasePenSize);
        }
        
        if (penSizeSlider != null)
        {
            penSizeSlider.minValue = minPenSize;
            penSizeSlider.maxValue = maxPenSize;
            penSizeSlider.value = currentPenSize;
            penSizeSlider.onValueChanged.AddListener(OnPenSizeSliderChanged);
        }
        
        // Setup color buttons
        if (colorButtons != null && colorButtons.Length > 0 && availableColors != null && availableColors.Length > 0)
        {
            int colorCount = Mathf.Min(colorButtons.Length, availableColors.Length);
            
            for (int i = 0; i < colorCount; i++)
            {
                Button button = colorButtons[i];
                if (button != null)
                {
                    int colorIndex = i; // Capture variable for closure
                    button.onClick.AddListener(() => SetPenColor(colorIndex));
                }
            }
        }
    }
    
    private void ClearWhiteboard()
    {
        if (whiteboard != null)
        {
            whiteboard.ClearBoard();
        }
    }
    
    private void IncreasePenSize()
    {
        currentPenSize = Mathf.Min(currentPenSize + penSizeStep, maxPenSize);
        UpdatePenSize();
    }
    
    private void DecreasePenSize()
    {
        currentPenSize = Mathf.Max(currentPenSize - penSizeStep, minPenSize);
        UpdatePenSize();
    }
    
    private void OnPenSizeSliderChanged(float newSize)
    {
        currentPenSize = newSize;
        UpdatePenSize();
    }
    
    private void UpdatePenSize()
    {
        // Update whiteboard pen size
        if (whiteboard != null)
        {
            whiteboard.SetPenSize(currentPenSize);
        }
        
        // Update UI
        UpdatePenSizeUI();
    }
    
    private void UpdatePenSizeUI()
    {
        if (penSizeSlider != null)
        {
            penSizeSlider.value = currentPenSize;
        }
        
        if (penSizeText != null)
        {
            penSizeText.text = currentPenSize.ToString("F1");
        }
    }
    
    private void SetPenColor(int colorIndex)
    {
        if (availableColors != null && colorIndex >= 0 && colorIndex < availableColors.Length)
        {
            currentPenColor = availableColors[colorIndex];
            
            if (whiteboard != null)
            {
                whiteboard.SetPenColor(currentPenColor);
            }
            
            UpdateColorUI();
        }
    }
    
    private void UpdateColorUI()
    {
        if (penColorPreview != null)
        {
            penColorPreview.color = currentPenColor;
        }
    }
} 