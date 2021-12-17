using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CarClass
{

    public GameObject carObject;
    Rigidbody2D carRigidBody;

    GameObject carWindow;
    GameObject carSpoiler;
    GameObject[] carTires = new GameObject[4];
    TrailRenderer[] carTireTracks = new TrailRenderer[4];

    enum CarType { Player, Opponent };
    CarType carType;

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

    enum Terrain { Asphalt, Pavement, Grass, Water, None };
    Terrain currTerrain = Terrain.None;
    Terrain lastTerrain = Terrain.None;

    const float HANDBRAKE_SPEED_MULT = 0.3f;
    const float HANDBRAKE_TORQUE_MULT = 0.4f;
    const float HANDBRAKE_LAT_DECELERATION = 0.01f;

    const float GRASS_SPEED_MULT = 0.8f;
    const float GRASS_TORQUE_MULT = 0.8f;
    const float GRASS_LAT_DECELERATION = 0.01f;
    const float GRASS_DRIFT_LIMIT_MULT = 0.5f;

    Tilemap tileAsphalt;
    Tilemap tileGrass;
    Tilemap tilePavement;

    public CarClass(string name, int type)
    {
        setup(name, type);
    }

    public CarClass(string name, int type, Vector2 pos, float rot)
    {
        setup(name, type);
        resetCar(pos, rot, false);
    }

    public CarClass(string name, int type, Vector2 pos, float rot, CarData data)
    {
        setup(name, type);
        resetCar(pos, rot, false);
        setCarData(data);
    }

    void setup(string name, int type)
    {
        Transform charactersFolder = GameObject.Find("Characters").transform;

        carObject = MonoBehaviour.Instantiate(ResourceLoader.carPrefab, Vector2.zero, Quaternion.identity, charactersFolder);
        carObject.name = name;

        carRigidBody = carObject.GetComponent<Rigidbody2D>();

        carWindow = carObject.transform.Find("Windows").gameObject;

        carSpoiler = carObject.transform.Find("Spoiler").gameObject;

        for (int i = 0; i < 4; i++)
        {
            carTires[i] = carObject.transform.Find("Tire_" + (i + 1).ToString()).gameObject;
            carTireTracks[i] = carTires[i].GetComponent<TrailRenderer>();
        }

        // Modify car based on type
        SpriteRenderer carIcon = carObject.transform.Find("Icon").GetComponent<SpriteRenderer>();
        if (type == 0)
        {
            carType = CarType.Player;
            carIcon.color = new Color(0.835f, 0.168f, 0.168f);
            carIcon.sortingOrder = 1000;
        }
        else if (type == 1)
        {
            carType = CarType.Opponent;
            carObject.AddComponent<AIController>();
            carIcon.color = new Color(0.835f, 0.835f, 0.168f);
            randomizeCar();
        }

        tileAsphalt = GameObject.Find("World/Map_Grid/Terrain/Tilemap_Asphalt").GetComponent<Tilemap>();
        tileGrass = GameObject.Find("World/Map_Grid/Terrain/Tilemap_Grass").GetComponent<Tilemap>();
        tilePavement = GameObject.Find("World/Map_Grid/Terrain/Tilemap_Pavement").GetComponent<Tilemap>();
    }

    public void move(float vertical, float horizontal)
    {
        if (carRigidBody.bodyType == RigidbodyType2D.Dynamic)
        {
            // Add forward-backward force
            if (vertical > 0)
                carRigidBody.AddForce(carObject.transform.up * vertical * currSpeed);
            else
                carRigidBody.AddForce(carObject.transform.up * vertical * currSpeed / 2);

            // Add rotation force
            carRigidBody.angularVelocity += carRigidBody.velocity.magnitude / currSpeed * horizontal * currTorque;

            // Initiate drift if lateral velocity exceeds tire friction limit
            bool isDrifting = getRightVelocity().magnitude > currDriftLimit;

            // Determine magnitude of lateral deceleration
            if (isDrifting)
                currDrift = currDriftMax;
            else
                currDrift = DRIFT_MIN;

            // Apply forward-backward and left-right velocities
            carRigidBody.velocity = getForwardVelocity() + getRightVelocity() * currDrift;

            // Emit tire tracks when drifting
            toggleTireTracks(isDrifting);

            // Effect car based on terrain under it
            handleTerrain();

            // Turn tires based on horizontal input
            foreach (GameObject tire in carTires)
                tire.transform.localEulerAngles = new Vector3(0, 0, horizontal * 45f);
        }
    }

    public void toggleHandbrake(bool enabled)
    {
        if (enabled)
        {
            currSpeed *= HANDBRAKE_SPEED_MULT;
            currTorque *= HANDBRAKE_TORQUE_MULT;
            currDriftMax -= HANDBRAKE_LAT_DECELERATION;
            currDriftLimit = 0;
        }
        else
        {
            currSpeed /= HANDBRAKE_SPEED_MULT;
            currTorque /= HANDBRAKE_TORQUE_MULT;
            currDriftMax += HANDBRAKE_LAT_DECELERATION;
            if (currTerrain == Terrain.Grass)
                currDriftLimit = DRIFT_LIMIT * GRASS_DRIFT_LIMIT_MULT;
            else
                currDriftLimit = DRIFT_LIMIT;
        }
    }

    public void toggleGrass(bool enabled)
    {
        if (enabled)
        {
            currSpeed *= GRASS_SPEED_MULT;
            currTorque *= GRASS_TORQUE_MULT;
            currDriftMax -= GRASS_LAT_DECELERATION;
            currDriftLimit *= GRASS_DRIFT_LIMIT_MULT;
        }
        else
        {
            currSpeed /= GRASS_SPEED_MULT;
            currTorque /= GRASS_TORQUE_MULT;
            currDriftMax += GRASS_LAT_DECELERATION;
            currDriftLimit /= GRASS_DRIFT_LIMIT_MULT;
        }
    }

    void toggleTireTracks(bool enabled)
    {
        foreach (TrailRenderer track in carTireTracks)
            track.emitting = enabled;
    }

    public void toggleFrozen(bool enabled)
    {
        if (enabled)
            carRigidBody.bodyType = RigidbodyType2D.Static;
        else
            carRigidBody.bodyType = RigidbodyType2D.Dynamic;
    }

    public void resetCar(Vector2 pos, float rot, bool flash)
    {
        if (carType == CarType.Player)
            GameManager.player.cm.Follow = null;

        carRigidBody.position = pos;
        carRigidBody.rotation = rot;

        carRigidBody.velocity = Vector2.zero;
        carRigidBody.angularVelocity = 0f;

        foreach (TrailRenderer track in carTireTracks)
            track.Clear();

        if (flash)
            GameManager.player.StartCoroutine(flashCar(10));
    }

    public void randomizeCar()
    {
        setCarChassis(Random.Range(0, 3), Random.Range(0, 4));
        setCarSpoiler(Random.Range(-1, 3));
    }

    IEnumerator flashCar(int amount)
    {
        if (carType == CarType.Player)
        {
            yield return new WaitForSeconds(0);
            GameManager.player.cm.Follow = carObject.transform;
        }

        Renderer[] childRenderers = carObject.GetComponentsInChildren<Renderer>();

        for (int i = 0; i < amount; i++)
        {
            carObject.GetComponent<Renderer>().enabled = false;
            foreach (Renderer r in childRenderers)
                r.enabled = false;
            yield return new WaitForSeconds(0.1f);
            carObject.GetComponent<Renderer>().enabled = true;
            foreach (Renderer r in childRenderers)
                r.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
    }

    void handleTerrain()
    {
        Vector3Int gridPos = Vector3Int.FloorToInt(carObject.transform.position);

        if (tilePavement.GetTile(gridPos) != null) currTerrain = Terrain.Pavement;
        else if (tileGrass.GetTile(gridPos) != null) currTerrain = Terrain.Grass;
        else if (tileAsphalt.GetTile(gridPos) != null) currTerrain = Terrain.Asphalt;
        else currTerrain = Terrain.Water;

        if (lastTerrain != Terrain.None && currTerrain != lastTerrain)
        {
            if (lastTerrain != Terrain.Pavement && currTerrain == Terrain.Pavement)
                carRigidBody.velocity *= 0.8f;

            if (lastTerrain != Terrain.Grass && currTerrain == Terrain.Grass)
                toggleGrass(true);
            else if (lastTerrain == Terrain.Grass && currTerrain != Terrain.Grass)
                toggleGrass(false);

            if (currTerrain == Terrain.Water)
                resetCar(GameManager.player.carSpawnPos, GameManager.player.carSpawnRot, true);
        }

        lastTerrain = currTerrain;
    }

    Vector2 getForwardVelocity()
    {
        return carObject.transform.up * Vector2.Dot(carRigidBody.velocity, carObject.transform.up);
    }

    Vector2 getRightVelocity()
    {
        return carObject.transform.right * Vector2.Dot(carRigidBody.velocity, carObject.transform.right);
    }

    public void setCarData(CarData data)    // CLEAN UP
    {
        carObject.GetComponent<SpriteRenderer>().sprite = ResourceLoader.carChassisSprites[data.chassisIndex];
        carObject.GetComponent<SpriteRenderer>().color = data.chassisColor;

        carWindow.GetComponent<SpriteRenderer>().sprite = ResourceLoader.carWindowSprites[data.chassisIndex];
        carWindow.GetComponent<SpriteRenderer>().color = data.windowColor;

        if (data.spoilerIndex == -1) carSpoiler.GetComponent<SpriteRenderer>().sprite = null;
        else carSpoiler.GetComponent<SpriteRenderer>().sprite = ResourceLoader.carSpoilerSprites[data.spoilerIndex];
    }

    public void setCarChassis(int chassisIndex, int chassisColorIndex)
    {
        carObject.GetComponent<SpriteRenderer>().sprite = ResourceLoader.carChassisSprites[chassisIndex];
        carObject.GetComponent<SpriteRenderer>().color = ResourceLoader.getStockColorByIndex(chassisIndex, chassisColorIndex);

        carWindow.GetComponent<SpriteRenderer>().sprite = ResourceLoader.carWindowSprites[chassisIndex];
    }

    public void setCarWindowColor(Color windowColor)
    {
        carWindow.GetComponent<SpriteRenderer>().color = windowColor;
    }

    public void setCarSpoiler(int spoilerIndex)
    {
        if (spoilerIndex == -1) carSpoiler.GetComponent<SpriteRenderer>().sprite = null;
        else carSpoiler.GetComponent<SpriteRenderer>().sprite = ResourceLoader.carSpoilerSprites[spoilerIndex];
    }

}
