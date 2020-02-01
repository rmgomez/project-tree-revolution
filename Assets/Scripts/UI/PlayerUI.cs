using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour 
{

    public CanvasGroup CanvasGroup;

    public int MaxSlots = 5;

    public GameObject SpellSlotPrefab;

    public Transform SpellSlotRoot;

    public ToggleGroup ToggleGroup;
    //private int Selecte


    public void SetInteractable(bool isInteractable)
    {
        CanvasGroup.interactable = isInteractable;
        CanvasGroup.alpha = isInteractable ? 1f : 0f;
    }
    

    public void SpawnSpellSlots(List<SpellData> activeSpells)
    {
        for (int i = 0; i < activeSpells.Count && i < MaxSlots; i++)
        {
            var slot = Instantiate(SpellSlotPrefab, SpellSlotRoot).GetComponent<SpellSlot>();
            
            slot.Init(activeSpells[i], ToggleGroup);
            slot.gameObject.transform.localScale = Vector3.one;
        }
        
    }


    public void CompleteTurn()
    {
        LevelManager.Instance.WatingForPlayer = false;
        SetInteractable(false);
    }
}
