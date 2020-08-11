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
    private double _videoClipLength;
    private float _elapsedTime = 0f;
    
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

        StartCoroutine(VideoActivator(intervalCheckingTime));
    }

    private IEnumerator VideoActivator(float timeToWait)
    {
        while (enabled)
        {
            _elapsedTime += Time.deltaTime;
            
            if (customButton.trigger && (_elapsedTime >= _videoClipLength))
            {
                videoCompletion = true;
            }
        
            yield return new WaitForSeconds(timeToWait);
        }
    }

    public void ResetElapsedTime()
    {
        _elapsedTime = 0f;
    }
}
