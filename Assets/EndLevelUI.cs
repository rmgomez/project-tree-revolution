using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndLevelUI : MonoBehaviour
{
    public Text EndLevelText;

    public GameObject NextLevelButton;

    public GameObject GoHomeButton;

    public GameObject RestartButton;
    // Start is called before the first frame update
    
    public void Init(bool hasWon)
    {
        EndLevelText.text = hasWon ? "LEVEL COMPLETE" : "<color=red>LEVEL FAILED</color>";

        if (hasWon)
        {
            RestartButton.SetActive(false);
            //enable win buttons
        }
        else
        {
            NextLevelButton.SetActive(true);
            GoHomeButton.SetActive(true);
            //enable lose button
        }
        
        gameObject.SetActive(true);
        
    }


    public void RestartLevel()
    {
        
    }

    public void NextLevel()
    {
        
    }

    public void GoHome()
    {
        
    }
}
