using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using static InputManager;

[RequireComponent(typeof(InputManager))]
public class LineManager : MonoBehaviour
{
    #region Parameters
    [SerializeField]
    private Player player;
    
    [SerializeField]
    private float lineSeparationDistance = .2f;
    [SerializeField]
    private float lineWidth = .1f;
    [SerializeField]
    private Color lineColor = Color.black;
    [SerializeField]
    private int lineCapVertices = 5;
    [SerializeField]
    private float effectorSpeed = 9f;
    [SerializeField]
    private PhysicsMaterial2D physicsMaterial2D;
    #endregion


    #region Private
    private List<GameObject> lines;
    private List<Vector2> currentLine;
    private GameObject currentLineObject;
    private LineRenderer currentlineRenderer;
    private EdgeCollider2D currentLineEdgeCollider;

    private bool drawing = false;
    private bool erasing = false;

    private Camera mainCamera;
    private Pan panning;
    private Zoom zoom;

    private InputManager inputManager;

    #endregion

    private void Awake()
    {
        inputManager = InputManager.Instance;
        mainCamera = Camera.main;
        panning = GetComponent<Pan>();
        zoom = GetComponent<Zoom>();
    }

    private void OnEnable()
    {
        inputManager.OnStartDraw += OnStartDraw;
        inputManager.OnEndDraw += OnEndDraw;
        inputManager.OnStartErase += OnStartErase;
        inputManager.OnEndErase += OnEndErase;

    }

    private void OnDisable()
    {
        inputManager.OnStartDraw -= OnStartDraw;
        inputManager.OnEndDraw -= OnEndDraw;
        inputManager.OnStartErase -= OnStartErase;
        inputManager.OnEndErase -= OnEndErase;
    }

    #region Drawing

    private void OnStartDraw()
    {
        if (!erasing)
            StartCoroutine("Drawing");
    }

    private void OnEndDraw()
    {
        drawing = false;
    }

    IEnumerator Drawing()
    {
        drawing= true;
        StartLine();
        while (drawing)
        {
            AddPoint(GetCurrentWorldPoint());
            yield return null;
        }
        EndLine();
    }

    private void StartLine()
    {
        // Instantiating the new line
        currentLine = new List<Vector2>();
        currentLineObject = new GameObject();
        currentLineObject.name = "Line";
        currentLineObject.transform.parent= transform;
        currentlineRenderer= currentLineObject.AddComponent<LineRenderer>();
        currentLineEdgeCollider = currentLineObject.AddComponent<EdgeCollider2D>();
        SurfaceEffector2D currentEffector = currentLineObject.AddComponent<SurfaceEffector2D>();

        // Set Settings
        currentlineRenderer.positionCount = 0;
        currentlineRenderer.startWidth = lineWidth;
        currentlineRenderer.endWidth = lineWidth;
        currentlineRenderer.numCapVertices = lineCapVertices;
        currentlineRenderer.material = new Material(Shader.Find("Particles/Standard Unlit"));
        currentlineRenderer.startColor = lineColor;
        currentlineRenderer.endColor = lineColor;
        currentLineEdgeCollider.edgeRadius = .1f;

        currentLineEdgeCollider.sharedMaterial = physicsMaterial2D;
        currentLineEdgeCollider.usedByEffector = true;
        currentEffector.speed = effectorSpeed;

        currentLineObject.layer = 1<<3;
    }

    private void EndLine()
    {
        if (currentLine.Count == 1)
        {
            DestroyLine(currentLineObject);
        }
        else 
        { 
            currentLineEdgeCollider.SetPoints(currentLine);
        }
    }

    private void AddPoint(Vector2 point)
    {
        if (PlacePoint(point))
        {
            currentLine.Add(point);
            currentlineRenderer.positionCount++;
            currentlineRenderer.SetPosition(currentlineRenderer.positionCount - 1, point);

        }
    }

    private bool PlacePoint(Vector2 point)
    {
        if (currentLine.Count == 0) return true;
        if(Vector2.Distance(point, currentLine[currentLine.Count - 1]) < lineSeparationDistance)
            return false;
        return true;
    }

    #endregion


    private void OnStartErase()
    {
        if (!drawing)
            StartCoroutine("Erasing");
    }

    private void OnEndErase()
    {
        erasing = false;
    }

    IEnumerator Erasing()
    {
        erasing = true;
        while (erasing) 
        {
            Vector2 screenMousePosition = GetCurrentScreenPoint();
            GameObject g = Utils.Raycast(mainCamera, screenMousePosition, 8); //100 = 8 in binary, layer mask 01000000
            if (g != null) DestroyLine(g);
            yield return null;
        }

    }

    private void DestroyLine(GameObject g)
    {
        Destroy(g);
    }

    private Vector2 GetCurrentScreenPoint()
    {
        return inputManager.GetMousePosition();
    }


    private Vector2 GetCurrentWorldPoint()
    {
        return mainCamera.ScreenToWorldPoint(inputManager.GetMousePosition());
    }

    private float GetZoomValue()
    {
        return inputManager.GetZoom();
    }

    private void Update()
    {
        if (!player.playing)
        {
            panning.PanScreen(GetCurrentScreenPoint());
            zoom.ZoomScreen(GetZoomValue());
        }
    }


}
