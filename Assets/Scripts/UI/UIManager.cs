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

    
    public EndLevelUI EndLevelUI;
    
    public ProgressBar ProgressBar;
    
    private void OnEnable()
    {
        LevelManager.OnEnemyTurnStarted += OnEnemyTurnStarted;
        LevelManager.OnEnemyTurnCompleted += OnEnemyTurnCompleted;
        LevelManager.OnLevelStart += OnLevelStart;
        LevelManager.OnLevelEnd += OnLevelEnd;
        
        LevelManager.OnPlayerTurnStarted += OnPlayerTurnStarted;
        LevelManager.OnPlayerTurnCompleted += OnPlayerTurnCompleted;
    }

    private void OnDisable()
    {
        LevelManager.OnEnemyTurnStarted -= OnEnemyTurnStarted;
        LevelManager.OnEnemyTurnCompleted -= OnEnemyTurnCompleted;
        LevelManager.OnLevelStart -= OnLevelStart;
        LevelManager.OnLevelEnd -= OnLevelEnd;
        
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
        ProgressBar.SetValue01(0f, true, 0.5f);
        PlayerUI.SpawnSpellSlots(LevelManager.Instance.LevelSpells);
    }

    void OnLevelEnd()
    {
        ProgressBar.SetValue01(1f, true, 0.5f);
        EndLevelUI.Init(LevelManager.Instance.IsLevelCompleted());
    }


    void OnPlayerTurnStarted()
    {
        TurnCounterText.text = $"TURN: {LevelManager.Instance.CurrentTurn}";
        TurnText.text = "PLAYER TURN";
        PlayerUI.SetInteractable(true);
        GridManager.Instance.ShowInteratable(LevelManager.Instance.GetActiveSpellData().Action);
    }
    void OnPlayerTurnCompleted()
    {
        GridManager.Instance.HideInteratableVisual();
    }
    void OnEnemyTurnStarted()
    {
        TurnText.text = "ENEMY TURN";
    }
    void OnEnemyTurnCompleted()
    {
        Debug.Log(LevelManager.Instance.GetLevelProgress());
        //var percent = LevelManager.Instance.GetLevelProgress();
        ProgressBar.SetValue01(LevelManager.Instance.GetLevelProgress(), true, 0.5f);
    }
    
}
