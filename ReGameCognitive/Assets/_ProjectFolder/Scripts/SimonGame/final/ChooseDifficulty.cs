using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseDifficulty : MonoBehaviour
{
    [SerializeField] private FinalSimon finalSimon;
    [SerializeField] private Difficulty difficulty;

    private void OnTriggerEnter(Collider other)
    {
        if (!finalSimon) return;
        
        finalSimon.SetDifficulty(difficulty);
    }
}
