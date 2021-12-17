using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanTileEffect : MonoBehaviour
{
    public Transform cameraTransform;

    void FixedUpdate()
    {
        transform.position = new Vector3(Mathf.Floor(cameraTransform.position.x), Mathf.Floor(cameraTransform.position.y), 4f);
    }
}
