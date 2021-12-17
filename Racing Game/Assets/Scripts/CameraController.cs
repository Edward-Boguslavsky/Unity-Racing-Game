using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    Transform target;
    public Transform ocean;

    float distX = 0f;
    float distY = 0f;

    float distMult = 20f;

    // Start is called before the first frame update
    void Start()
    {
        target = GameManager.player.car.carObject.transform;

        transform.position = new Vector3(target.position.x, target.position.y, -20f);
    }

    void FixedUpdate()
    {
        transform.position -= new Vector3(distX * distMult, distY * distMult, 0f);

        distX = target.position.x - transform.position.x;
        distY = target.position.y - transform.position.y;

        transform.position = new Vector3(target.position.x + distX * distMult, target.position.y + distY * distMult, -20f);

        ocean.position = new Vector3(Mathf.Floor(transform.position.x), Mathf.Floor(transform.position.y), 4f);
    }
}
