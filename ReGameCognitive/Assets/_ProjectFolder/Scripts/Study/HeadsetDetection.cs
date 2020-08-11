using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OVRHeadsetDetection : MonoBehaviour
{
    public bool headsetHasBeenMounted = false;
    
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
        headsetHasBeenMounted = true;
    }

    private void HMDUnMounted()
    {
        headsetHasBeenMounted = false;
    }
}
