using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private LifeController _life;
    private MovementController _movement;
    private AttackController _attack;
    
    
    private void Start()
    {
        GetReferences();
    }

    private void GetReferences()
    {
        _life = GetComponent<LifeController>();
        _movement = GetComponent<MovementController>();
        _attack = GetComponent<AttackController>();
        
        
        if (_life == null || _movement== null || _attack == null) 
            Debug.LogError($"Missing Controllers for Enemy{gameObject.name}");
    }

    public void ExecuteAction(){}
     
    private void Move(){}
     
    private void Attack(){}
}
