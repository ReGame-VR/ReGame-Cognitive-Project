using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonButton : MonoBehaviour
{
    [SerializeField] private ButtonData buttonData;
    [SerializeField] private SimonGame simonGame;
    
    
    private void OnTriggerEnter(Collider collider)
    {
        if (!simonGame) return;
        
        simonGame.ButtonPress(buttonData);
    }

    private void OnTriggerExit(Collider collider)
    {
        if (!simonGame) return;
        
        simonGame.ButtonRelease(buttonData);
    }
}
