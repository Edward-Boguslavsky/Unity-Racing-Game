using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
//using UnityEditor;
//using System.Linq;

public class Car2DController : MonoBehaviour
{
    Rigidbody2D CarRB2D;

    public Joystick joystick;
    public Tilemap terrain;

    float verAxis = 0f; // Up-down value of joystick
    float horAxis = 0f; // Left-right value of joystick

    const float SPEED = 3f;
    const float TORQUE = 4f;
    const float DRIFT_MAX = 1f;
    const float DRIFT_MIN = 0.95f;  // Lateral deceleration not during drift
    const float DRIFT_LIMIT = 1.25f;

    float currSpeed = SPEED;   // Forward-backward force
    float currTorque = TORQUE;  // Rotation force
    float currDrift = DRIFT_MIN;   // Current later deceleration
    float currDriftMax = DRIFT_MAX; // Lateral deceleration during drift
    float currDriftLimit = DRIFT_LIMIT;   // Traction limit for entering drift

    const float HANDBRAKE_SPEED_MULT = 0.3f;
    const float HANDBRAKE_TORQUE_MULT = 0.4f;
    const float HANDBRAKE_LAT_DECELERATION = 0.01f;

    //Sprite[] carChassisSprites;
    public Sprite[] carChassisSprites = new Sprite[3];

    public GameObject[] tires = new GameObject[4];
    TrailRenderer[] tireTracks = new TrailRenderer[4];

    bool frozen = false;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.player.LoadPlayer();

        CarRB2D = GetComponent<Rigidbody2D>();

        //carChassisSprites = AssetDatabase.LoadAllAssetsAtPath("Assets/Sprites/car_sprites.png").OfType<Sprite>().ToArray();
        //transform.GetComponent<SpriteRenderer>().sprite = carChassisSprites[Player.player.carChassis];
        //transform.GetComponent<SpriteRenderer>().color = Player.player.carChassisColor;

        for (int i = 0; i < 4; i++)
            tireTracks[i] = tires[i].GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        // Smooth out joystick inputs
        if (frozen)
        {
            verAxis = 0f;
            horAxis = 0f;
        }
        else
        {
            verAxis = joystick.Vertical * Mathf.Abs(Mathf.Sin(joystick.Vertical * Mathf.PI / 2)) + Input.GetAxis("Vertical");
            horAxis = -joystick.Horizontal * Mathf.Abs(Mathf.Sin(-joystick.Horizontal * Mathf.PI / 2)) + Input.GetAxis("Horizontal");
        }

        // Add forward-backward force
        if (verAxis > 0)
            CarRB2D.AddForce(transform.up * verAxis * currSpeed);
        else
            CarRB2D.AddForce(transform.up * verAxis * currSpeed / 2);

        // Add rotation force
        CarRB2D.angularVelocity += CarRB2D.velocity.magnitude / currSpeed * horAxis * currTorque;

        // Determine magnitude of lateral deceleration
        if (getRightVelocity().magnitude > currDriftLimit)
            currDrift = currDriftMax;
        else
            currDrift = DRIFT_MIN;

        // Apply forward-backward and left-right velocities
        CarRB2D.velocity = getForwardVelocity() + getRightVelocity() * currDrift;

        // Emit tire tracks when drifting
        for (int i = 0; i < 4; i++)
            tireTracks[i].emitting = getRightVelocity().magnitude > currDriftLimit;

        // Reset car when falling into ocean
        if (!terrain.GetTile(new Vector3Int(Mathf.FloorToInt(CarRB2D.position.x), Mathf.FloorToInt(CarRB2D.position.y), Mathf.FloorToInt(terrain.transform.position.z))))
            resetCar(new Vector2(-4.25f, 4.5f), 90f, true);
    }

    Vector2 getForwardVelocity()
    {
        return transform.up * Vector2.Dot(CarRB2D.velocity, transform.up);
    }
    Vector2 getRightVelocity()
    {
        return transform.right * Vector2.Dot(CarRB2D.velocity, transform.right);
    }

    public void toggleHandbrake(int enabled)
    {
        currSpeed = SPEED - SPEED * (1 - HANDBRAKE_SPEED_MULT) * enabled;
        currTorque = TORQUE - TORQUE * (1 - HANDBRAKE_TORQUE_MULT) * enabled;
        currDriftMax = DRIFT_MAX - (HANDBRAKE_LAT_DECELERATION * enabled);
        currDriftLimit = DRIFT_LIMIT * (1 - enabled);
    }

    public void resetCar(Vector2 pos, float rot, bool flash)
    {
        CarRB2D.position = pos;
        CarRB2D.velocity = Vector2.zero;
        CarRB2D.rotation = rot;
        CarRB2D.angularVelocity = 0f;
        for (int i = 0; i < 4; i++)
            tireTracks[i].Clear();
        if (flash)
            StartCoroutine(flashCar(10));
    }

    public void freezeCar(bool enabled)
    {
        frozen = enabled;
    }

    IEnumerator flashCar(int amount)
    {
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < amount; i++)
        {
            gameObject.GetComponent<Renderer>().enabled = false;
            foreach (var renderer in renderers)
                renderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            gameObject.GetComponent<Renderer>().enabled = true;
            foreach (var renderer in renderers)
                renderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }        
    }
}
