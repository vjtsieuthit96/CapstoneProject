using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(LineRenderer))]
public class PathDrawer : MonoBehaviour
{
    public static PathDrawer Instance { get; private set; }

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform target;

    [Header("Settings")]
    [SerializeField] private float groundOffset = 0.05f; 
    [SerializeField] private float startOffset = 1.5f;
    [SerializeField] private float hideDistance = 2f;
    [SerializeField] private float visibleDuration = 2f;

    [Header("Visual")]
    [SerializeField] private Color pathColor = new Color(0f, 1f, 1f, 0.4f);
    [SerializeField] private float lineWidth = 0.15f;

    private LineRenderer lineRenderer;
    private NavMeshPath navMeshPath;
    private float visibleTimer;
    private bool isVisible;

    public Transform task1;
    public Transform task2;
    public Transform task3;
    public Transform task4;
    public Transform task5;
    public Transform Enemy;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        lineRenderer = GetComponent<LineRenderer>();
        navMeshPath = new NavMeshPath();

        SetupLineRenderer();
        lineRenderer.enabled = false;
          
    }
    public void setplayer(Transform PlayerSet)
    {
        player = PlayerSet;
    }    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ShowPath();
        }
        if (!isVisible) return;
        if (target == null || player == null)
        {
            HidePath();
            return;
        }

        visibleTimer -= Time.deltaTime;
        if (visibleTimer <= 0f)
        {
            HidePath();
            return;
        }
        
    }

    public void CheckGame()
    {
        var Game = PlayerRealTimeData.Instance;
        if(!Game.isNewGame)
        {
            if (!Game.isTask1 && !Game.isTask2 && !Game.isTask3 && !Game.isTask4)
            {
                task1.gameObject.SetActive(true);
                SetTarget(task1);
            }
            else if (Game.isTask1 && !Game.isTask2 && !Game.isTask3 && !Game.isTask4)
            {
                task1.gameObject.SetActive(false);
                task2.gameObject.SetActive(false);
                task3.gameObject.SetActive(true);
                SetTarget(task3);
            }
            else if (Game.isTask1 && Game.isTask2 && !Game.isTask3 && !Game.isTask4)
            {
                task1.gameObject.SetActive(false);
                task2.gameObject.SetActive(false);
                task3.gameObject.SetActive(false);
                task4.gameObject.SetActive(true);
                SetTarget(task4);
            }
            else if (Game.isTask1 && Game.isTask2 && Game.isTask3 && !Game.isTask4)
            {
                task1.gameObject.SetActive(false);
                task2.gameObject.SetActive(false);
                task3.gameObject.SetActive(false);
                task4.gameObject.SetActive(false);
                task5.gameObject.SetActive(true);
                SetTarget(task5);
            }
            else if (Game.isTask1 && Game.isTask2 && !Game.isTask3 && !Game.isTask4)
            {
                task1.gameObject.SetActive(false);
                task2.gameObject.SetActive(false);
                task3.gameObject.SetActive(false);
                task4.gameObject.SetActive(false);
                task5.gameObject.SetActive(false);
                Enemy.gameObject.SetActive(true);
                SetTarget(Enemy);
            }
        }
        else
        {
            task1.gameObject.SetActive(true);
            SetTarget(task1);
        }    
    }

    private void UpdatePath()
    {
        if (Vector3.Distance(player.position, target.position) <= hideDistance)
        {
            HidePath();
            return;
        }

        if (NavMesh.CalculatePath(player.position, target.position, NavMesh.AllAreas, navMeshPath))
        {
            Vector3[] adjustedCorners = new Vector3[navMeshPath.corners.Length + 1];

            Vector3 startPos = player.position + player.forward * startOffset;
            startPos.y = groundOffset;
            adjustedCorners[0] = startPos;

            for (int i = 0; i < navMeshPath.corners.Length; i++)
            {
                Vector3 corner = navMeshPath.corners[i];
                corner.y = groundOffset;
                adjustedCorners[i + 1] = corner;
            }

            lineRenderer.positionCount = adjustedCorners.Length;
            lineRenderer.SetPositions(adjustedCorners);
        }
        else
        {
            HidePath();
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void ClearTarget()
    {
        target = null;
        HidePath();
    }

    public void ShowPath()
    {
        if (player == null || target == null) return;

        isVisible = true;
        visibleTimer = visibleDuration;
        lineRenderer.enabled = true;
        UpdatePath();
    }

    private void HidePath()
    {
        isVisible = false;
        lineRenderer.enabled = false;
    }

    private void SetupLineRenderer()
    {
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = pathColor;
        lineRenderer.endColor = pathColor;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.numCapVertices = 4;
        lineRenderer.numCornerVertices = 4;
        lineRenderer.useWorldSpace = true;
    }

    public void ChangePathColor(Color newColor)
    {
        pathColor = newColor;
        lineRenderer.startColor = newColor;
        lineRenderer.endColor = newColor;
    }
}
