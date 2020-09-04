using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseDifficulty : MonoBehaviour
{
    [SerializeField] private SimonGame simonGame;
    [SerializeField] private Difficulty difficulty;
    [SerializeField] private KeyCode difficultyKey;

    private void OnEnable()
    {
        ButtonSwitch();
        StartCoroutine(WaitForInput(difficultyKey));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!simonGame) return;
        
        simonGame.SetDifficulty(difficulty);
    }

    private void ButtonSwitch()
    {
        switch (difficulty.level)
        {
            case 1:
                difficultyKey = KeyCode.B;
                break;
            case 2:
                difficultyKey = KeyCode.G;
                break;
            case 3:
                difficultyKey = KeyCode.R;
                break;
            case 4:
                difficultyKey = KeyCode.Y;
                break;
            case 5:
                difficultyKey = KeyCode.O;
                break;
            default:
                Debug.Log("Difficulty Key not found.");
                break;
        }
    }

    private IEnumerator WaitForInput(KeyCode key)
    {
        while (Input.GetKeyDown(key) == false)
        {
            yield return null;
        }
        
        simonGame.SetDifficulty(difficulty);
    }
}
