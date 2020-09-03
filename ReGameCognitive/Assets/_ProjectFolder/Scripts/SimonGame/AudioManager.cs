using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [SerializeField] private SimonGame simonGame;
    [SerializeField] private AudioSource distractionAudio;
    [SerializeField] public AudioSource auditoryStart;

    private void Start()
    {
        if (simonGame)
        {
            simonGame.DifficultyWasSet += SetDistractions;
            simonGame.SessionHasEnded += DisableDistractions;
            
        }
    }

    private void OnDestroy()
    {
        simonGame.DifficultyWasSet -= SetDistractions;
        simonGame.SessionHasEnded -= DisableDistractions;
    }

    private void SetDistractions(Difficulty difficulty)
    {
        if (!difficulty)
            return;

        if (difficulty.level == 5)
        {
            distractionAudio.Play();
        }
    }

    private void DisableDistractions()
    {
        distractionAudio.Stop();
    }
}
