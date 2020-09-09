using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [SerializeField] private SimonGame simonGame;
    [SerializeField] private VideoPanel videoPanel;
    [SerializeField] private OVRHeadsetDetection headsetDetection;
    [SerializeField] private AudioSource distractionAudio;
    [SerializeField] public AudioSource auditoryStart;
    [SerializeField] public AudioSource instructionAudio;
    [SerializeField] public AudioClip practiceAudio;
    [SerializeField] private AudioClip tutorialAudio;
    [SerializeField] private AudioClip headsetOn;
    [SerializeField] private AudioClip headsetOff;
    
    
    private void Awake()
    {
        if (simonGame)
        {
            simonGame.DifficultyWasSet += SetDistractions;
            simonGame.SessionHasEnded += DisableDistractions;
            simonGame.practiceAudio += PlayPracticeAudio;
            videoPanel.videoAudio += PlayTutorialAudio;
            headsetDetection.headsetOn += PlayHeadsetOn;
            headsetDetection.headsetOff += PlayHeadsetOff;
        }
    }

    private void OnDestroy()
    {
        simonGame.DifficultyWasSet -= SetDistractions;
        simonGame.SessionHasEnded -= DisableDistractions;
        simonGame.practiceAudio -= PlayPracticeAudio;
        videoPanel.videoAudio -= PlayTutorialAudio;
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

    private void PlayPracticeAudio()
    {
        instructionAudio.clip = practiceAudio;
        instructionAudio.Play();
    }

    private IEnumerator WaitUntilClipIsDone(AudioClip audioClip)
    {
        yield return new WaitForSeconds(audioClip.length);
    }

    private void PlayTutorialAudio()
    {
        instructionAudio.clip = tutorialAudio;
        instructionAudio.Play();
    }

    private void PlayHeadsetOn()
    {
        instructionAudio.clip = headsetOn;
        instructionAudio.Play();
    }

    private void PlayHeadsetOff()
    {
        instructionAudio.clip = headsetOff;
        instructionAudio.Play();
    }
}
