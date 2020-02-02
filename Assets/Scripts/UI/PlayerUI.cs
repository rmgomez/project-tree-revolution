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

    public AudioClip sonido_next_turn;

    
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
        if (GameObject.Find("LevelManager") != null && GameObject.Find("LevelManager").GetComponent<AudioSource>() != null && sonido_next_turn)
        {
            GameObject.Find("LevelManager").GetComponent<AudioSource>().PlayOneShot(sonido_next_turn);
        }
        LevelManager.Instance.WatingForPlayer = false;
        SetInteractable(false);
    }
}
