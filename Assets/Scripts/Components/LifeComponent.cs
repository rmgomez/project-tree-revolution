using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeComponent : MonoBehaviour
{
    public delegate void Event();

    public Event EventLifeChange;


    [Range(0, 10)]
    public int maxQuantity = 1;
    [HideInInspector]
    public int actualQuantity;

    public bool isAlive = true;

    public bool KeepLastVisual = false;
    public bool IsObjective;
    
    public bool DisplayLife;
    public GameObject DisplayLifeObject;
    private TextMesh LifeText;
    private void Awake()
    {
        actualQuantity = maxQuantity;

        if (DisplayLife)
        {
            LifeText = Instantiate(DisplayLifeObject, transform).GetComponent<TextMesh>();
            LifeText.text = actualQuantity.ToString();
        }
    }
    
    private void OnEnable()
    {
        LevelManager.OnLevelStart += OnLevelStart;
    }

    private void OnDisable()
    {
        LevelManager.OnLevelStart -= OnLevelStart;
    }

    public bool CanBeHeal
    {
        get
        {
            return actualQuantity < maxQuantity;
        }
    }

    public void GetHeal(int heal)
    {
        actualQuantity = heal + actualQuantity > maxQuantity ? maxQuantity : heal + actualQuantity;
        if (DisplayLife) LifeText.text = actualQuantity.ToString();
        EventLifeChange?.Invoke();
    }

    public void GetDamage(int damage)
    {
        if(IsObjective && isAlive) LevelManager.Instance.CurrentObjectiveLife -= damage;
        actualQuantity = damage > actualQuantity ? 0 : actualQuantity - damage;
        if (DisplayLife) LifeText.text = actualQuantity.ToString();

        EventLifeChange?.Invoke();

        if (actualQuantity == 0)
        {
            Death();
        }
    }

    public void Death()
    {
        isAlive = false;
    }

    void OnLevelStart()
    {
        if (IsObjective)
        {
            LevelManager.Instance.TotalObjectiveLife += maxQuantity;
            LevelManager.Instance.CurrentNaturePoints += maxQuantity;
        }
    }
}
