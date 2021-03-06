﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndLevelUI : MonoBehaviour
{
    public Text EndLevelText;

    public GameObject NextLevelButton;

    public GameObject GoHomeButton;

    public GameObject RestartButton;

    public string nextLevelName = "scn_game_tutorial";
    public string menuLevel = "Menu_home";

    public void Init(bool hasWon)
    {
        EndLevelText.text = hasWon ? "LEVEL COMPLETE" : "<color=red>LEVEL FAILED</color>";

        if (hasWon)
        {
            NextLevelButton.SetActive(true);
            RestartButton.SetActive(false);
            GoHomeButton.SetActive(true);
            //enable win buttons
        }
        else
        {
            NextLevelButton.SetActive(false);
            RestartButton.SetActive(true);
            GoHomeButton.SetActive(true);
            //enable lose button
        }
        
        gameObject.SetActive(true);
        
    }


    public void RestartLevel()
    {
        LoadingManager.Instance.ReloadActualScene();
    }

    public void NextLevel()
    {
        LoadingManager.Instance.NextScene();
    }

    public void GoHome()
    {
        LoadingManager.Instance.Load(menuLevel);
    }
}
