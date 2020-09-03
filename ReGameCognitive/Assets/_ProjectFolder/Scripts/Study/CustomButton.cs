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
    [SerializeField] private Collider collider;
    [SerializeField] private Collider leftHand;
    [SerializeField] private Collider rightHand;
    [SerializeField] private MeshRenderer renderer;
    [SerializeField] private Color activationColor;
    [SerializeField] private InputController inputController;
    public bool trigger = false;

    private void Start()
    {
        ToggleOffTrigger();
    }

    private void OnTriggerStay(Collider other)
    {
        //VrTrigger(other);
    }

    private void Update()
    {
        //needs to be disabled in VR version
        PcTrigger();
    }

    private void PcTrigger()
    {
        if (inputController.spacebarTrigger)
        {
            trigger = true;
        }
    }

    private void VrTrigger(Collider other)
    {
        if (currentTime >= timeUntilActivation)
        {
            trigger = true;
        }
        
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
    }

    private void ResetButton()
    {
        trigger = false;
        currentTime = 0;
        renderer.material.color = Color.white;
    }

    public void ToggleOffTrigger()
    {
        ResetButton();
        renderer.enabled = false;
        collider.enabled = false;
    }
    
    public void ToggleOnTrigger()
    {
        renderer.enabled = true;
        collider.enabled = true;
    }
}
