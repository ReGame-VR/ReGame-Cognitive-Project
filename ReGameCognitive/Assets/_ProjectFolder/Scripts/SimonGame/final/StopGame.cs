using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopGame : MonoBehaviour
{
    [SerializeField] private FinalSimon finalSimon;
    [SerializeField] private Feedback stopFeedback;

    public GameObject[] Cubes;
    public GameObject DifficultyButtons;
    public GameObject StopLights;
    
    
    void OnEnable()
    {
        StartCoroutine(PlaySequence());
    }

    private IEnumerator PlaySequence()
    {
        LightUpStopCubes();

        yield return new WaitForSeconds(4);

        SetupDifficultyButtons();
        StopLights.SetActive(false);
    }

    private void LightUpStopCubes()
    {
        for (int i = 0; i < 12; i++)
        {
            finalSimon.PlayFeedback(i, stopFeedback, false);
            Cubes[i].SetActive(true);
        }
    }

    private void SetupDifficultyButtons()
    {
        EnableDifficultyCubes();
        DisableNonDifficultyCubes();
        DifficultyButtons.SetActive(true);
    }

    private void DisableNonDifficultyCubes()
    {
        for (int i = 5; i < 12; i++)
        {
            Cubes[i].SetActive(false);
        }
    }

    private void EnableDifficultyCubes()
    {
        for (int i = 0; i < 5; i++)
        {
            finalSimon.PlaySilentFeedback(i);
            Cubes[i].SetActive(true);
        }
    }
}
