﻿using UnityEngine;
using UnityEngine.UI;
using CBGames;
using TMPro;

public class LeaderboardItemController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI usernameTxt = null;
    [SerializeField] private TextMeshProUGUI levelTxt = null;


    public void OnSetup(int indexRank, PlayerLeaderboardData data)
    {
        transform.localScale = Vector3.one;
        usernameTxt.text = indexRank.ToString() + "." + " " + data.Name;
        levelTxt.text = "Level: " + data.Level.ToString();

        if (indexRank == 1)
        {
            usernameTxt.color = Color.red;
            levelTxt.color = Color.red;
        }
        else if (indexRank == 2)
        {
            usernameTxt.color = Color.yellow;
            levelTxt.color = Color.yellow;
        }
        else if (indexRank == 3)
        {
            usernameTxt.color = Color.blue;
            levelTxt.color = Color.blue;
        }
        else if (indexRank == 4)
        {
            usernameTxt.color = Color.green;
            levelTxt.color = Color.green;
        }
        else if (indexRank == 5)
        {
            usernameTxt.color = Color.magenta;
            levelTxt.color = Color.magenta;
        }
    }
}
