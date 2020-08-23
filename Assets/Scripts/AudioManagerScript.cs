using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{
    public AudioSource clickUIStatIncrease;
    public AudioSource clickTap;

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private GameController gameController;

    //Clips
    [SerializeField]
    private AudioClip startClip;

    [SerializeField]
    private AudioClip winClip;

    [SerializeField]
    private AudioClip loseClip;

    [SerializeField]
    private AudioClip missionCompletedClip;

    [SerializeField]
    private AudioClip missionFailedClip;

    [SerializeField]
    private AudioClip cursedTouchClip;

    private void Awake()
    {
        gameController.OnGameStarted.Subscribe(OnGameStarted);    
        gameController.OnGameFinished.Subscribe(OnGameFinished);
        gameController.OnGameFinishedText.Subscribe(OnGameFinishedText);
        gameController.OnCursedTouch.Subscribe(OnCursedTouch);
    }

    private void OnCursedTouch()
    {
        audioSource.clip = cursedTouchClip;
        audioSource.Play();
    }

    private void OnGameStarted()
    {
        audioSource.clip = startClip;
        audioSource.Play();
    }

    private void OnGameFinished(bool win)
    {
        if (win) { audioSource.clip = missionCompletedClip; }
        else { audioSource.clip = missionFailedClip; }
        audioSource.Play();
    }

    private void OnGameFinishedText(bool win)
    {
        if (win) { audioSource.clip = winClip; }
        else { audioSource.clip = loseClip; }
        audioSource.Play();
    }

    public void PlayStatIncreaseSound()
    {
        clickUIStatIncrease.Play();
    }

    public void PlayButtonTap()
    {
        clickTap.Play();
    }
}
