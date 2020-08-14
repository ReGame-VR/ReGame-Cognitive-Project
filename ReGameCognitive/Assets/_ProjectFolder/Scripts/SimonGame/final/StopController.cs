using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopController : MonoBehaviour
{
    [SerializeField] private SimonGame simonGame;
    [SerializeField] private Feedback stopFeedback;
    [SerializeField] private GameObject difficultyColliderParent;
    [SerializeField] private GameObject[] buttonModelGameObjects;
    [SerializeField] private float lightUpInterval = 4;
    [SerializeField] private float startUpBuffer = .1f;


    public void PlayStopSequence()
    {
        StartCoroutine(PlayStopSequenceCoroutine());
    }

    public void PlayStopSequenceAndChooseDifficulty()
    {
        StartCoroutine(PlayStopSequenceAndChooseDifficultyCoroutine());
    }

    private IEnumerator PlayStopSequenceCoroutine()
    {
        yield return new WaitForSeconds(startUpBuffer);

        LightUpStopCubes();

        yield return new WaitForSeconds(lightUpInterval);

        //SetupDifficultyButtons();
    }
    
    private IEnumerator PlayStopSequenceAndChooseDifficultyCoroutine()
    {
        yield return new WaitForSeconds(startUpBuffer);

        LightUpStopCubes();

        yield return new WaitForSeconds(lightUpInterval);

        //SetupDifficultyButtons();
    }

    public void LightUpStopCubes()
    {
        for (var i = 0; i < 12; i++)
        {
            simonGame.PlayFeedback(i, stopFeedback, false);
            buttonModelGameObjects[i].SetActive(true);
        }
    }

    private void SetupDifficultyButtons()
    {
        if (difficultyColliderParent) difficultyColliderParent.SetActive(true);

        EnableDifficultyCubes();
        DisableNonDifficultyCubes();
    }

    private void DisableNonDifficultyCubes()
    {
        for (var i = 5; i < 12; i++)
        {
            buttonModelGameObjects[i].SetActive(false);
        }
    }

    private void EnableDifficultyCubes()
    {
        for (var i = 0; i < 5; i++)
        {
            simonGame.PlaySilentFeedback(i);
            buttonModelGameObjects[i].SetActive(true);
        }
    }
}
