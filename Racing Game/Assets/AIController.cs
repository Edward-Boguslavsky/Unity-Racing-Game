using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{

    Transform[] waypoints;
    public int nextWaypointIndex = -1;
    public Vector2 nextWaypointPos = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        waypoints = GameObject.Find("Waypoints").GetComponent<WaypointManager>().waypoints.ToArray();

        updateWaypoint();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector2.Distance(waypoints[nextWaypointIndex].position, transform.position) < 8f)
            updateWaypoint();
    }

    public Vector2 driveTowards(Vector2 pos)
    {
        float forward;
        float lateral;

        Vector2 distBetween = pos - new Vector2(transform.position.x, transform.position.y);
        float angleBetween = Vector2.SignedAngle(transform.up, distBetween) / 180f;

        lateral = CubeRoot(angleBetween);
        forward = 1f - lateral;

        return new Vector2(forward, lateral);
    }

    void updateWaypoint()
    {
        nextWaypointIndex = (nextWaypointIndex + 1) % waypoints.Length;
        nextWaypointPos = waypoints[nextWaypointIndex].position;
    }

    private static float CubeRoot(float x)
    {
        if (x < 0)
            return -Mathf.Pow(-x, 1f / 3f);
        else
            return Mathf.Pow(x, 1f / 3f);
    }
}
