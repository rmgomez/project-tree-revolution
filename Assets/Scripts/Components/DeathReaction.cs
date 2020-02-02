using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathReaction : MonoBehaviour
{
    public float animationDuration = 0;

    public IEnumerator Action()
    {
        float pourcent = 0;

        while (pourcent < 1 && animationDuration > 0)
        {
            pourcent += Time.deltaTime / animationDuration;

            yield return null;
        }

        GetComponent<SoundComponent>()?.PlayOnDeath();
    }
}
