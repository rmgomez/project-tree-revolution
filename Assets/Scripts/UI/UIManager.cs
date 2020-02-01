using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public PlayerUI PlayerUI;
    public Text TurnCounterText;
    public Text TurnText;
    public Text NaturePointsText;
    
    

    private void OnEnable()
    {
        LevelManager.OnEnemyTurnStarted += OnEnemyTurnStarted;
        LevelManager.OnEnemyTurnCompleted += OnEnemyTurnCompleted;
        LevelManager.OnLevelStart += OnLevelStart;
        
        LevelManager.OnPlayerTurnStarted += OnPlayerTurnStarted;
        LevelManager.OnPlayerTurnCompleted += OnPlayerTurnCompleted;
    }

    private void OnDisable()
    {
        LevelManager.OnEnemyTurnStarted -= OnEnemyTurnStarted;
        LevelManager.OnEnemyTurnCompleted -= OnEnemyTurnCompleted;
        LevelManager.OnLevelStart -= OnLevelStart;
        
        LevelManager.OnPlayerTurnStarted -= OnPlayerTurnStarted;
        LevelManager.OnPlayerTurnCompleted -= OnPlayerTurnCompleted;
    }

    void Update()
    {
        if (LevelManager.Instance.CurrentState != LevelState.LevelEnd)
        {
            NaturePointsText.text = LevelManager.Instance.CurrentNaturePoints.ToString("D2");
        }
    }

    void OnLevelStart()
    {
        PlayerUI.SpawnSpellSlots(LevelManager.Instance.LevelSpells);
    }


    void OnPlayerTurnStarted()
    {
        TurnCounterText.text = $"TURN: {LevelManager.Instance.CurrentTurn}";
        TurnText.text = "PLAYER TURN";
        PlayerUI.SetInteractable(true);
    }
    void OnPlayerTurnCompleted()
    {

    }
    void OnEnemyTurnStarted()
    {
        TurnText.text = "ENEMY TURN";
    }
    void OnEnemyTurnCompleted()
    {
        
    }
    
}
