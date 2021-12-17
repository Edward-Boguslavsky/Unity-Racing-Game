using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{

    public Button buttonContinue;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

        buttonContinue.interactable = SaveSystem.SaveFileExists();

    }

    

}
