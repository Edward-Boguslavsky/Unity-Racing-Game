using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Car Data")]
public class CarData : ScriptableObject
{
    [Range(0, 2)]
    public int chassisIndex;
    public Color chassisColor;
    public Color windowColor;
    [Range(-1, 2)]
    public int spoilerIndex;
}
