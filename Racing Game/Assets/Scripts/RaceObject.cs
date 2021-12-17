using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Race")]
public class RaceObject : ScriptableObject
{
    public Vector2 startpointPos;
    public Vector2[] checkpointPos;
    public Vector2[] checkpointRad;

    public Vector2 raceStartPos;
    public float raceStartRot;
    public int numOpponents;
}
