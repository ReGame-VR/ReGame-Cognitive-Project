using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonButton : MonoBehaviour
{
    [SerializeField] private ButtonData buttonData;
    [SerializeField] private FinalSimon finalSimon;
    
    
    private void OnTriggerEnter(Collider collider)
    {
        if (!finalSimon) return;
        
        finalSimon.ButtonPress(buttonData);
    }

    private void OnTriggerExit(Collider collider)
    {
        if (!finalSimon) return;
        
        finalSimon.ButtonRelease(buttonData);
    }
}
