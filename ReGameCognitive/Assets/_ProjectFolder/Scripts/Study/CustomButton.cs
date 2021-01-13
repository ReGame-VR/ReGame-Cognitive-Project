using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomButton : MonoBehaviour
{
    [SerializeField] private float timeUntilActivation;
    [SerializeField] private float hapticsAmplitude;
    [SerializeField] private float hapticsFrequency;
    [SerializeField] private Collider collider;
    [SerializeField] private Collider leftHand;
    [SerializeField] private Collider rightHand;
    [SerializeField] private MeshRenderer renderer;
    [SerializeField] private Color activationColor;
    [SerializeField] private InputController inputController;
    
    public bool wasButtonActivated;
    
    private float _timer;
    private Color _inactiveColor = Color.white;
    
    private bool HasTimeDelayElapsed => _timer >= timeUntilActivation;
    private bool IsActivationDelayActive => _timer < timeUntilActivation;
    private const float BUTTON_CHECK_INTERVAL_SECONDS = .5f;


    private void OnTriggerStay(Collider other)
    {
        VrTrigger(other);
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
            wasButtonActivated = true;
        }
    }

    private void VrTrigger(Collider other)
    {
        if (HasTimeDelayElapsed)
        {
            wasButtonActivated = true;
        }
        
        if (other.transform.name == leftHand.name && IsActivationDelayActive)
        {
            ControllerHaptics.ActivateHaptics(hapticsAmplitude, hapticsFrequency, true);
            _timer += Time.deltaTime;
        }
        
        if (other.transform.name == rightHand.name && IsActivationDelayActive)
        {
            ControllerHaptics.ActivateHaptics(hapticsAmplitude, hapticsFrequency, false);
            _timer += Time.deltaTime;
        }
        renderer.material.color = Color.Lerp(_inactiveColor, activationColor, _timer);
    }
    
    private void ResetButton()
    {
        wasButtonActivated = false;
        _timer = 0;
        renderer.material.color = _inactiveColor;
    }

    public void Disable()
    {
        ResetButton();
        renderer.enabled = false;
        collider.enabled = false;
    }
    
    public void Enable()
    {
        renderer.enabled = true;
        collider.enabled = true;
    }

    public IEnumerator WaitForButtonActivation()
    {
        ResetButton();
        Enable();

        while (!wasButtonActivated)
        {
            yield return new WaitForSeconds(BUTTON_CHECK_INTERVAL_SECONDS);
        }

        Disable();
    }
}
