﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
//using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : Singleton<LevelManager>
{

    
    [Header("Level Config: ")]
    
    public int TotalTurns;
    public List<SpellData> LevelSpells;
    public int InitialNaturePoints;
    public int NaturePointsPerTurn;


    [Header("Debug state:")] 
    public int TotalObjectiveLife;

    public int CurrentObjectiveLife;
    public int SelectedSpellID;
    public int CurrentNaturePoints;
    public LevelState CurrentState;
    public bool WatingForPlayer;
    public bool WatingForGrid;
    public int CurrentTurn;
    private bool _isPlaying;

    public AudioSource DefaultSoundtrack;
    public AudioSource BadSoundtrack;
    

    public AudioClip sonido_ganar_vida;
    public GameObject particula_vida_extra;

    public AudioSource AudioSource;

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
        ResetLevel();
        AudioSource = GetComponent<AudioSource>();
        _isPlaying = true;
        CurrentState = LevelState.PlayerTurn;
        CurrentNaturePoints = InitialNaturePoints;
        SelectedSpellID = LevelSpells[0].SpellID;
    }

    private IEnumerator LevelUpdate()
    {
        while (_isPlaying)
        {
            switch (CurrentState)
            {
                case LevelState.Idle: Debug.Log("Idle State"); break;
                
                case LevelState.EnemyTurn: yield return StartCoroutine(EnemyTurn()); break;
                
                case LevelState.PlayerTurn:

                    if (GameObject.Find("GridManager").GetComponent<GridManager>() != null) {
                        GameObject.Find("GridManager").GetComponent<GridManager>().HideInteratableVisual();
                    }

                    yield return StartCoroutine(PlayerTurn()); 

                    if (GameObject.Find("GridManager").GetComponent<GridManager>() != null) {
                        GameObject.Find("GridManager").GetComponent<GridManager>().HideInteratableVisual();
                    }

                    foreach (Transform item in GameObject.Find("SpellBar").transform) {
                        item.GetChild(1).GetComponent<Toggle>().isOn = false;
                    }

                    yield return new WaitForSeconds(0.1f);

                    if (GameObject.Find("GridManager").GetComponent<GridManager>() != null) {
                        GameObject.Find("GridManager").GetComponent<GridManager>().HideInteratableVisual();
                    }
                    break;
                

                // HideInteratableVisual();

                case LevelState.LevelEnd:
                    //UIManager.Set
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
        CurrentTurn = 1;
        WatingForPlayer = false;
        CurrentObjectiveLife = 0;
        TotalObjectiveLife = 0;

    }

    private IEnumerator EnemyTurn()
    {
        Debug.Log("[ENEMY TURN] STARTED");

        OnEnemyTurnStarted?.Invoke();
        
        yield return StartCoroutine(SubTurnManager.Instance.SubLoop());

        if (GetDestroyProgress() <= 0.5 && DefaultSoundtrack.volume > 0.1)
        {
            StartCoroutine(CrossFadeSoundTrack());
        } 
        
        if (IsLevelCompleted() || IsLevelFailed())
        {
            CurrentState = LevelState.LevelEnd;
        }
        else
        {
            CurrentTurn++;
            
            CurrentState = LevelState.PlayerTurn;
            
            yield return AddNaturePoints();
            
            OnEnemyTurnCompleted?.Invoke();
        }
        
        yield return new WaitForSeconds(1f);
    }

       // yield return new WaitForSeconds(0.5f);

    public IEnumerator AddNaturePoints()
    {
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("Plant"))
        {
            if (item != null && item.GetComponent<Add_nature_points_end_of_round>() != null)
            {
                
                if (item.GetComponent<LifeComponent>() != null) 
                {
                    if (item.GetComponent<LifeComponent>().actualQuantity == item.GetComponent<LifeComponent>().maxQuantity)
                    {

                        // Creamos la partícula de vida
                        if (particula_vida_extra != null) 
                        {
                            GameObject created_particle = GameObject.Instantiate(particula_vida_extra, item.transform.position, Quaternion.identity);
                            
                            var text = created_particle.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>();
                            
                            if(text != null) text.text = "+" + item.GetComponent<Add_nature_points_end_of_round>().to_add.ToString();
                        }
                        
                        // Esperamos un cachín y hacemos el resto
                        yield return new WaitForSeconds(0.5f);

                        if (sonido_ganar_vida != null)
                        {
                            AudioSource.PlayOneShot(sonido_ganar_vida);
                        }

                        if (item != null)
                        {
                            CurrentNaturePoints += item.GetComponent<Add_nature_points_end_of_round>().to_add;
                        }
                    }
                }
                else
                {

                    // Creamos la partícula de vida
                    if (particula_vida_extra != null) {
                        GameObject created_particle = GameObject.Instantiate(particula_vida_extra, item.transform.position, Quaternion.identity);
                        created_particle.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = "+" + item.GetComponent<Add_nature_points_end_of_round>().to_add.ToString();
                    }
                    // Esperamos un cachín y hacemos el resto
                    yield return new WaitForSeconds(0.5f);
                    if (sonido_ganar_vida != null) { AudioSource.PlayOneShot(sonido_ganar_vida); }
                    CurrentNaturePoints += item.GetComponent<Add_nature_points_end_of_round>().to_add;

                }






            }
        }
    }
    
    private IEnumerator PlayerTurn()
    {
        WatingForPlayer = true;
        CurrentNaturePoints += NaturePointsPerTurn;
        Debug.Log("[PLAYER TURN] STARTED");

        OnPlayerTurnStarted?.Invoke();

        if (GameObject.Find("GridManager").GetComponent<GridManager>() != null) {
            GameObject.Find("GridManager").GetComponent<GridManager>().HideInteratableVisual();
        }

        yield return new WaitWhile(() => WatingForPlayer);
        
        OnPlayerTurnCompleted?.Invoke();
        
        CurrentState = LevelState.EnemyTurn;
        
        Debug.Log("[PLAYER TURN] ENDED");
        yield return null;
    }

    public bool IsLevelCompleted()
    {
        
        //LOGIC FOR TURN BASED
        return CurrentTurn >= TotalTurns && !IsLevelFailed() ;

    }

    public bool IsLevelFailed()
    {
        return CurrentObjectiveLife <= 0;
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

    public float GetLevelProgress()
    {
        return ((((CurrentTurn - 1) * 100f) / (TotalTurns )) / 100f) ;
    }

    public float GetDestroyProgress()
    {
        return (((CurrentObjectiveLife * 100f) / (TotalObjectiveLife )) / 100f) ;
    }

    public IEnumerator CrossFadeSoundTrack()
    {
        var duration = new Duration(6f);
        
        while (!duration.IsDone)
        {
            DefaultSoundtrack.volume -= duration.Progress01 * Time.deltaTime;
            BadSoundtrack.volume += duration.Progress01 * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}


public enum LevelState
{
    Idle = 0,
    EnemyTurn = 1,
    PlayerTurn = 2,
    LevelEnd = 3
}

