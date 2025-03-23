using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;

public class Whiteboard : MonoBehaviour
{
    [Header("Whiteboard Settings")]
    [SerializeField] private Transform boardTransform;
    [SerializeField] private int textureWidth = 2048;
    [SerializeField] private int textureHeight = 1024;
    [SerializeField] private float penSize = 5.0f;
    [SerializeField] private Color penColor = Color.black;
    [SerializeField] private Color eraserColor = Color.white;

    private Texture2D boardTexture;
    private Color[] pixels;
    private Renderer boardRenderer;
    private RaycastHit hit;

    private bool isDrawing = false;
    private bool isErasing = false;
    private Vector2 lastUV;

    private void Start()
    {
        if (boardTransform == null)
        {
            boardTransform = transform;
        }

        boardRenderer = GetComponent<Renderer>();

        // Create a blank whiteboard texture
        InitializeTexture();
    }

    private void InitializeTexture()
    {
        boardTexture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);
        pixels = new Color[textureWidth * textureHeight];

        // Fill with white
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.white;
        }

        boardTexture.SetPixels(pixels);
        boardTexture.Apply();

        // Assign the texture to the material
        Material material = boardRenderer.material;
        material.mainTexture = boardTexture;
    }

    private void Update()
    {
        // Mouse input for testing in non-VR mode and for teachers to use
        if (Input.GetMouseButtonDown(0)) // Left click
        {
            isDrawing = true;
            isErasing = false;
            HandleBoardInteraction();
        }
        else if (Input.GetMouseButtonDown(1)) // Right click
        {
            isDrawing = false;
            isErasing = true;
            HandleBoardInteraction();
        }
        else if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            isDrawing = false;
            isErasing = false;
        }
        else if (isDrawing || isErasing)
        {
            HandleBoardInteraction();
        }
    }

    private void HandleBoardInteraction()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit) && hit.transform == boardTransform)
        {
            Vector2 uv = hit.textureCoord;
            int x = (int)(uv.x * textureWidth);
            int y = (int)(uv.y * textureHeight);

            if (isDrawing || isErasing)
            {
                // If this is not the first point in the stroke
                if (lastUV != Vector2.zero && Vector2.Distance(lastUV, uv) < 0.1f)
                {
                    DrawLine(lastUV, uv, isErasing ? eraserColor : penColor);
                }
                else
                {
                    DrawDot(x, y, isErasing ? eraserColor : penColor);
                }
                
                lastUV = uv;
                boardTexture.Apply();
            }
        }
    }

    private void DrawDot(int x, int y, Color color)
    {
        int radius = (int)penSize;
        for (int i = -radius; i <= radius; i++)
        {
            for (int j = -radius; j <= radius; j++)
            {
                if (i * i + j * j <= radius * radius)
                {
                    int drawX = x + i;
                    int drawY = y + j;
                    
                    if (drawX >= 0 && drawX < textureWidth && drawY >= 0 && drawY < textureHeight)
                    {
                        boardTexture.SetPixel(drawX, drawY, color);
                    }
                }
            }
        }
    }

    private void DrawLine(Vector2 startUV, Vector2 endUV, Color color)
    {
        int startX = (int)(startUV.x * textureWidth);
        int startY = (int)(startUV.y * textureHeight);
        int endX = (int)(endUV.x * textureWidth);
        int endY = (int)(endUV.y * textureHeight);
        
        int steps = Mathf.Max(Mathf.Abs(endX - startX), Mathf.Abs(endY - startY));
        steps = Mathf.Max(steps, 1);
        
        for (int i = 0; i <= steps; i++)
        {
            float t = (float)i / steps;
            int x = Mathf.RoundToInt(Mathf.Lerp(startX, endX, t));
            int y = Mathf.RoundToInt(Mathf.Lerp(startY, endY, t));
            
            DrawDot(x, y, color);
        }
    }

    public void ClearBoard()
    {
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.white;
        }
        
        boardTexture.SetPixels(pixels);
        boardTexture.Apply();
    }
    
    public void SetPenSize(float size)
    {
        penSize = Mathf.Max(1f, size);
    }
    
    public void SetPenColor(Color color)
    {
        penColor = color;
    }
} 