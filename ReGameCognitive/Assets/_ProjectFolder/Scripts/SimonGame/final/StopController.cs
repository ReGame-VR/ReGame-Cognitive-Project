﻿using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
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

        SetupDifficultyButtons();
    }

    public void LightUpStopCubes()
    {
        for (var i = 0; i < 12; i++)
        {
            simonGame.PlayFeedback(i, stopFeedback, false);
            buttonModelGameObjects[i].SetActive(true);
        }
    }

    public void SetupDifficultyButtons()
    {
        if (difficultyColliderParent) difficultyColliderParent.SetActive(true);

        EnableDifficultyCubes();
        DisableNonDifficultyCubes();
    }
    
    public void SetupDifficultyButtons(int level)
    {
        if (difficultyColliderParent) difficultyColliderParent.SetActive(true);

        EnableDifficultyCubes(level);
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
        
        for (var i = 0; i < difficultyColliderParent.transform.childCount; i++)
        {
            var colliderTransform = difficultyColliderParent.transform.GetChild(i);
            if (colliderTransform) colliderTransform.gameObject.SetActive(true);
        }
    }
    
    [Button]
    private void EnableDifficultyCubes(int level)
    {
        var levelIndex = level == 0 ? 0 : level - 1;
        for (var i = 0; i < 5; i++)
        {
            simonGame.PlaySilentFeedback(i);
            buttonModelGameObjects[i].SetActive(true);
        }

        for (var i = 0; i < difficultyColliderParent.transform.childCount; i++)
        {
            var colliderTransform = difficultyColliderParent.transform.GetChild(i);
            if (colliderTransform) colliderTransform.gameObject.SetActive(i == levelIndex);
        }
    }
}
