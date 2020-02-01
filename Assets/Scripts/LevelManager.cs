using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
//using UnityEditor.VersionControl;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{

    
    [Header("Level Config: ")]
    
    public int TotalTurns;
    public List<SpellData> LevelSpells;
    public int InitialNaturePoints;
    public int NaturePointsPerTurn;

    
    [Header("Debug state:")] 
    
    public int SelectedSpellID;
    public int CurrentNaturePoints;
    public LevelState CurrentState;
    public bool WatingForPlayer;
    public bool WatingForGrid;
    public int CurrentTurn { get; private set; }
    private bool _isPlaying;
    

    #region EVENTS
    public static event Action OnLevelStart;
    public static event Action OnLevelEnd;
    
    public static event Action OnEnemyTurnStarted;
    public static event Action OnEnemyTurnCompleted;
    
    public static event Action OnPlayerTurnStarted;
    public static event Action OnPlayerTurnCompleted;
    #endregion
    
    private void Start()
    {
        Init();
        
        StartCoroutine(LevelUpdate());
        OnLevelStart?.Invoke();
    }

    public void Init()
    {
        _isPlaying = true;
        CurrentState = LevelState.PlayerTurn;
        CurrentNaturePoints = InitialNaturePoints;
    }

    private IEnumerator LevelUpdate()
    {
        while (_isPlaying)
        {
            switch (CurrentState)
            {
                case LevelState.Idle: Debug.Log("Idle State"); break;
                
                case LevelState.EnemyTurn: yield return StartCoroutine(EnemyTurn()); break;
                
                case LevelState.PlayerTurn:yield return StartCoroutine(PlayerTurn()); break;
                
                case LevelState.LevelEnd:
                    Debug.Log("Level Completed, go to endScreen");
                    OnLevelEnd?.Invoke();
                    _isPlaying = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            yield return new WaitForEndOfFrame();
        }
       
        
        yield return null;
    }


    void ResetLevel()
    {
        //RESET LEVEL STATE
        CurrentTurn = 0;
        WatingForPlayer = false;
        
    }

    private IEnumerator EnemyTurn()
    {
        Debug.Log("[ENEMY TURN] STARTED");

        OnEnemyTurnStarted?.Invoke();


        yield return StartCoroutine(SubTurnManager.Instance.SubLoop());
        
       // yield return new WaitForSeconds(2f);
        
        //yield return new WaitWhile(() => WaitingForGrid);
        
        /*foreach (var enemy in _activeEnemies)
        {
            enemy.ExecuteAction();
            yield return new WaitForSeconds(1f);
        }*/
        
        OnEnemyTurnCompleted?.Invoke();
        Debug.Log("[ENEMY TURN] COMPLETED");
        CurrentTurn++;
        
        CurrentState = IsLevelCompleted() ? LevelState.LevelEnd : LevelState.PlayerTurn;
        
        yield return new WaitForSeconds(2f);

    }
    
    private IEnumerator PlayerTurn()
    {
        WatingForPlayer = true;
        CurrentNaturePoints += NaturePointsPerTurn;
        Debug.Log("[PLAYER TURN] STARTED");
        
        OnPlayerTurnStarted?.Invoke();
        
        yield return new WaitWhile(() => WatingForPlayer);
        
        OnPlayerTurnCompleted?.Invoke();
        
        CurrentState = LevelState.EnemyTurn;
        
        Debug.Log("[PLAYER TURN] ENDED");
        yield return null;
    }

    public bool IsLevelCompleted()
    {
        
        //LOGIC FOR TURN BASED
        return CurrentTurn >= TotalTurns;

    }

    public SpellData GetActiveSpellData()
    {
        return GetSpellData(SelectedSpellID);
    }

    public bool CanCastActiveSpell()
    {
        return GetActiveSpellData().NaturePoints <= CurrentNaturePoints;
    }
    
    public SpellData GetSpellData(int spellID)
    {
       var temp =  LevelSpells.Find((data => data.SpellID == spellID));

       if (temp == null) Debug.LogError($"No Spell Found with ID: {spellID}");
       
       return temp;

    }

}


public enum LevelState
{
    Idle = 0,
    EnemyTurn = 1,
    PlayerTurn = 2,
    LevelEnd = 3
}

