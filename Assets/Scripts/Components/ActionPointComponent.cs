using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPointComponent : MonoBehaviour
{
    [Range(0, 10)]
    public int maxActionPoints = 1;
    [HideInInspector]
    public int actualActionPoints;

    private void Awake()
    {
        ResetAction();
    }

    public void ResetAction()
    {
        actualActionPoints = maxActionPoints;
    }
}
