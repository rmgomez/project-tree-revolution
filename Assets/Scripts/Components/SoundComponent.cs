using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundComponent : MonoBehaviour
{
    [SerializeField]
    private AudioClip soundPutOnGrid;
    public void PlayPlaceOnGrid()
    {
        if (soundPutOnGrid != null)
        {
            LevelManager.Instance.AudioSource.PlayOneShot(soundPutOnGrid);
        }
    }

    [SerializeField]
    private AudioClip soundOnDeath;
    public void PlayOnDeath()
    {
        if (soundOnDeath != null)
        {
            LevelManager.Instance.AudioSource.PlayOneShot(soundOnDeath);
        }
    }

    [SerializeField]
    private AudioClip cuandoSeBorra;
    public void PlayCuandoSeBorra()
    {
        if (cuandoSeBorra != null)
        {
            LevelManager.Instance.AudioSource.PlayOneShot(cuandoSeBorra);
        }
    }

    [SerializeField]
    private AudioClip cuandoExplota;
    public void PlayCuandoExplota()
    {
        if (cuandoExplota != null)
        {
            LevelManager.Instance.AudioSource.PlayOneShot(cuandoExplota);
        }
    }
}
