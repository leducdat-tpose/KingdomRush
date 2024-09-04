using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    [Header("Waypoints")]
    public Waypoint Waypoint;
    private Vector3 startingPoint;
    private Vector3 endingPoint;

    private void Awake()
    {
        instance = this;
        Waypoint = GameObject.Find("Waypoints").GetComponent<Waypoint>();
    }

    void Start()
    {
        startingPoint = Waypoint.Points[0];
        endingPoint = Waypoint.Points[Waypoint.Points.Length - 1];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 GetPoint(int index)
    {
        return Waypoint.GetWaypointPosition(index);
    }

    public Vector3 GetStartingPoint() { return startingPoint; }
    public Vector3 GetEndingPoint() {  return endingPoint; }

    public int GetWaypointsLength()
    {
        return Waypoint.Points.Length;
    }
}
