using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackComponent : MonoBehaviour
{
    [Range(0, 10)]
    public int attackForce = 1;

    private float animationDuration = 0.1f;

    public IEnumerator Action()
    {
        float pourcent = 0;

        while (pourcent < 1)
        {
            pourcent += Time.deltaTime / animationDuration;

            yield return null;
        }
    }
}
