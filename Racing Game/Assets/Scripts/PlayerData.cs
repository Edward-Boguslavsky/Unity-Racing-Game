using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int money;
    public int experience;

    public PlayerData(GameManager player)
    {
        money = player.money;
        experience = player.experience;
    }
}
