﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;

public class VideoPanel : MonoBehaviour
{
    [SerializeField] private SimonGame simonGame;
    [SerializeField] private InputController inputController;
    [SerializeField] public bool videoCompletion = false;
    [SerializeField] private GameObject displayPrefab;
    [SerializeField] private float intervalCheckingTime = 0.5f;
    [SerializeField] private CustomButton customButton;
    [SerializeField] private double _videoClipLength;
    [SerializeField] private double _elapsedTime = 0f;
    [SerializeField] private CustomTextCanvas customTextCanvas;
    
    
    private void Start()
    {
        if (displayPrefab)
        {
            VideoPlayer videoPlayer = displayPrefab.GetComponent<VideoPlayer>();

            if (videoPlayer)
            {
                _videoClipLength = videoPlayer.length;
            }
        }
    }

    public IEnumerator Enable(bool isVrVersion)
    {
        var activatorCoroutine = isVrVersion ? VrVideoActivator(intervalCheckingTime) : PcVideoActivator();
        yield return StartCoroutine(activatorCoroutine);
    }

    private IEnumerator VrVideoActivator(float timeToWait)
    {
        customTextCanvas.Enable();
        customTextCanvas.SetTitle("");
        customTextCanvas.SetBody("Watch the video on how to play the game.\n" +
                                 "When you have finished watching the video,\n" +
                                 "place and hold your hand in the white circle\n" +
                                 "to begin the practice round.");
           
        
        if (!displayPrefab.activeSelf)
        {
            displayPrefab.SetActive(true);
        }
        
        while (!videoCompletion)
        {
            if (_elapsedTime >= _videoClipLength)
            {
                customButton.ToggleOnTrigger();
            }
            
            if (customButton.trigger && (_elapsedTime >= _videoClipLength))
            {
                videoCompletion = true;
            }
            
            yield return new WaitForSeconds(timeToWait);
            _elapsedTime += Time.deltaTime + timeToWait;
        }

        customTextCanvas.SetBody("This is just a practice round and will not\n" +
                                 "have an impact on your score.");
        
        customButton.ToggleOffTrigger();
        displayPrefab.SetActive(false);
    }

    private IEnumerator PcVideoActivator()
    {
        customTextCanvas.Enable();
        customTextCanvas.SetTitle("");
        customTextCanvas.SetBody("Watch the video on how to play the game.\n" +
                                 "When you have finished watching the video,\n" +
                                 "press the Space bar to begin the practice round.");
           
        
        if (!displayPrefab.activeSelf)
        {
            displayPrefab.SetActive(true);
        }
        
        while (!videoCompletion)
        {
            if (inputController.spacebarTrigger)
            {
                videoCompletion = true;
            }

            yield return null;
        }

        customTextCanvas.SetBody("This is just a practice round and will not\n" +
                                 "have an impact on your score.");
        
        customButton.ToggleOffTrigger();
        displayPrefab.SetActive(false);
        yield return null;
    }

    public void ResetElapsedTime()
    {
        _elapsedTime = 0f;
    }
}
