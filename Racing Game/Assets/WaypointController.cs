using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointController : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "gizmo_target.png", true);
    }
}
