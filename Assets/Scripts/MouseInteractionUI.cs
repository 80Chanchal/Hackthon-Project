using UnityEngine;
using UnityEngine.UI;

public class MouseInteractionUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private RectTransform mouseIndicatorPanel;
    [SerializeField] private Image leftButtonImage;
    [SerializeField] private Image rightButtonImage;
    
    [Header("Colors")]
    [SerializeField] private Color defaultLeftColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    [SerializeField] private Color activeLeftColor = new Color(0.4f, 0.7f, 1f, 1f);
    [SerializeField] private Color defaultRightColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    [SerializeField] private Color activeRightColor = new Color(1f, 0.5f, 0.5f, 1f);
    
    [Header("Settings")]
    [SerializeField] private float panelWidth = 150f;
    [SerializeField] private float panelHeight = 100f;
    [SerializeField] private bool showMouseIndicator = true;
    [SerializeField] private bool followMouse = false;
    [SerializeField] private Vector2 staticPosition = new Vector2(150f, 100f);
    
    private MouseInteractionHandler interactionHandler;
    
    private void Start()
    {
        // Find the interaction handler
        interactionHandler = FindObjectOfType<MouseInteractionHandler>();
        
        if (interactionHandler == null)
        {
            Debug.LogError("MouseInteractionUI requires a MouseInteractionHandler in the scene.");
            enabled = false;
            return;
        }
        
        // Create UI elements if not assigned
        CreateUIIfNeeded();
        
        // Set up initial colors
        if (leftButtonImage != null)
        {
            leftButtonImage.color = defaultLeftColor;
        }
        
        if (rightButtonImage != null)
        {
            rightButtonImage.color = defaultRightColor;
        }
        
        // Set mouse panel visibility
        if (mouseIndicatorPanel != null)
        {
            mouseIndicatorPanel.gameObject.SetActive(showMouseIndicator);
        }
    }
    
    private void Update()
    {
        if (!showMouseIndicator || mouseIndicatorPanel == null)
            return;
            
        // If set to follow mouse, update position
        if (followMouse)
        {
            Vector2 mousePosition = Input.mousePosition;
            mouseIndicatorPanel.position = new Vector2(mousePosition.x + panelWidth/2 + 20, mousePosition.y);
        }
        else
        {
            // Use static position
            mouseIndicatorPanel.position = staticPosition;
        }
        
        // Update button colors based on mouse input
        if (leftButtonImage != null)
        {
            leftButtonImage.color = Input.GetMouseButton(0) ? activeLeftColor : defaultLeftColor;
        }
        
        if (rightButtonImage != null)
        {
            rightButtonImage.color = Input.GetMouseButton(1) ? activeRightColor : defaultRightColor;
        }
    }
    
    private void CreateUIIfNeeded()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            // Create canvas if one doesn't exist
            GameObject canvasObj = new GameObject("MouseIndicatorCanvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
        }
        
        if (mouseIndicatorPanel == null)
        {
            // Create panel
            GameObject panelObj = new GameObject("MouseIndicatorPanel");
            panelObj.transform.SetParent(canvas.transform, false);
            mouseIndicatorPanel = panelObj.AddComponent<RectTransform>();
            Image panelImage = panelObj.AddComponent<Image>();
            panelImage.color = new Color(0.2f, 0.2f, 0.2f, 0.7f);
            
            // Set size and position
            mouseIndicatorPanel.sizeDelta = new Vector2(panelWidth, panelHeight);
            mouseIndicatorPanel.anchorMin = Vector2.zero;
            mouseIndicatorPanel.anchorMax = Vector2.zero;
            mouseIndicatorPanel.pivot = new Vector2(0.5f, 0.5f);
            mouseIndicatorPanel.position = staticPosition;
            
            // Add a title text
            GameObject titleObj = new GameObject("Title");
            titleObj.transform.SetParent(mouseIndicatorPanel, false);
            RectTransform titleRect = titleObj.AddComponent<RectTransform>();
            Text titleText = titleObj.AddComponent<Text>();
            titleText.text = "Mouse Controls";
            titleText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            titleText.fontSize = 14;
            titleText.alignment = TextAnchor.MiddleCenter;
            titleText.color = Color.white;
            titleRect.sizeDelta = new Vector2(panelWidth, 20);
            titleRect.anchorMin = new Vector2(0, 1);
            titleRect.anchorMax = new Vector2(1, 1);
            titleRect.pivot = new Vector2(0.5f, 1);
            titleRect.anchoredPosition = new Vector2(0, -10);
            
            // Create mouse image
            GameObject mouseObj = new GameObject("MouseImage");
            mouseObj.transform.SetParent(mouseIndicatorPanel, false);
            RectTransform mouseRect = mouseObj.AddComponent<RectTransform>();
            Image mouseImage = mouseObj.AddComponent<Image>();
            mouseImage.color = Color.white;
            mouseRect.sizeDelta = new Vector2(60, 80);
            mouseRect.anchorMin = new Vector2(0.5f, 0.5f);
            mouseRect.anchorMax = new Vector2(0.5f, 0.5f);
            mouseRect.pivot = new Vector2(0.5f, 0.5f);
            mouseRect.anchoredPosition = Vector2.zero;
            
            // Create left button
            GameObject leftObj = new GameObject("LeftButton");
            leftObj.transform.SetParent(mouseRect, false);
            RectTransform leftRect = leftObj.AddComponent<RectTransform>();
            leftButtonImage = leftObj.AddComponent<Image>();
            leftButtonImage.color = defaultLeftColor;
            leftRect.sizeDelta = new Vector2(30, 40);
            leftRect.anchorMin = new Vector2(0, 0.5f);
            leftRect.anchorMax = new Vector2(0.5f, 0.5f);
            leftRect.pivot = new Vector2(0.5f, 0.5f);
            leftRect.anchoredPosition = new Vector2(-8, 0);
            
            // Create right button
            GameObject rightObj = new GameObject("RightButton");
            rightObj.transform.SetParent(mouseRect, false);
            RectTransform rightRect = rightObj.AddComponent<RectTransform>();
            rightButtonImage = rightObj.AddComponent<Image>();
            rightButtonImage.color = defaultRightColor;
            rightRect.sizeDelta = new Vector2(30, 40);
            rightRect.anchorMin = new Vector2(0.5f, 0.5f);
            rightRect.anchorMax = new Vector2(1, 0.5f);
            rightRect.pivot = new Vector2(0.5f, 0.5f);
            rightRect.anchoredPosition = new Vector2(8, 0);
            
            // Left button label
            GameObject leftLabelObj = new GameObject("LeftLabel");
            leftLabelObj.transform.SetParent(mouseIndicatorPanel, false);
            RectTransform leftLabelRect = leftLabelObj.AddComponent<RectTransform>();
            Text leftLabelText = leftLabelObj.AddComponent<Text>();
            leftLabelText.text = "Draw";
            leftLabelText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            leftLabelText.fontSize = 12;
            leftLabelText.alignment = TextAnchor.MiddleCenter;
            leftLabelText.color = Color.white;
            leftLabelRect.sizeDelta = new Vector2(60, 20);
            leftLabelRect.anchorMin = new Vector2(0, 0);
            leftLabelRect.anchorMax = new Vector2(0.5f, 0);
            leftLabelRect.pivot = new Vector2(0.5f, 0);
            leftLabelRect.anchoredPosition = new Vector2(0, 10);
            
            // Right button label
            GameObject rightLabelObj = new GameObject("RightLabel");
            rightLabelObj.transform.SetParent(mouseIndicatorPanel, false);
            RectTransform rightLabelRect = rightLabelObj.AddComponent<RectTransform>();
            Text rightLabelText = rightLabelObj.AddComponent<Text>();
            rightLabelText.text = "Erase";
            rightLabelText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            rightLabelText.fontSize = 12;
            rightLabelText.alignment = TextAnchor.MiddleCenter;
            rightLabelText.color = Color.white;
            rightLabelRect.sizeDelta = new Vector2(60, 20);
            rightLabelRect.anchorMin = new Vector2(0.5f, 0);
            rightLabelRect.anchorMax = new Vector2(1, 0);
            rightLabelRect.pivot = new Vector2(0.5f, 0);
            rightLabelRect.anchoredPosition = new Vector2(0, 10);
        }
    }
    
    public void ToggleVisibility(bool visible)
    {
        showMouseIndicator = visible;
        if (mouseIndicatorPanel != null)
        {
            mouseIndicatorPanel.gameObject.SetActive(showMouseIndicator);
        }
    }
    
    public void ToggleFollowMouse(bool follow)
    {
        followMouse = follow;
    }
} 