using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public Canvas uiCanvas;
    public Canvas pauseMenuCanvas;

    public void pauseGame()
    {
        GUIController.setActiveMenu(1, false);
        GUIController.setTimeScale(0f);
    }

    public void resumeGame()
    {
        GUIController.setActiveMenu(0, false);
        GUIController.setTimeScale(1f);
    }
}
