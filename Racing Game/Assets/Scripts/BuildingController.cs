using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BuildingController : MonoBehaviour
{
    public enum TargetScene { Garage, Shop };
    public TargetScene scene = TargetScene.Garage;

    public Vector2 exitPosition;
    public float exitRotation;

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.player.carSpawnPos = exitPosition;
        GameManager.player.carSpawnRot = exitRotation;
        GameSceneManager.sceneManager.loadScene("GarageScene");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(exitPosition, 0.5f);
        Gizmos.color = Color.red;
        float rot = Mathf.Deg2Rad * -exitRotation;
        Gizmos.DrawLine(exitPosition, exitPosition + new Vector2(Mathf.Sin(rot), Mathf.Cos(rot)) * 0.5f);
    }
}
