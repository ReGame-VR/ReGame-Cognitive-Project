using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserTriggerActivation : MonoBehaviour
{
    [SerializeField] private CustomButton customButton;
    [SerializeField] private float intervalTime = 0.5f;
    [SerializeField] private GameObject instructionsPanelParent;
    

    private IEnumerator TriggerActivation(float timeToWait)
    {
        while (!customButton.trigger)
        {
            yield return new WaitForSeconds(timeToWait);
        }

        instructionsPanelParent.SetActive(false);
        customButton.ToggleOffTrigger();
    } 
    
    public IEnumerator Enable()
    {
        customButton.trigger = false;
        
        yield return StartCoroutine(TriggerActivation(intervalTime));
    }
}
