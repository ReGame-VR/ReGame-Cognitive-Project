using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseDifficulty : MonoBehaviour
{
    [SerializeField] private SimonGame simonGame;
    [SerializeField] private Difficulty difficulty;

    private void OnTriggerEnter(Collider other)
    {
        if (!simonGame) return;
        
        simonGame.SetDifficulty(difficulty);
    }
}
