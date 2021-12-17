using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager sceneManager;

    void Awake()
    {
        if (sceneManager != null)
            GameObject.Destroy(sceneManager);
        else
            sceneManager = this;

        DontDestroyOnLoad(this);

        ResourceLoader.LoadAll();
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("OnSceneLoaded: " + scene.name);
        //Debug.Log(mode);

        switch (scene.name)
        {
            case "GameScene":
                {
                    GameManager.player.LoadPlayer();
                    GameManager.player.findComponents();
                    GameManager.player.createCars();
                    break;
                }
            case "GarageScene":
                {
                    GameManager.player.LoadPlayer();
                    break;
                }
        }
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void loadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

        switch (sceneName)
        {
            case "GameScene":
                {
                    GameManager.player.SavePlayer();
                    break;
                }
            case "GarageScene":
                {
                    GameManager.player.deleteCars();
                    break;
                }
        }
    }
}
