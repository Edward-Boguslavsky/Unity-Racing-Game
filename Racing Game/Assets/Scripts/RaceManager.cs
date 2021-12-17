using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceManager : MonoBehaviour
{
    enum RaceState {Roaming, Countdown, Racing, Statistics, Paused};
    int currRaceState = 0;
    bool raceStateChanged = false;
    bool raceStateChangedLate = false;

    CarClass car;

    public Button startButton;
    public Button retryButton;
    public Button forfeitButton;

    int currRaceIndex = 0;
    int currCheckpointIndex = 0;

    float raceStartTime;

    public RaceObject[] raceObject;

    List<GameObject> raceStartpoints = new List<GameObject>();
    List<GameObject> raceCheckpoints = new List<GameObject>();

    public Image raceLight;
    public Sprite[] raceLightStates = new Sprite[4];

    // Start is called before the first frame update
    void Start()
    {
        car = GameManager.player.car;

        for (int i = 0; i < raceObject.Length; i++)
            raceStartpoints.Add(Instantiate(ResourceLoader.raceStartpointPrefab, raceObject[i].startpointPos, Quaternion.identity, transform));
    }

    void FixedUpdate()
    {
        raceStateChangedLate = false;

        if (currRaceState == (int)RaceState.Roaming)
        {
            if (raceStateChanged)
            {
                // Show startpoints
                foreach (GameObject startpoint in raceStartpoints)
                    startpoint.SetActive(true);

                // Hide checkpoints
                raceCheckpoints.Clear();

                // Toggle UI
                GUIController.setActiveMenu(0, false);

                // Hide race info
                GUIController.raceInfo.SetActive(false);

                // Hide retry and forfeit button
                retryButton.gameObject.SetActive(false);
                forfeitButton.gameObject.SetActive(false);

                // Turn off car brakes
                car.toggleHandbrake(false);
            }

            // Show start race button when in race starting point
            startButton.gameObject.SetActive(false);
            for (int i = 0; i < raceStartpoints.Count; i++)
            {
                if (raceStartpoints[i].GetComponent<BoxCollider2D>().bounds.Contains(car.carObject.transform.position))
                {
                    startButton.gameObject.SetActive(true);
                    currRaceIndex = i;
                    break;
                }
            }
        }
        else if (currRaceState == (int)RaceState.Countdown)
        {
            if (raceStateChanged)
            {
                // Hide startpoints
                foreach (GameObject start in raceStartpoints)
                    start.SetActive(false);

                // Show checkpoints
                generateRace(raceObject[currRaceIndex]);

                // Get car into position
                car.resetCar(raceObject[currRaceIndex].raceStartPos, raceObject[currRaceIndex].raceStartRot, false);
                car.toggleFrozen(true);

                // Turn off car brakes
                car.toggleHandbrake(false);

                // Toggle UI
                GUIController.setActiveMenu(0, false);

                // Show race info
                GUIController.raceInfo.SetActive(true);
                GUIController.setRaceTime(0f);

                // Hide start, retry and forfeit button
                startButton.gameObject.SetActive(false);
                retryButton.gameObject.SetActive(false);
                forfeitButton.gameObject.SetActive(false);

                // Show first checkpoint
                setActiveCheckpoint(0);

                // Show race countdown graphic
                StartCoroutine(startRaceCountdown());
            }
        }
        else if (currRaceState == (int)RaceState.Racing)
        {
            if (raceStateChanged)
            {
                // Unfreeze car
                car.toggleFrozen(false);

                // Set time that race began
                raceStartTime = Time.time;

                // Show retry and forfeit button
                retryButton.gameObject.SetActive(true);
                forfeitButton.gameObject.SetActive(true);
            }

            // Update current checkpoint
            if (raceCheckpoints[currCheckpointIndex].GetComponent<BoxCollider2D>().bounds.Contains(car.carObject.transform.position))
            {
                setActiveCheckpoint(currCheckpointIndex + 1);

                if (currCheckpointIndex == raceCheckpoints.Count)
                {
                    changeRaceState(RaceState.Statistics, false);
                    raceStateChangedLate = true;
                }
            }

            // Update race time
            GUIController.setRaceTime(Time.time - raceStartTime);
        }
        else if (currRaceState == (int)RaceState.Statistics)
        {
            if (raceStateChanged)
            {
                // Turn on car brakes
                car.toggleHandbrake(true);

                // Toggle UI
                GUIController.setActiveMenu(2, false);

                // Slow down time scale
                GUIController.setTimeScale(0.1f);
            }
        }
        else if (currRaceState == (int)RaceState.Paused)
        {
            if (raceStateChanged)
            {
                
            }
        }

        if (!raceStateChangedLate)
            raceStateChanged = false;
    }

    void changeRaceState(RaceState state, bool transition)
    {
        if (transition)
            GUIController.triggerBlackout(() => changeRaceState(state, false));
        else
        {
            // Reset time scale
            GUIController.setTimeScale(1f);

            currRaceState = (int)state;
            raceStateChanged = true;
        }
    }

    public void changeRaceState(int state)
    {
        int newState = state % 5;
        bool transition = state != newState;

        switch(newState)
        {
            case 0: changeRaceState(RaceState.Roaming, transition); break;
            case 1: changeRaceState(RaceState.Countdown, transition); break;
            case 2: changeRaceState(RaceState.Racing, transition); break;
            case 3: changeRaceState(RaceState.Statistics, transition); break;
            case 4: changeRaceState(RaceState.Paused, transition); break;
        }
    }

    void generateRace(RaceObject race)
    {
        for (int i = 0; i < race.checkpointPos.Length; i++)
        {
            GameObject checkpointPrefab;
            if (i < race.checkpointPos.Length - 1)
                checkpointPrefab = ResourceLoader.raceCheckpointPrefab;
            else
                checkpointPrefab = ResourceLoader.raceEndpointPrefab;

            GameObject checkpoint = Instantiate(checkpointPrefab, race.checkpointPos[i], Quaternion.identity, transform);
            checkpoint.GetComponent<BoxCollider2D>().size = race.checkpointRad[i];
            checkpoint.SetActive(false);

            raceCheckpoints.Add(checkpoint);
        }
    }

    void setActiveCheckpoint(int checkpointIndex)
    {
        currCheckpointIndex = checkpointIndex;

        if (checkpointIndex < raceCheckpoints.Count)
        {
            raceCheckpoints[checkpointIndex].SetActive(true);
            raceCheckpoints[checkpointIndex].transform.GetChild(0).gameObject.SetActive(true);
        }

        // Hide previous checkpoint
        if (checkpointIndex > 0)
            StartCoroutine(hideCheckpoint(raceCheckpoints[checkpointIndex - 1]));
    }

    IEnumerator startRaceCountdown()
    {
        raceLight.gameObject.SetActive(true);
        raceLight.transform.GetChild(0).GetComponent<Image>().sprite = raceLightStates[3];

        yield return new WaitForSeconds(2f);
        raceLight.transform.GetChild(0).GetComponent<Image>().sprite = raceLightStates[0];

        yield return new WaitForSeconds(1f);
        raceLight.transform.GetChild(0).GetComponent<Image>().sprite = raceLightStates[1];

        yield return new WaitForSeconds(1f);
        raceLight.transform.GetChild(0).GetComponent<Image>().sprite = raceLightStates[2];

        changeRaceState(RaceState.Racing, false);

        yield return new WaitForSeconds(1.5f);
        raceLight.gameObject.SetActive(false);
    }

    IEnumerator hideCheckpoint(GameObject checkpoint)
    {
        checkpoint.GetComponent<Animator>().SetTrigger("GotCheckpoint");
        checkpoint.transform.GetChild(0).gameObject.SetActive(false);

        yield return new WaitForSeconds(0.25f);
        checkpoint.GetComponent<Animator>().Rebind();
        checkpoint.SetActive(false);
    }
}
