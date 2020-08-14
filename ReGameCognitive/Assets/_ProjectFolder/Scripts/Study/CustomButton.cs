using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomButton : MonoBehaviour
{
    [SerializeField] private float currentTime;
    [SerializeField] private float timeUntilActivation;
    [SerializeField] private float hapticsAmplitude;
    [SerializeField] private float hapticsFrequency;
    [SerializeField] private Collider leftHand;
    [SerializeField] private Collider rightHand;
    [SerializeField] private MeshRenderer renderer;
    [SerializeField] private Color activationColor;
    public bool trigger = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.name == leftHand.name && (currentTime < timeUntilActivation))
        {
            ControllerHaptics.ActivateHaptics(hapticsAmplitude, hapticsFrequency, true);
            currentTime += Time.deltaTime;
        }
        
        if (other.transform.name == rightHand.name && (currentTime < timeUntilActivation))
        {
            ControllerHaptics.ActivateHaptics(hapticsAmplitude, hapticsFrequency, false);
            currentTime += Time.deltaTime;
        }
        renderer.material.color = Color.Lerp(Color.white, activationColor, currentTime);
        trigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        currentTime = 0;
        renderer.material.color = Color.white;
    }
}
