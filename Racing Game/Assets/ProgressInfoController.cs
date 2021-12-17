using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressInfoController : MonoBehaviour
{
    public Text textMoney;
    public Text textXp;

    void Start()
    {
        textMoney = transform.Find("Progress_Money/Text").GetComponent<Text>();
        textXp = transform.Find("Progress_XP/Text").GetComponent<Text>();
    }

    public void setMoney(int money)
    {
        textMoney.text = string.Format("{0:n0}", money);
    }

    public void setXP(int xp)
    {

    }
}
