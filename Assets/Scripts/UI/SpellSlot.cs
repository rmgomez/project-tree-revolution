﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellSlot : MonoBehaviour
{
    public Image Icon;
    public Toggle ToggleButton;
    public Text NaturePointsText;
    public Text SpellNameText;
    public AudioClip select_this_button;

    private int _slotIndex;

    private SpellData _data;
    public void Init(SpellData data, ToggleGroup toggleGroup)
    {
        _data = data;
        Icon.sprite = data.ButtonIcon;
        NaturePointsText.text = data.NaturePoints.ToString("D2");
        SpellNameText.text = data.SpellName;
        ToggleButton.group = toggleGroup;
    }

    public void SelectSpell()
    {
        if (GameObject.Find("LevelManager") != null && GameObject.Find("LevelManager").GetComponent<AudioSource>() != null && select_this_button)
        {
            GameObject.Find("LevelManager").GetComponent<AudioSource>().PlayOneShot(select_this_button);
        }

        Debug.Log($"Is Slot {gameObject.name} on: {ToggleButton.isOn}");
        if (ToggleButton.isOn)
        {
            GridManager.Instance.ShowInteratable(_data.Action);
            LevelManager.Instance.SelectedSpellID = _data.SpellID;
        }
    }
}
