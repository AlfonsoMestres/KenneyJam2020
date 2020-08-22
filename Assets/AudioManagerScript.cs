using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{
    public AudioSource clickUIStatIncrease;
    public AudioSource clickTap;

    public void PlayStatIncreaseSound()
    {
        clickUIStatIncrease.Play();
    }

    public void PlayButtonTap()
    {
        clickTap.Play();
    }
}
