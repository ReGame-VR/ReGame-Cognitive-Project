﻿using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class StudyManager : MonoBehaviour
{
    [SerializeField] private SimonGame simonGame;
    [SerializeField] private VideoPanel videoPanel;
    
    private CSVManager _userCsvManager;
    private CSVManager _sessionCsvManager;
    private User _currentUser;
    private Session _currentSession;


    private void Awake()
    {
        _userCsvManager = gameObject.AddComponent<CSVManager>();
        _sessionCsvManager = gameObject.AddComponent<CSVManager>();
        
        if (simonGame)
        {
            simonGame.SessionHasStarted += StartSession;
            simonGame.SessionHasEnded += EndSession;
            simonGame.RoundHasEnded += StoreData;
            simonGame.RoundHasEnded += AppendSessionData;
            
            simonGame.SetUser(_currentUser);
        }
        
        StartStudy();
    }

    private void OnDestroy()
    {
        if (simonGame)
        {
            simonGame.SessionHasStarted -= StartSession;
            simonGame.SessionHasEnded -= EndSession;
            simonGame.RoundHasEnded -= StoreData;
            simonGame.RoundHasEnded -= AppendSessionData;
        }
    }

    [Button]
    private void StartStudy()
    {
        _currentUser = new User();
        
        if (_userCsvManager) _userCsvManager.Initialize(_currentUser);

        StartCoroutine(StartStudyCoroutine());
    }
    
    [Button]
    private void EndStudy()
    {
        if (_userCsvManager == null) return;
        
        if (_currentUser != null)
        {
            _currentUser.SetEndTime();
            _currentUser.studyCompleted = true;
        }
        
        StoreData();
        _userCsvManager.AppendReport();
    }

    private void StartSession()
    {
        _currentSession = new Session(_currentUser);
        
        if (_sessionCsvManager) _sessionCsvManager.Initialize(_currentSession);
        if (simonGame) simonGame.SetSession(_currentSession);
    }

    private void EndSession()
    {
        StoreData();
        if (_sessionCsvManager) _sessionCsvManager.AppendReport();
        
        _currentSession = null;
    }

    private void StoreData()
    {
        if (_userCsvManager) _userCsvManager.UpdateData(_currentUser);
        if (_sessionCsvManager) _sessionCsvManager.UpdateData(_currentSession);
    }

    private void AppendSessionData()
    {
        if (_sessionCsvManager) _sessionCsvManager.AppendReport();
    }

    private IEnumerator StartStudyCoroutine()
    {
        if (!videoPanel || !simonGame) yield break;
        
        yield return StartCoroutine(videoPanel.Enable());
        
        simonGame.Activate();
    }
}
