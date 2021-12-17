using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CycleCars : MonoBehaviour
{
    public Button leftArrow;
    public Button rightArrow;

    GameObject[] cars = new GameObject[3];
    int middleCar = 0;
    int middleCarColor = 0;

    public Text carNameTag;
    string[] carNames = {"Subaru BRZ", "Dodge Challenger", "Tesla Model 3"};

    public GameObject colorOptions;
    public Transform[] colorPreviews = new Transform[4];
    Color[,] carColors = { { new Color(0.915f, 0.915f, 0.915f), new Color(0.251f, 0.251f, 0.251f), new Color(0.749f, 0.188f, 0.188f), new Color(0.188f, 0.337f, 0.749f) },
                           { new Color(0.915f, 0.915f, 0.915f), new Color(0.251f, 0.251f, 0.251f), new Color(0.749f, 0.376f, 0.188f), new Color(0.549f, 0.749f, 0.188f) },
                           { new Color(0.915f, 0.915f, 0.915f), new Color(0.251f, 0.251f, 0.251f), new Color(0.749f, 0.188f, 0.188f), new Color(0.188f, 0.337f, 0.749f) } };

    public RuntimeAnimatorController leftMidAnimCont;
    public RuntimeAnimatorController rightMidAnimCont;
    public RuntimeAnimatorController toggleLeftAnimCont;
    public RuntimeAnimatorController toggleRightAnimCont;

    public Button playButton;

    // Start is called before the first frame update
    void Start()
    {
        /*
        // Hide left arrow
        leftArrow.gameObject.SetActive(false);

        // Set cars dynamically
        for (int i = 0; i < cars.Length; i++)
            cars[i] = transform.Find("Car_" + (i + 1).ToString()).gameObject;

        // Set name tag to first car name
        carNameTag.text = carNames[middleCar];

        // Set color previews to first car colors
        for (int i = 0; i < 4; i++)
            colorPreviews[i].GetComponent<Image>().color = carColors[middleCar, i];
            */
    }

    public void cycle(float direction)
    {
        /*
        // Reset car color
        changeCarColor(0);

        // Animate car cycle
        if (direction == 1)
        {
            cars[middleCar - 1].GetComponent<Animator>().runtimeAnimatorController = leftMidAnimCont;
            cars[middleCar].GetComponent<Animator>().runtimeAnimatorController = rightMidAnimCont;

            cars[middleCar - 1].GetComponent<Animator>().SetTrigger("LeftArrowPressed");
            cars[middleCar].GetComponent<Animator>().SetTrigger("LeftArrowPressed");

            if (middleCar - 2 >= 0)
            {
                cars[middleCar - 2].GetComponent<Animator>().runtimeAnimatorController = toggleLeftAnimCont;
                cars[middleCar - 2].GetComponent<Animator>().SetTrigger("CycleActive");
            }
            if (middleCar + 1 <= cars.Length - 1)
            {
                cars[middleCar + 1].GetComponent<Animator>().runtimeAnimatorController = toggleRightAnimCont;
                cars[middleCar + 1].GetComponent<Animator>().SetTrigger("CycleInactive");
            }

            middleCar--;
        }
        else
        {
            cars[middleCar].GetComponent<Animator>().runtimeAnimatorController = leftMidAnimCont;
            cars[middleCar + 1].GetComponent<Animator>().runtimeAnimatorController = rightMidAnimCont;

            cars[middleCar].GetComponent<Animator>().SetTrigger("RightArrowPressed");
            cars[middleCar + 1].GetComponent<Animator>().SetTrigger("RightArrowPressed");

            if (middleCar - 1 >= 0)
            {
                cars[middleCar - 1].GetComponent<Animator>().runtimeAnimatorController = toggleLeftAnimCont;
                cars[middleCar - 1].GetComponent<Animator>().SetTrigger("CycleInactive");
            }
            if (middleCar + 2 <= cars.Length - 1)
            {
                cars[middleCar + 2].GetComponent<Animator>().runtimeAnimatorController = toggleRightAnimCont;
                cars[middleCar + 2].GetComponent<Animator>().SetTrigger("CycleActive");
            }

            middleCar++;
        }

        // Show/hide arrows at ends of car cycle
        if (middleCar == 0)
            leftArrow.gameObject.SetActive(false);
        else if (middleCar == cars.Length - 1)
            rightArrow.gameObject.SetActive(false);
        else
        {
            leftArrow.gameObject.SetActive(true);
            rightArrow.gameObject.SetActive(true);
        }

        // Disable buttons while cycling
        StartCoroutine(reactivateButtons());

        // Set name tag text
        carNameTag.text = carNames[middleCar];

        // Animate color previews
        colorOptions.GetComponent<Animator>().SetTrigger("ArrowPressed");
        StartCoroutine(updateColorOptions());
        */
    }

    public void changeCarColor(int colorIndex)
    {
        middleCarColor = colorIndex;
        cars[middleCar].GetComponent<Image>().color = carColors[middleCar, colorIndex];
    }

    public void loadScene()
    {
        GameManager.player.carData.chassisIndex = middleCar;
        GameManager.player.carData.chassisColor = carColors[middleCar, middleCarColor];
        GameSceneManager.sceneManager.loadScene("GameScene");
    }

    IEnumerator reactivateButtons()
    {
        leftArrow.interactable = false;
        rightArrow.interactable = false;
        playButton.interactable = false;
        playButton.GetComponentInChildren<Text>().color = new Color(1f, 1f, 1f, 0.5f);
        yield return new WaitForSeconds(0.25f);
        leftArrow.interactable = true;
        rightArrow.interactable = true;
        playButton.interactable = true;
        playButton.GetComponentInChildren<Text>().color = new Color(1f, 1f, 1f, 1f);
    }

    IEnumerator updateColorOptions()
    {
        yield return new WaitForSeconds(7f / 60f);
        for (int i = 0; i < 4; i++)
            colorPreviews[i].GetComponent<Image>().color = carColors[middleCar, i];
    }
}
