using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
public class WaypointManager : MonoBehaviour
{

    int numWaypoints;
    public List<Transform> waypoints = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        updateWaypoints();
    }

    private void OnDrawGizmos()
    {
        if (numWaypoints != transform.childCount)
            updateWaypoints();

        for (int i = 0; i < waypoints.Count; i++)
        {
            Gizmos.DrawLine(waypoints[i].position, waypoints[(i + 1) % waypoints.Count].position);
        }
    }

    void updateWaypoints()
    {
        numWaypoints = transform.childCount;
        waypoints = GetComponentsInChildren<Transform>().Skip(1).ToList();
    }
}
