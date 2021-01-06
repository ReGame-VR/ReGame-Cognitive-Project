using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [SerializeField] private SimonGame simonGame;
    [SerializeField] private VideoPanel videoPanel;
    [SerializeField] private OVRHeadsetDetection headsetDetection;
    [SerializeField] private InstructionPanel instructionPanel;
    [SerializeField] public AudioSource auditoryStart;
    [SerializeField] public AudioSource instructionAudio;
    [SerializeField] public AudioClip practiceAudio;
    [SerializeField] private AudioClip tutorialAudio;
    [SerializeField] private AudioClip headsetOn;
    [SerializeField] private AudioClip headsetOff;
    [SerializeField] private AudioClip blueLevel;
    [SerializeField] private AudioClip greenLevel;
    [SerializeField] private AudioClip yellowLevel;
    [SerializeField] private AudioClip redLevel;
    [SerializeField] private AudioClip orangeLevel;
    [SerializeField] private AudioClip finalInstructionsClip;
    
    
    private void Awake()
    {
        if (simonGame)
        {
            simonGame.practiceAudio += PlayPracticeAudio;
            videoPanel.videoAudio += PlayTutorialAudio;
            headsetDetection.HeadsetWasMounted += PlayHeadsetOn;
            headsetDetection.HeadsetWasUnmounted += PlayHeadsetOff;
            simonGame.buttonInstruction += PlayLevelAudio;
            instructionPanel.finalInstructions += PlayFinalInstructions;
        }
    }

    private void OnDestroy()
    {
        simonGame.practiceAudio -= PlayPracticeAudio;
        videoPanel.videoAudio -= PlayTutorialAudio;
        simonGame.buttonInstruction -= PlayLevelAudio;
        instructionPanel.finalInstructions -= PlayFinalInstructions;
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

    private void PlayLevelAudio(Difficulty difficulty)
    {
        LevelAudioSwitcher(difficulty);
    }

    private void LevelAudioSwitcher(Difficulty difficulty)
    {
        switch (difficulty.colorString)
        {
            case "BLUE":
                instructionAudio.clip = blueLevel;
                instructionAudio.Play();
                break;
            case "RED":
                instructionAudio.clip = redLevel;
                instructionAudio.Play();
                break;
            case "GREEN":
                instructionAudio.clip = greenLevel;
                instructionAudio.Play();
                break;
            case "YELLOW":
                instructionAudio.clip = yellowLevel;
                instructionAudio.Play();
                break;
            case "ORANGE":
                instructionAudio.clip = orangeLevel;
                instructionAudio.Play();
                break;
        }
    }

    private void PlayFinalInstructions()
    {
        instructionAudio.clip = finalInstructionsClip;
        instructionAudio.Play();
    }
}
