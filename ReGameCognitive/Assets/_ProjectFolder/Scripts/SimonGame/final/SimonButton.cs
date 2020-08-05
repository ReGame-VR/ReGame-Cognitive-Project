using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonButton : MonoBehaviour
{
    public FinalSimon FinalSimon;
    public int CubeNumber;
    
    
    private void OnTriggerEnter()
    {
        FinalSimon.buttonPushedIndex = CubeNumber;
        FinalSimon.PlayFeedback(CubeNumber - 1, true);
    }

    private void OnTriggerExit()
    {
        FinalSimon.buttonPushedIndex = 0;
        FinalSimon.StopFeedback(CubeNumber - 1);
    }
}
