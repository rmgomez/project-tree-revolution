using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	private LifeComponent _life;
	private MovementComponent _movement;
	private AttackComponent _attack;


	private void Start()
	{
		GetReferences();
	}

	private void GetReferences()
	{
		_life = GetComponent<LifeComponent>();
		_movement = GetComponent<MovementComponent>();
		_attack = GetComponent<AttackComponent>();

		if (_life == null || _movement == null || _attack == null)
			Debug.LogError($"Missing Controllers for Enemy{gameObject.name}");
	}

	public void ExecuteAction() { }

	private void Move() { }

	private void Attack() { }
}
