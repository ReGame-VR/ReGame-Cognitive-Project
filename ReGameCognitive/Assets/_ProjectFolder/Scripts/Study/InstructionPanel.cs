using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionPanel : MonoBehaviour
{
    [SerializeField] private CustomButton customButton;
    [SerializeField] private float _elapsedTime = 0f;
    [SerializeField] private float minTimeToRead = 5f;
    private bool hasReadInstructions = false;
    

    private IEnumerator InstructionsActivator(float timeToWait)
    {
        while (!hasReadInstructions)
        {
            _elapsedTime += Time.deltaTime;
            
            if (customButton.trigger && (_elapsedTime >= minTimeToRead))
            {
                hasReadInstructions = true;
            }
            
            yield return new WaitForSeconds(timeToWait);
        }
    }
    
    
}
