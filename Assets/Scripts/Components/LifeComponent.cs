using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeComponent : MonoBehaviour
{
    [Range(0, 10)]
    public int maxQuantity = 1;
    [HideInInspector]
    public int actualQuantity;

    public bool isAlive = true;

    private void Awake()
    {
        actualQuantity = maxQuantity;
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
    }

    public void GetDamage(int damage)
    {
        actualQuantity = damage > actualQuantity ? 0 : actualQuantity - damage;

        if (actualQuantity == 0)
        {
            Death();
        }
    }

    public void Death()
    {
        isAlive = false;
    }
}
