using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanWalkOnIt : MonoBehaviour
{
	public WalkReaction walkReaction;

	public GameObject object_to_replace;

	public IEnumerable OnWalkOnIt()
	{
		yield return null;
	}
}
