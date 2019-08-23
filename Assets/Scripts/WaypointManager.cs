using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    [Header("Monitoring Purpose")]
    [SerializeField]
    private List<Waypoint> wayPoints = new List<Waypoint>();

    private void Awake()
    {
        LoadAssets();
    }

    private void LoadAssets()
    {
        foreach (Waypoint wayPoint in GetComponentsInChildren<Waypoint>())
            wayPoints.Add(wayPoint);
    }

    public Waypoint GetRandomWayPoint()
    {
        int randomIndex = Random.Range(0, wayPoints.Count);
        return wayPoints[randomIndex];
    }
}
