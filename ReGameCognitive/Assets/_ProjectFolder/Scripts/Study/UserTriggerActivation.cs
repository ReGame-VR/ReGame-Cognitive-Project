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
        while (!customButton.wasButtonActivated)
        {
            yield return new WaitForSeconds(timeToWait);
        }

        instructionsPanelParent.SetActive(false);
        customButton.Disable();
    } 
    
    public IEnumerator Enable()
    {
        customButton.wasButtonActivated = false;
        
        yield return StartCoroutine(TriggerActivation(intervalTime));
    }
}
