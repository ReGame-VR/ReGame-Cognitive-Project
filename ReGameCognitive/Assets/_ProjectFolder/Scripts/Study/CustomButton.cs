using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomButton : MonoBehaviour
{
    [SerializeField] private float triggerTime;
    [SerializeField] private float timeUntilActivation;
    [SerializeField] private float hapticsAmplitude;
    [SerializeField] private float hapticsFrequency;
    public bool trigger = false;

    private void Start()
    {
        triggerTime = 0f;
    }

    //Currently hard coding hand collider values. Needs update
    private void OnTriggerStay(Collider other)
    {
        while (triggerTime < timeUntilActivation)
        {
            if (other.transform.parent.name == "CustomHandLeft")
            {
                triggerTime += Time.deltaTime;
                ControllerHaptics.ActivateHaptics(hapticsAmplitude, hapticsFrequency, true);
            }
        
            if (other.transform.parent.name == "CustomHandRight")
            {
                triggerTime += Time.deltaTime;
                ControllerHaptics.ActivateHaptics(hapticsAmplitude, hapticsFrequency, false);
            }
        }
        trigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        triggerTime = 0f;
        ControllerHaptics.ForceQuitOVRHaptics();
    }
}
