using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshRenderer))]
public class WhiteboardDrawing : MonoBehaviour
{
    [Header("Drawing Settings")]
    [SerializeField] private Color drawColor = Color.black;
    [SerializeField] private int textureSize = 2048;
    [SerializeField] private float brushSize = 10f;
    [SerializeField] private float eraserSize = 30f;
    [SerializeField] private Texture2D brushTexture;
    
    [Header("Components")]
    [SerializeField] private MouseInteractionHandler interactionHandler;
    
    // Private variables
    private Texture2D drawingTexture;
    private Color[] clearColors;
    private MeshRenderer meshRenderer;
    private bool initialized = false;
    
    private void Awake()
    {
        // Get the required components
        meshRenderer = GetComponent<MeshRenderer>();
        
        // Create the drawing texture
        CreateDrawingTexture();
        
        // Register for mouse handler events if available
        if (interactionHandler != null)
        {
            RegisterForMouseEvents();
        }
    }
    
    private void Start()
    {
        // If interaction handler wasn't assigned in Inspector, try to find it
        if (interactionHandler == null)
        {
            interactionHandler = FindObjectOfType<MouseInteractionHandler>();
            
            if (interactionHandler != null)
            {
                RegisterForMouseEvents();
            }
            else
            {
                Debug.LogWarning("WhiteboardDrawing could not find a MouseInteractionHandler. Drawing won't work until one is available.");
            }
        }
    }
    
    private void OnDestroy()
    {
        // Unregister from mouse handler events
        if (interactionHandler != null)
        {
            interactionHandler.OnDrawStart -= OnDrawStart;
            interactionHandler.OnDrawMove -= OnDrawMove;
            interactionHandler.OnDrawEnd -= OnDrawEnd;
            interactionHandler.OnErase -= OnErase;
        }
    }
    
    private void CreateDrawingTexture()
    {
        // Create a new texture with the specified size
        drawingTexture = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);
        
        // Create the clear colors array (transparent white)
        clearColors = new Color[textureSize * textureSize];
        for (int i = 0; i < clearColors.Length; i++)
        {
            clearColors[i] = Color.clear;
        }
        
        // Apply the clear colors
        drawingTexture.SetPixels(clearColors);
        drawingTexture.Apply();
        
        // Create a default brush texture if none is assigned
        if (brushTexture == null)
        {
            CreateDefaultBrushTexture();
        }
        
        // Apply the texture to the renderer
        meshRenderer.material.mainTexture = drawingTexture;
        
        initialized = true;
    }
    
    private void CreateDefaultBrushTexture()
    {
        // Create a simple circular brush
        int brushTextureSize = 64;
        brushTexture = new Texture2D(brushTextureSize, brushTextureSize, TextureFormat.RGBA32, false);
        
        Color[] brushColors = new Color[brushTextureSize * brushTextureSize];
        for (int y = 0; y < brushTextureSize; y++)
        {
            for (int x = 0; x < brushTextureSize; x++)
            {
                float distX = x - brushTextureSize / 2;
                float distY = y - brushTextureSize / 2;
                float dist = Mathf.Sqrt(distX * distX + distY * distY);
                float alpha = 1.0f - Mathf.Clamp01(dist / (brushTextureSize / 2));
                
                brushColors[y * brushTextureSize + x] = new Color(1, 1, 1, alpha * alpha);
            }
        }
        
        brushTexture.SetPixels(brushColors);
        brushTexture.Apply();
    }
    
    private void RegisterForMouseEvents()
    {
        interactionHandler.OnDrawStart += OnDrawStart;
        interactionHandler.OnDrawMove += OnDrawMove;
        interactionHandler.OnDrawEnd += OnDrawEnd;
        interactionHandler.OnErase += OnErase;
    }
    
    private void OnDrawStart(Vector3 position, float radius)
    {
        // Convert world position to UV coordinates
        Vector2 uv = WorldPosToUV(position);
        
        // Draw a dot at the position
        DrawTexture(uv, brushSize * radius * textureSize);
    }
    
    private void OnDrawMove(Vector3 lastPosition, Vector3 currentPosition, float radius)
    {
        // Convert world positions to UV coordinates
        Vector2 lastUV = WorldPosToUV(lastPosition);
        Vector2 currentUV = WorldPosToUV(currentPosition);
        
        // Draw a line between the positions
        DrawLine(lastUV, currentUV, brushSize * radius * textureSize);
    }
    
    private void OnDrawEnd()
    {
        // Nothing special to do on draw end for now
    }
    
    private void OnErase(Vector3 position, float radius)
    {
        // Convert world position to UV coordinates
        Vector2 uv = WorldPosToUV(position);
        
        // Erase at the position
        EraseTexture(uv, eraserSize * radius * textureSize);
    }
    
    private Vector2 WorldPosToUV(Vector3 worldPos)
    {
        // Convert from world position to local position
        Vector3 localPos = transform.InverseTransformPoint(worldPos);
        
        // Convert from local position to UV coordinates (assuming the mesh uses standard UV mapping)
        // By default, the UV origin (0,0) is at the bottom-left corner
        return new Vector2(localPos.x + 0.5f, localPos.z + 0.5f);
    }
    
    private void DrawTexture(Vector2 uv, float size)
    {
        if (!initialized) return;
        
        // Calculate the pixel coordinates
        int x = Mathf.FloorToInt(uv.x * textureSize);
        int y = Mathf.FloorToInt(uv.y * textureSize);
        
        // Calculate the brush size
        int brushRadius = Mathf.CeilToInt(size / 2);
        
        // Get the brush texture pixels to use for alpha
        int brushTexSize = brushTexture.width;
        Color[] brushColors = brushTexture.GetPixels();
        
        // Apply the brush to the texture
        for (int i = -brushRadius; i < brushRadius; i++)
        {
            for (int j = -brushRadius; j < brushRadius; j++)
            {
                // Calculate the texture coordinates
                int texX = x + i;
                int texY = y + j;
                
                // Skip if outside texture bounds
                if (texX < 0 || texX >= textureSize || texY < 0 || texY >= textureSize)
                    continue;
                
                // Calculate normalized position in the brush
                float normX = (i + brushRadius) / (float)(brushRadius * 2);
                float normY = (j + brushRadius) / (float)(brushRadius * 2);
                
                // Get the corresponding pixel from the brush texture
                int brushX = Mathf.FloorToInt(normX * brushTexSize);
                int brushY = Mathf.FloorToInt(normY * brushTexSize);
                brushX = Mathf.Clamp(brushX, 0, brushTexSize - 1);
                brushY = Mathf.Clamp(brushY, 0, brushTexSize - 1);
                
                Color brushPixel = brushColors[brushY * brushTexSize + brushX];
                
                // Get the existing color
                Color existingColor = drawingTexture.GetPixel(texX, texY);
                
                // Blend the colors
                Color newColor = Color.Lerp(existingColor, drawColor, brushPixel.a);
                
                // Apply the new color
                drawingTexture.SetPixel(texX, texY, newColor);
            }
        }
        
        // Apply the changes to the texture
        drawingTexture.Apply();
    }
    
    private void DrawLine(Vector2 from, Vector2 to, float size)
    {
        if (!initialized) return;
        
        // Calculate the distance between the points
        float distance = Vector2.Distance(from, to);
        
        // Calculate the number of steps based on the distance
        int steps = Mathf.Max(1, Mathf.CeilToInt(distance * textureSize));
        
        // Draw dots along the line
        for (int i = 0; i <= steps; i++)
        {
            float t = (float)i / steps;
            Vector2 point = Vector2.Lerp(from, to, t);
            DrawTexture(point, size);
        }
    }
    
    private void EraseTexture(Vector2 uv, float size)
    {
        if (!initialized) return;
        
        // Calculate the pixel coordinates
        int x = Mathf.FloorToInt(uv.x * textureSize);
        int y = Mathf.FloorToInt(uv.y * textureSize);
        
        // Calculate the eraser size
        int eraserRadius = Mathf.CeilToInt(size / 2);
        
        // Get the brush texture pixels to use for alpha
        int brushTexSize = brushTexture.width;
        Color[] brushColors = brushTexture.GetPixels();
        
        // Apply the eraser to the texture
        for (int i = -eraserRadius; i < eraserRadius; i++)
        {
            for (int j = -eraserRadius; j < eraserRadius; j++)
            {
                // Calculate the texture coordinates
                int texX = x + i;
                int texY = y + j;
                
                // Skip if outside texture bounds
                if (texX < 0 || texX >= textureSize || texY < 0 || texY >= textureSize)
                    continue;
                
                // Calculate normalized position in the brush
                float normX = (i + eraserRadius) / (float)(eraserRadius * 2);
                float normY = (j + eraserRadius) / (float)(eraserRadius * 2);
                
                // Get the corresponding pixel from the brush texture
                int brushX = Mathf.FloorToInt(normX * brushTexSize);
                int brushY = Mathf.FloorToInt(normY * brushTexSize);
                brushX = Mathf.Clamp(brushX, 0, brushTexSize - 1);
                brushY = Mathf.Clamp(brushY, 0, brushTexSize - 1);
                
                Color brushPixel = brushColors[brushY * brushTexSize + brushX];
                
                // Get the existing color
                Color existingColor = drawingTexture.GetPixel(texX, texY);
                
                // Decrease the alpha of the existing color
                Color newColor = existingColor;
                newColor.a = Mathf.Max(0, newColor.a - brushPixel.a);
                
                // Apply the new color
                drawingTexture.SetPixel(texX, texY, newColor);
            }
        }
        
        // Apply the changes to the texture
        drawingTexture.Apply();
    }
    
    public void ClearDrawing()
    {
        if (!initialized) return;
        
        // Reset the texture to clear
        drawingTexture.SetPixels(clearColors);
        drawingTexture.Apply();
    }
    
    public void SetDrawColor(Color color)
    {
        drawColor = color;
    }
    
    public Color GetDrawColor()
    {
        return drawColor;
    }
    
    public void SetBrushSize(float size)
    {
        brushSize = Mathf.Max(1f, size);
    }
    
    public float GetBrushSize()
    {
        return brushSize;
    }
    
    public void SetEraserSize(float size)
    {
        eraserSize = Mathf.Max(1f, size);
    }
    
    public float GetEraserSize()
    {
        return eraserSize;
    }
    
    public Texture2D GetDrawingTexture()
    {
        return drawingTexture;
    }
    
    public void SaveDrawing(string filePath)
    {
        if (!initialized) return;
        
        // Encode the texture to PNG
        byte[] bytes = drawingTexture.EncodeToPNG();
        
        // Save the bytes to a file
        System.IO.File.WriteAllBytes(filePath, bytes);
        
        Debug.Log("Drawing saved to: " + filePath);
    }
    
    public void LoadDrawing(string filePath)
    {
        if (!initialized) return;
        
        // Check if the file exists
        if (!System.IO.File.Exists(filePath))
        {
            Debug.LogError("File does not exist: " + filePath);
            return;
        }
        
        // Load the bytes from the file
        byte[] bytes = System.IO.File.ReadAllBytes(filePath);
        
        // Load the texture from the bytes
        Texture2D loadedTexture = new Texture2D(2, 2);
        if (loadedTexture.LoadImage(bytes))
        {
            // Resize the loaded texture if needed
            if (loadedTexture.width != textureSize || loadedTexture.height != textureSize)
            {
                // Create a temporary render texture for scaling
                RenderTexture renderTexture = RenderTexture.GetTemporary(textureSize, textureSize, 0, RenderTextureFormat.ARGB32);
                
                // Copy and scale the loaded texture to the render texture
                Graphics.Blit(loadedTexture, renderTexture);
                
                // Save the current active render texture
                RenderTexture previousActiveRT = RenderTexture.active;
                
                // Set the render texture as active
                RenderTexture.active = renderTexture;
                
                // Create a new texture for the resized image
                Texture2D resizedTexture = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);
                
                // Copy the render texture to the new texture
                resizedTexture.ReadPixels(new Rect(0, 0, textureSize, textureSize), 0, 0);
                resizedTexture.Apply();
                
                // Restore the previous active render texture
                RenderTexture.active = previousActiveRT;
                
                // Release the temporary render texture
                RenderTexture.ReleaseTemporary(renderTexture);
                
                // Copy the pixels from the resized texture to the drawing texture
                drawingTexture.SetPixels(resizedTexture.GetPixels());
                drawingTexture.Apply();
                
                // Clean up
                Destroy(resizedTexture);
            }
            else
            {
                // Copy the pixels directly
                drawingTexture.SetPixels(loadedTexture.GetPixels());
                drawingTexture.Apply();
            }
            
            Debug.Log("Drawing loaded from: " + filePath);
        }
        else
        {
            Debug.LogError("Failed to load image from: " + filePath);
        }
        
        // Clean up
        Destroy(loadedTexture);
    }
} 