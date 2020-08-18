using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class StudyManager : MonoBehaviour
{
    [SerializeField] private SimonGame simonGame;
    [SerializeField] private VideoPanel videoPanel;
    [SerializeField] private OVRHeadsetDetection headsetDetection;
    [SerializeField] private UserTriggerActivation triggerActivation;
    [SerializeField] private Difficulty tutorialDifficulty;
    [SerializeField] private Difficulty level1;
    [SerializeField] private Difficulty level2;
    [SerializeField] private Difficulty level3;
    [SerializeField] private Difficulty level4;
    [SerializeField] private Difficulty level5;

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
        if (simonGame) simonGame.SetUser(_currentUser);

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

        yield return StartCoroutine(videoPanel.Enable());                        //Watch instruction video
        yield return StartCoroutine(simonGame.PlayRound(tutorialDifficulty));    //Play practice round
        yield return StartCoroutine(headsetDetection.Enable());                  //start checking for headset
        yield return StartCoroutine(triggerActivation.Enable());                 //Wait for player to activate trigger
        yield return StartCoroutine(simonGame.PlayRound(level1));                //Play level 1
        yield return StartCoroutine(headsetDetection.Enable());                  //start checking for headset
        yield return StartCoroutine(triggerActivation.Enable());                 //Wait for player to activate trigger
        yield return StartCoroutine(simonGame.PlayRound(level2));                //Play level 2
        yield return StartCoroutine(headsetDetection.Enable());                  //start checking for headset
        yield return StartCoroutine(triggerActivation.Enable());                 //Wait for player to activate trigger
        yield return StartCoroutine(simonGame.PlayRound(level3));                //Play level 3
        yield return StartCoroutine(headsetDetection.Enable());                  //start checking for headset
        yield return StartCoroutine(triggerActivation.Enable());                 //Wait for player to activate trigger
        yield return StartCoroutine(simonGame.PlayRound(level4));                //Play level 4
        yield return StartCoroutine(headsetDetection.Enable());                  //start checking for headset
        yield return StartCoroutine(triggerActivation.Enable());                 //Wait for player to activate trigger
        yield return StartCoroutine(simonGame.PlayRound(level5));                //Play level 5
        yield return StartCoroutine(headsetDetection.Enable());                  //start checking for headset
        yield return StartCoroutine(triggerActivation.Enable());                 //Wait for player to activate trigger
        yield return StartCoroutine(simonGame.PlayRound());                    //Choose Difficulty, play round
        
        EndStudy();
    }
}
