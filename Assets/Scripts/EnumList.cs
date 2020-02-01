using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GroundTypes
{
	Void          = 0,
	Green         = 1,
	Brown         = 2,
	AmbientGrass0 = 3,
	AmbientGrass1 = 4,
	AmbientGrass2 = 5,
	AmbientGrass3 = 6,
	AmbientGrass4 = 7,
}

public enum PieceTypes
{
	Nothing			= 0,

	// Threees
	ThreeMedium     = 1,
	ThreeBig0		= 8,
	ThreeBig1		= 9,
	ThreeBig2		= 10,
	Bee				= 2,
	River0			=14,
	River1 = 15,
	River2 = 16,

	// Enemies
	EnnemyOne		= 3,
	EnnemyTwo		= 4,
	Lumberjack		= 5,
	Lumberjack_Hat	= 6,

	// Other nature
	Pumpkin			= 7,
	Rock 			= 11,
	Sheep 			= 12,
	Honeycomb 		= 13,
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