using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PrefabList", menuName = "Data/PrefabList")]
public class PrefabListScriptableObject : ScriptableObject
{
    public Ground[] grounds;
    public Piece[] pieces;
}