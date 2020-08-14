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
        
        //StartCoroutine(VideoActivator(intervalCheckingTime));
    }

    public IEnumerator Enable()
    {
        yield return StartCoroutine(VideoActivator(intervalCheckingTime));
    }

    private IEnumerator VideoActivator(float timeToWait)
    {
        if (!displayPrefab.activeSelf)
        {
            displayPrefab.SetActive(true);
        }
        
        while (!videoCompletion)
        {
            _elapsedTime += Time.deltaTime;

            if (_elapsedTime >= _videoClipLength)
            {
                customButton.ToggleOnTrigger();
            }
            
            if (customButton.trigger && (_elapsedTime >= _videoClipLength))
            {
                videoCompletion = true;
            }
            
            yield return new WaitForSeconds(timeToWait);
        }
        
        customButton.ToggleOffTrigger();
        displayPrefab.SetActive(false);
    }

    public void ResetElapsedTime()
    {
        _elapsedTime = 0f;
    }
}
