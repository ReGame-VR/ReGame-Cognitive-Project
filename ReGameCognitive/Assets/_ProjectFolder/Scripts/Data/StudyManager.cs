using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class StudyManager : MonoBehaviour
{
    [SerializeField] private SimonGame simonGame;

    private CSVManager _userCsvManager;
    private CSVManager _sessionCsvManager;
    private User _currentUser;
    private Session _currentSession;


    private void Awake()
    {
        if (simonGame)
        {
            simonGame.SessionHasStarted += StartSession;
            simonGame.SessionHasEnded += EndSession;
        }
        
        _userCsvManager = gameObject.AddComponent<CSVManager>();
        _sessionCsvManager = gameObject.AddComponent<CSVManager>();
    }

    private void OnDestroy()
    {
        if (simonGame)
        {
            simonGame.SessionHasStarted -= StartSession;
            simonGame.SessionHasEnded -= EndSession;
        }
    }

    [Button]
    private void StartStudy()
    {
        _currentUser = new User();
        
        if (_userCsvManager) _userCsvManager.Initialize(_currentUser);
        if (simonGame) simonGame.Activate();
    }
    
    [Button]
    private void EndStudy()
    {
        StoreData();
        if (_userCsvManager) _userCsvManager.AppendReport();
    }

    private void StartSession()
    {
        _currentSession = new Session(_currentUser);
        
        if (_sessionCsvManager) _sessionCsvManager.Initialize(_currentSession);
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
}
