using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundComponent : MonoBehaviour
{
    [SerializeField]
    private AudioClip soundPutOnGrid;
    public void PlayPlaceOnGrid()
    {
        if (soundPutOnGrid)
        {
            LevelManager.Instance.AudioSource.PlayOneShot(soundPutOnGrid);
        }
    }

    [SerializeField]
    private AudioClip soundOnDeath;
    public void PlayOnDeath()
    {
        if (soundOnDeath)
        {
            LevelManager.Instance.AudioSource.PlayOneShot(soundOnDeath);
        }
    }

    [SerializeField]
    private AudioClip cuandoSeBorra;
    public void PlayCuandoSeBorra()
    {
        if (cuandoSeBorra)
        {
            LevelManager.Instance.AudioSource.PlayOneShot(cuandoSeBorra);
        }
    }
}
