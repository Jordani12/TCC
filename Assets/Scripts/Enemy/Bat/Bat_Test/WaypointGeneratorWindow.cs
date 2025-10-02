using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class WaypointGeneratorWindow : EditorWindow
{
    [System.Serializable]
    public class BoxSettings
    {
        public Transform parentTransform; // Transform pai individual para esta caixa
        public Vector3 center = Vector3.zero;
        public Vector3 size = new Vector3(10f, 10f, 10f);
        public Color color = Color.yellow;
        public bool showBox = true;
        public int numberOfWaypoints = 5; // Número de waypoints específico para esta caixa
    }

    private List<BoxSettings> boxes = new List<BoxSettings>();
    private bool showWaypointGizmos = true;
    private float waypointGizmoSize = 0.5f;
    private Color waypointGizmoColor = Color.cyan;
    private List<GameObject> generatedWaypoints = new List<GameObject>();
    private Vector2 scrollPosition;
    private int totalWaypoints; // Total de waypoints a serem gerados (soma de todas as caixas)

    [MenuItem("Tools/Random Waypoint Generator")]
    public static void ShowWindow()
    {
        GetWindow<WaypointGeneratorWindow>("Random Waypoint Generator");
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Box Settings", EditorStyles.boldLabel);

        totalWaypoints = 0; // Resetar o total

        for (int i = 0; i < boxes.Count; i++)
        {
            EditorGUILayout.LabelField($"Box {i + 1}", EditorStyles.boldLabel);
            boxes[i].parentTransform = (Transform)EditorGUILayout.ObjectField("Box Parent", boxes[i].parentTransform, typeof(Transform), true);
            boxes[i].center = EditorGUILayout.Vector3Field("Center (Local)", boxes[i].center);
            boxes[i].size = EditorGUILayout.Vector3Field("Size", boxes[i].size);
            boxes[i].color = EditorGUILayout.ColorField("Box Color", boxes[i].color);
            boxes[i].showBox = EditorGUILayout.Toggle("Show Box", boxes[i].showBox);
            boxes[i].numberOfWaypoints = EditorGUILayout.IntField("Waypoints Count", boxes[i].numberOfWaypoints);

            // Atualizar o total
            totalWaypoints += boxes[i].numberOfWaypoints;

            if (GUILayout.Button("Remove Box"))
            {
                boxes.RemoveAt(i);
                break;
            }
            EditorGUILayout.Space();
        }

        // Mostrar o total de waypoints
        EditorGUILayout.LabelField($"Total Waypoints: {totalWaypoints}", EditorStyles.boldLabel);

        if (GUILayout.Button("Add New Box"))
        {
            boxes.Add(new BoxSettings());
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Waypoint Settings", EditorStyles.boldLabel);
        showWaypointGizmos = EditorGUILayout.Toggle("Show Waypoints", showWaypointGizmos);
        waypointGizmoColor = EditorGUILayout.ColorField("Waypoints Color", waypointGizmoColor);
        waypointGizmoSize = EditorGUILayout.Slider("Waypoint Size", waypointGizmoSize, 0.1f, 2f);

        EditorGUILayout.Space();
        if (GUILayout.Button("Generate Random Waypoints"))
        {
            GenerateRandomWaypoints();
        }
        if (GUILayout.Button("Clear Generated Waypoints"))
        {
            ClearGeneratedWaypoints();
        }

        EditorGUILayout.EndScrollView();
    }

    private Vector3 GetRandomPositionInBox(BoxSettings box)
    {
        Vector3 halfSize = box.size * 0.5f;
        Vector3 localPos = box.center + new Vector3(
            Random.Range(-halfSize.x, halfSize.x),
            Random.Range(-halfSize.y, halfSize.y),
            Random.Range(-halfSize.z, halfSize.z));

        // Se a caixa tiver um pai, converter para as coordenadas do pai
        if (box.parentTransform != null)
        {
            return box.parentTransform.TransformPoint(localPos);
        }
        return localPos;
    }

    private void GenerateRandomWaypoints()
    {
        if (boxes.Count == 0)
        {
            Debug.LogWarning("At least one box must be defined.");
            return;
        }

        ClearGeneratedWaypoints();

        int waypointIndex = 0;

        foreach (BoxSettings box in boxes)
        {
            for (int i = 0; i < box.numberOfWaypoints; i++)
            {
                Vector3 worldPos = GetRandomPositionInBox(box);

                GameObject waypoint = new GameObject($"Waypoint_{waypointIndex}");
                waypointIndex++;
                waypoint.transform.position = worldPos;

                // Se a caixa tiver um pai, torna o waypoint filho desse pai
                if (box.parentTransform != null)
                {
                    waypoint.transform.SetParent(box.parentTransform);
                }

                generatedWaypoints.Add(waypoint);
            }
        }

        Debug.Log($"Generated {waypointIndex} waypoints across {boxes.Count} boxes");
    }

    private void ClearGeneratedWaypoints()
    {
        foreach (GameObject wp in generatedWaypoints)
        {
            if (wp != null) DestroyImmediate(wp);
        }
        generatedWaypoints.Clear();
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        foreach (BoxSettings box in boxes)
        {
            if (!box.showBox) continue;

            Handles.color = box.color;
            Matrix4x4 originalMatrix = Handles.matrix;

            // Se a caixa tiver um pai, usar a matriz de transformação do pai
            if (box.parentTransform != null)
            {
                Handles.matrix = box.parentTransform.localToWorldMatrix;
            }

            Handles.DrawWireCube(box.center, box.size);
            Handles.matrix = originalMatrix;
        }

        if (!showWaypointGizmos) return;

        Handles.color = waypointGizmoColor;
        foreach (GameObject wp in generatedWaypoints)
        {
            if (wp == null) continue;
            Handles.DrawWireDisc(wp.transform.position, Vector3.up, waypointGizmoSize);
            Handles.DrawWireDisc(wp.transform.position, Vector3.forward, waypointGizmoSize);
            Handles.DrawWireDisc(wp.transform.position, Vector3.right, waypointGizmoSize);
        }
    }
}
