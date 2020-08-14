using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OVRHeadsetDetection : MonoBehaviour
{
    private bool _headsetHasBeenMounted = false;
    [SerializeField] private float intervalTime = 0.5f;

    private void Awake()
    {
        OVRManager.HMDMounted += HMDMounted;
        OVRManager.HMDUnmounted += HMDUnMounted;
    }

    private void OnDestroy()
    {
        OVRManager.HMDMounted -= HMDMounted;
        OVRManager.HMDUnmounted -= HMDUnMounted;
    }

    private void HMDMounted()
    {
        _headsetHasBeenMounted = true;
    }

    private void HMDUnMounted()
    {
        _headsetHasBeenMounted = false;
    }

    //This  might need to be changed to headsetHasBeenMounted depending on how you 
    //use it in your logic. Ex. return if headset has been place on or off. 
    private IEnumerator HeadsetDetection(float timeToWait)
    {
        while (!_headsetHasBeenMounted)
        {
            yield return new WaitForSeconds(timeToWait);
        }
    }
}
