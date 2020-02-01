using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellData_", menuName = "Data/Spell Data", order = 1)]
public class SpellData : ScriptableObject
{
    public int SpellID;
    public string SpellName;
    public int NaturePoints;
    public Sprite ButtonIcon;
    public PieceTypes PieceToSpawn;
    public int HealAmount;
    public int AttackAmount;
    public PlayerActions Action;

}
