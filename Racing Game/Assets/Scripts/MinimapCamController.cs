using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamController : MonoBehaviour
{

    Transform target;

    // Start is called before the first frame update
    void Start()
    {
        target = GameManager.player.car.carObject.transform;
    }

    void LateUpdate()
    {
        transform.position = new Vector3(target.position.x, target.position.y, -20);
    }
}
