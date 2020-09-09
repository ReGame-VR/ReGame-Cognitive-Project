using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] private SimonGame simonGame;
    
    public bool spacebarTrigger = false;

    private void Update()
    {
        if (!simonGame) return;
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            spacebarTrigger = true;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            spacebarTrigger = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            simonGame.ForceStartNextRound(false);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            simonGame.ForceStartNextRound(true);
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
