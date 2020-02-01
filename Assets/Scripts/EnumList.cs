using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GroundTypes
{
	Void = 0,
	Green = 1,
	Brown = 2,
	AmbientGrass0 = 3,
	AmbientGrass1 = 4,
	AmbientGrass2 = 5,
	AmbientGrass3 = 6,
	AmbientGrass4 = 7,
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