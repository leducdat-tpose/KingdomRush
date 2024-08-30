using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField]
    private float _radiusWayPoint = 0.5f;
    [SerializeField]
    private Vector3[] points;
    public Vector3[] Points => points;

    public Vector3 currentPosition => _currentPosition;

    private Vector3 _currentPosition;

    private bool _gameStarted;

    void Start()
    {
        _gameStarted = true;
        _currentPosition = transform.position;
    }

    public Vector3 GetWaypointPosition(int index)
    {
        return currentPosition + points[index];
    }

    private void OnDrawGizmos()
    {
        if(!_gameStarted && transform.hasChanged) 
        {
            _currentPosition = transform.position;
        }
        for(int i = 0;  i < points.Length; i++)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(points[i] + _currentPosition, _radiusWayPoint);

            if(i < points.Length - 1)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawLine(points[i] + _currentPosition, points[i + 1] + _currentPosition);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[CustomEditor(typeof(Waypoint))]
public class WaypointEditor : Editor
{
    Waypoint Waypoint => target as Waypoint;
    private void OnSceneGUI()
    {
        Handles.color = Color.cyan;
        for(int i = 0; i < Waypoint.Points.Length; i++)
        {
            EditorGUI.BeginChangeCheck();
            //Create handles
            Vector3 currentWaypointPoint = Waypoint.currentPosition + Waypoint.Points[i];
            Vector3 newWaypointPoint = Handles.FreeMoveHandle(currentWaypointPoint,
                Quaternion.identity, 0.7f,
                new Vector3(0.3f, 0.3f, 0.3f), Handles.SphereHandleCap);
            //Create text
            GUIStyle textStyle = new GUIStyle();
            textStyle.fontStyle = FontStyle.Bold;
            textStyle.fontSize = 16;
            textStyle.normal.textColor = Color.white;
            Vector3 textAlignment = Vector3.down * 0.35f + Vector3.right * 0.35f;
            Handles.Label(Waypoint.currentPosition + Waypoint.Points[i] + textAlignment,
                $"{i + 1}", textStyle);
            EditorGUI.EndChangeCheck();

            if(EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Free Move Handle");
                Waypoint.Points[i] = newWaypointPoint - Waypoint.currentPosition;
            }
            
        }
    }
}
