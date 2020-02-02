using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackComponent : MonoBehaviour
{
    [Range(0, 10)]
    public int damages = 1;

    public float animationDuration = 0;

    public IEnumerator Action()
    {
        float pourcent = 0;

        while (pourcent < 1 && animationDuration > 0)
        {
            pourcent += Time.deltaTime / animationDuration;

            yield return null;
        }
    }
}
