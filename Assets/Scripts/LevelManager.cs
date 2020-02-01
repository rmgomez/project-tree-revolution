using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEditor.VersionControl;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public LevelState CurrentState;

    private bool _isPlaying;


    private List<EnemyController> _activeEnemies;
    public List<EnemyController> ActiveEnemies
    {
        get => _activeEnemies;
        private set => _activeEnemies = value;
    } 


    public Action OnLevelStart;
    public Action OnLevelEnd;
    
    public Action OnEnemyTurnStarted;
    public Action OnEnemyTurnCompleted;
    
    public Action OnPlayerTurnStarted;
    public Action OnPlayerTurnCompleted;


    public bool WatingForPlayer;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }


    private void Start()
    {
        StartCoroutine(LevelUpdate());
        OnLevelStart?.Invoke();
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
        //RESET PLAYER STATE
        
        _activeEnemies.Clear();
        WatingForPlayer = false;
        
    }

    private IEnumerator EnemyTurn()
    {
        Debug.Log("[ENEMY TURN] STARTED");

        OnEnemyTurnStarted?.Invoke();
        
        foreach (var enemy in _activeEnemies)
        {
            enemy.ExecuteAction();
            yield return new WaitForSeconds(1f);
        }
        
        OnEnemyTurnCompleted?.Invoke();
        Debug.Log("[ENEMY TURN] COMPLETED");
        yield return new WaitForSeconds(2f);

    }
    
    private IEnumerator PlayerTurn()
    {
        Debug.Log("[PLAYER TURN] STARTED");
        OnPlayerTurnStarted?.Invoke();
        
        yield return new WaitWhile(() => WatingForPlayer);
        
        OnPlayerTurnCompleted?.Invoke();
        
        Debug.Log("[PLAYER TURN] ENDED");
        yield return null;
    }


    public void SuscribeEnemy(EnemyController enemyController)
    {
        _activeEnemies.Add(enemyController);
    }

    public void UnsuscribeEnemy(EnemyController enemyController)
    {
        _activeEnemies.Remove(enemyController);
    }

}


public enum LevelState
{
    Idle = 0,
    EnemyTurn = 1,
    PlayerTurn = 2,
    LevelEnd = 3
}

