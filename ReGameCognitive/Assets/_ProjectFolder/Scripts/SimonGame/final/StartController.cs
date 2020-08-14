﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartController : MonoBehaviour
{
    [SerializeField] private SimonGame simonGame;
    [SerializeField] private Feedback greenFeedback;
    [SerializeField] private Feedback yellowFeedback;
    [SerializeField] private Feedback redFeedback;
    [SerializeField] private GameObject[] buttonModelGameObjects;
    [SerializeField] private float lightUpInterval = 1;

    private List<Feedback> _feedbacks = new List<Feedback>();

    
    void Awake()
    {
        _feedbacks = new List<Feedback>()
        {
            redFeedback,
            yellowFeedback,
            greenFeedback,
        };
    }

    public void PlayStartSequence()
    {
        SetColumnColors();
        StartCoroutine(PlaySequence());
    }
    
    private IEnumerator PlaySequence()
    {
        EnableCubes();
        
        yield return new WaitForSeconds(lightUpInterval);
        for (int i = 0; i < 3; i++)
        {
            if (simonGame)
            {
                PlayColumnFeedback(i);
            }
            yield return new WaitForSeconds(lightUpInterval);
        }
        yield return new WaitForSeconds(lightUpInterval);
        
        simonGame.StartGame();
    }

    private void EnableCubes()
    {
        foreach (var cube in buttonModelGameObjects)
        {
            cube.SetActive(true);
        }
    }

    private void PlayColumnFeedback(int i)
    {
        simonGame.PlayFeedback(i, _feedbacks[i], false);
        simonGame.PlayFeedback(i + 3, _feedbacks[i], false);
        simonGame.PlayFeedback(i + 6, _feedbacks[i], false);
        simonGame.PlayFeedback(i + 9, _feedbacks[i], false);
    }

    private void SetColumnColors()
    {
        for (int i = 0; i < 3; i++)
        {
            if (simonGame)
            {
                simonGame.StopFeedback(i, _feedbacks[i]);
                simonGame.StopFeedback(i + 3, _feedbacks[i]);
                simonGame.StopFeedback(i + 6, _feedbacks[i]);
                simonGame.StopFeedback(i + 9, _feedbacks[i]);
            }
        }
    }
}