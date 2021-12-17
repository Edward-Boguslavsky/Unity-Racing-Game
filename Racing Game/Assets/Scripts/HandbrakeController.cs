using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandbrakeController : MonoBehaviour
{
    public void handbrakeButtonDown()
    {
        GameManager.player.car.toggleHandbrake(true);
    }

    public void handbrakeButtonUp()
    {
        GameManager.player.car.toggleHandbrake(false);
    }

}
