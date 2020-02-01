using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GroundTypes
{
	Void,
	Green,
	Brown
}

public enum PieceTypes
{
	Nothing = 0,
	Three = 1,
	Bee = 2,
	EnnemyOne = 3,
	EnnemyTwo = 4,
	Lumberjack = 5,
	Lumberjack_Hat = 6,
}

public enum VisualTileInfos
{
	Hide,
	Valid,
	Invalid,
	Selected,
	Over
}

public enum PlayerActions
{
	Place,
	Heal,
	Attack
}