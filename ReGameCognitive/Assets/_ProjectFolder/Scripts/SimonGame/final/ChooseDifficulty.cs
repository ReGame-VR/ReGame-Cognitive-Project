using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseDifficulty : MonoBehaviour
{
    public int DifficultyNumber;
    public int StartAtSequence;
    public FinalSimon FinalSimon;
    public GameObject DifficultyButtons;
    public GameObject StartLight;
    public float timeLimit;
    public LevelColors levelColors;

    private void OnTriggerEnter(Collider other)
    {
        if (levelColors) levelColors.ChangeAllObjectsMaterial();

        if (FinalSimon)
        {
            FinalSimon.NumberOfButtons = DifficultyNumber;
            FinalSimon.numSequences = StartAtSequence;
            FinalSimon.timeLimit = timeLimit;
        }
        
        if (DifficultyButtons) DifficultyButtons.SetActive(false);
        if (StartLight) StartLight.SetActive(true);
    }
}
