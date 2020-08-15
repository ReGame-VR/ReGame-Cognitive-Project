using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;

public class VideoPanel : MonoBehaviour
{
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

    public IEnumerator Enable()
    {
        yield return StartCoroutine(VideoActivator(intervalCheckingTime));
    }

    private IEnumerator VideoActivator(float timeToWait)
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

    public void ResetElapsedTime()
    {
        _elapsedTime = 0f;
    }
}
