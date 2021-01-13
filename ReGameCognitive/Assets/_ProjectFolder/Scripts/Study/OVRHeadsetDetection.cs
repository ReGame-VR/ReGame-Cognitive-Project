using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OVRHeadsetDetection : MonoBehaviour
{
    private bool _headsetHasBeenMounted = false;
    [SerializeField] private float intervalTime = 0.5f;
    [SerializeField] private CustomTextCanvas customTextCanvas;
    [SerializeField] private CustomButton customButton;
    [SerializeField] private GameObject instructionsPanelParent;
    
    public Action HeadsetWasUnmounted;
    public Action HeadsetWasMounted;

    
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
    
    public IEnumerator EnableDetection(bool isVrVersion)
    {
        var detectionCoroutine = isVrVersion ? HeadsetDetection(intervalTime) : InputDetection();
        yield return StartCoroutine(detectionCoroutine);
    }
    
    private IEnumerator HeadsetDetection(float timeToWait)
    {
        instructionsPanelParent.SetActive(true);
        customTextCanvas.SetBody("Please take your headset off\n" +
                                 " to talk to the researcher.");
        
        HeadsetWasUnmounted?.Invoke();
        
        while (_headsetHasBeenMounted)
        {
            yield return new WaitForSeconds(timeToWait);
        }

        customTextCanvas.SetBody("Touch the white circle to continue.");
        customButton.Enable();

        while (!_headsetHasBeenMounted)
        {
            yield return new WaitForSeconds(timeToWait);
        }
        
        HeadsetWasMounted?.Invoke();
    }

    private IEnumerator InputDetection()
    {
        instructionsPanelParent.SetActive(true);
        customTextCanvas.SetBody("Press the Space bar to continue.");
        yield return null;
    }
}
