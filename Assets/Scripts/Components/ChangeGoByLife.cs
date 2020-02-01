using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LifeComponent))]
public class ChangeGoByLife : MonoBehaviour
{
    public GameObject[] visualList;

    private LifeComponent lifeComponent;
    private void Awake()
    {
        lifeComponent = GetComponent<LifeComponent>();

        lifeComponent.EventLifeChange += ChangeVisual;
    }

    private void Start()
    {
        ChangeVisual();
    }

    private void ChangeVisual()
    {
        foreach (var item in visualList)
        {
            item.SetActive(false);
        }

        if (lifeComponent.actualQuantity == 0)
        {
            visualList[0].SetActive(true);
        }
        else if (visualList.Length > lifeComponent.actualQuantity - 1)
        {
            visualList[lifeComponent.actualQuantity - 1].SetActive(true);
        }
        else
        {
            visualList[visualList.Length - 1].SetActive(true);
        }
    }

}
