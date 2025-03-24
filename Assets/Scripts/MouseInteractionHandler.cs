using UnityEngine;
using System;

public class MouseInteractionHandler : MonoBehaviour
{
    [Header("Modes")]
    [SerializeField] private bool drawMode = true;
    [SerializeField] private bool eraseMode = false;
    
    [Header("Settings")]
    [SerializeField] private float drawRadius = 0.01f;
    [SerializeField] private float eraseRadius = 0.03f;
    [SerializeField] private bool debugDraw = false;
    
    [Header("Components")]
    [SerializeField] private MouseInteractionUI uiController;
    [SerializeField] private Camera mainCamera;
    
    // Events that other components can subscribe to
    public event Action<Vector3, float> OnDrawStart;
    public event Action<Vector3, Vector3, float> OnDrawMove;
    public event Action OnDrawEnd;
    public event Action<Vector3, float> OnErase;
    
    // Internal variables
    private bool isDrawing = false;
    private Vector3 lastDrawPosition;
    
    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogError("MouseInteractionHandler requires a camera reference.");
                enabled = false;
                return;
            }
        }
        
        // Find UI controller if not assigned
        if (uiController == null)
        {
            uiController = FindObjectOfType<MouseInteractionUI>();
        }
    }
    
    private void Update()
    {
        // Process mode toggles
        if (Input.GetKeyDown(KeyCode.D))
        {
            SetDrawMode();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            SetEraseMode();
        }
        
        // Process mouse interactions
        Vector3 hitPosition;
        if (GetMouseWorldPosition(out hitPosition))
        {
            // Left click (draw)
            if (Input.GetMouseButtonDown(0) && drawMode)
            {
                StartDrawing(hitPosition);
            }
            else if (Input.GetMouseButton(0) && isDrawing)
            {
                ContinueDrawing(hitPosition);
            }
            else if (Input.GetMouseButtonUp(0) && isDrawing)
            {
                StopDrawing();
            }
            
            // Right click (erase)
            if (Input.GetMouseButton(1) && eraseMode)
            {
                EraseAtPosition(hitPosition);
            }
            
            // Debug visuals
            if (debugDraw)
            {
                if (drawMode)
                {
                    Debug.DrawLine(mainCamera.transform.position, hitPosition, Color.blue);
                    DebugDrawCircle(hitPosition, drawRadius, Color.blue);
                }
                
                if (eraseMode)
                {
                    Debug.DrawLine(mainCamera.transform.position, hitPosition, Color.red);
                    DebugDrawCircle(hitPosition, eraseRadius, Color.red);
                }
            }
        }
    }
    
    private bool GetMouseWorldPosition(out Vector3 hitPosition)
    {
        hitPosition = Vector3.zero;
        
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit))
        {
            hitPosition = hit.point;
            return true;
        }
        
        return false;
    }
    
    private void StartDrawing(Vector3 position)
    {
        isDrawing = true;
        lastDrawPosition = position;
        
        // Invoke the draw start event for listeners
        if (OnDrawStart != null)
        {
            OnDrawStart(position, drawRadius);
        }
    }
    
    private void ContinueDrawing(Vector3 currentPosition)
    {
        if (Vector3.Distance(lastDrawPosition, currentPosition) > drawRadius * 0.5f)
        {
            // Invoke the draw move event for listeners
            if (OnDrawMove != null)
            {
                OnDrawMove(lastDrawPosition, currentPosition, drawRadius);
            }
            
            lastDrawPosition = currentPosition;
        }
    }
    
    private void StopDrawing()
    {
        isDrawing = false;
        
        // Invoke the draw end event for listeners
        if (OnDrawEnd != null)
        {
            OnDrawEnd();
        }
    }
    
    private void EraseAtPosition(Vector3 position)
    {
        // Invoke the erase event for listeners
        if (OnErase != null)
        {
            OnErase(position, eraseRadius);
        }
    }
    
    public void SetDrawMode()
    {
        drawMode = true;
        eraseMode = false;
    }
    
    public void SetEraseMode()
    {
        drawMode = false;
        eraseMode = true;
    }
    
    public void ToggleDebugDraw(bool enable)
    {
        debugDraw = enable;
    }
    
    public bool IsInDrawMode()
    {
        return drawMode;
    }
    
    public bool IsInEraseMode()
    {
        return eraseMode;
    }
    
    public float GetDrawRadius()
    {
        return drawRadius;
    }
    
    public float GetEraseRadius()
    {
        return eraseRadius;
    }
    
    public void SetDrawRadius(float radius)
    {
        drawRadius = Mathf.Max(0.001f, radius);
    }
    
    public void SetEraseRadius(float radius)
    {
        eraseRadius = Mathf.Max(0.001f, radius);
    }
    
    private void DebugDrawCircle(Vector3 center, float radius, Color color)
    {
        int segments = 16;
        Vector3 prevPoint = center + new Vector3(radius, 0, 0);
        
        for (int i = 0; i < segments + 1; i++)
        {
            float angle = (float)i / segments * 360f * Mathf.Deg2Rad;
            Vector3 newPoint = center + new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
            Debug.DrawLine(prevPoint, newPoint, color);
            prevPoint = newPoint;
        }
    }
} 