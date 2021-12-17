using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager player;

    public int money;
    public int experience;

    public Cinemachine.CinemachineVirtualCamera cm;
    public Cinemachine.CinemachineFramingTransposer cmFrame;
    public Joystick joystick;

    public CarClass car;

    public CarClass[] traffic = new CarClass[3];
    AIController[] trafficAI = new AIController[3];

    public Vector2 carSpawnPos = new Vector2(91.75f, -11.5f);
    public float carSpawnRot = 90f;
    public CarData carData;

    void Awake()
    {
        if (player != null)
            GameObject.Destroy(player);
        else
            player = this;

        DontDestroyOnLoad(this);

        ResourceLoader.LoadAll();
    }

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if (car != null)
            car.move(smoothInput(joystick.Vertical) + Input.GetAxis("Vertical"), -smoothInput(joystick.Horizontal) + Input.GetAxis("Horizontal"));

        for (int i = 0; i < traffic.Length; i++)
        {
            if (traffic[i] != null)
            {
                Vector2 inputAI = trafficAI[i].driveTowards(trafficAI[i].nextWaypointPos);
                traffic[i].move(inputAI.x, inputAI.y);
            }
        }

        //if (GUIController.progressInfoMoney != null)
        //    GUIController.setMoney(money);

    }

    public void createCars()
    {
        //car = new CarClass("Car", 0, new Vector2(-4.25f, 4.5f), 90f);
        car = new CarClass("Car", 0, carSpawnPos, carSpawnRot, carData);
        cm.Follow = car.carObject.transform;

        traffic[0] = new CarClass("Traffic_1", 1, new Vector2(23.5f, -18.5f), 0f);
        traffic[1] = new CarClass("Traffic_2", 1, new Vector2(24.5f, -18.5f), 0f);
        traffic[2] = new CarClass("Traffic_3", 1, new Vector2(24.5f, -19.5f), 0f);
        for (int i = 0; i < trafficAI.Length; i++)
            trafficAI[i] = traffic[i].carObject.GetComponent<AIController>();
    }

    public void deleteCars()
    {
        cm = null;
        car = null;

        for (int i = 0; i < traffic.Length; i++)
        {
            trafficAI[i] = null;
            traffic[i] = null;
        }
    }

    float smoothInput(float input)
    {
        return input * Mathf.Abs(Mathf.Sin(input * Mathf.PI / 2));
    }

    public void findComponents()
    {
        cm = GameObject.Find("Cinemachine").GetComponent<Cinemachine.CinemachineVirtualCamera>();
        cmFrame = cm.GetCinemachineComponent<Cinemachine.CinemachineFramingTransposer>();

        joystick = GameObject.Find("Fixed Joystick").GetComponent<FixedJoystick>();
    }

    public void SavePlayer()
    {
        SaveSystem.SaveProgress(this);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadProgress();
        
        if (data != null)
        {
            money = data.money;
            experience = data.experience;
        }
    }

}
