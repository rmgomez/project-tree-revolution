using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathReaction : MonoBehaviour
{
    private float animationDuration = 0.5f;

    public IEnumerator Action()
    {
        float pourcent = 0;

        while (pourcent < 1)
        {
            pourcent += Time.deltaTime / animationDuration;

            yield return null;
        }

        GetComponent<SoundComponent>()?.PlayOnDeath();
    }
}
