using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIController : MonoBehaviour
{
    public static GameObject[] menus = new GameObject[3];
    public static GameObject blackout;

    public static ProgressInfoController progressCont;

    public static GameObject raceInfo;
    public static Text raceInfoPositionNumber;
    public static Text raceInfoPositionSuffix;
    public static Text raceInfoTime;
    public static Text raceResultsPosition;
    public static Text raceResultsTime;

    void Start()
    {
        for (int i = 0; i < 3; i++)
            menus[i] = transform.GetChild(i).gameObject;

        blackout = transform.GetChild(transform.childCount - 1).gameObject;

        progressCont = menus[0].transform.Find("Progress_Info").gameObject.GetComponent<ProgressInfoController>();
        progressCont.setMoney(GameManager.player.money);

        raceInfo = menus[0].transform.Find("Race_Info").gameObject;

        raceInfoPositionNumber = raceInfo.transform.Find("Race_Position/Race_Position_Number").GetComponent<Text>();
        raceInfoPositionSuffix = raceInfo.transform.Find("Race_Position/Race_Position_Suffix").GetComponent<Text>();
        raceInfoTime = raceInfo.transform.Find("Race_Time").GetComponent<Text>();
        raceResultsPosition = menus[2].transform.Find("Race_Results_Menu_Back/Race_Position").GetComponent<Text>();
        raceResultsTime = menus[2].transform.Find("Race_Results_Menu_Back/Race_Time").GetComponent<Text>();
    }

    public static void setActiveMenu(int menuIndex, bool transition)
    {
        if (transition)
            triggerBlackout(() => setActiveMenu(menuIndex, false));

        foreach (GameObject menu in menus)
            menu.SetActive(false);

        menus[menuIndex].SetActive(true);
    }

    public static void triggerBlackout(System.Action callback)
    {
        blackout.GetComponent<BlackoutTransition>().startTransition(callback);
    }

    public static void setRaceTime(float time)
    {
        int mn = (int)(time / 60f);
        int sc = (int)time % 60;
        int ms = (int)(time * 1000) % 1000;
        string timeFormat = mn.ToString("00") + ":" + sc.ToString("00") + ":" + ms.ToString("000");

        raceInfoTime.text = timeFormat;
        raceResultsTime.text = timeFormat;
    }

    public static void setTimeScale(float scale)
    {
        Time.timeScale = scale;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }
}
