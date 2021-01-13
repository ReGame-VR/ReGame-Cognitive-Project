using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class StudyManager : MonoBehaviour
{
    [SerializeField] private SimonGame simonGame;
    [SerializeField] private VideoPanel videoPanel;
    [SerializeField] private InstructionPanel instructionPanel;
    [SerializeField] private OVRHeadsetDetection headsetDetection;
    [SerializeField] private UserTriggerActivation triggerActivation;
    [SerializeField] private CustomButton customButton;
    [SerializeField] private GameObject vrRig;
    [SerializeField] private GameObject CameraRig;
    [SerializeField] private Difficulty tutorialDifficulty;
    [SerializeField] private Difficulty level1;
    [SerializeField] private Difficulty level2;
    [SerializeField] private Difficulty level3;
    [SerializeField] private Difficulty level4;
    [SerializeField] private Difficulty level5;
    [SerializeField] private bool isVrVersion;
    
    private CSVManager _userCsvManager;
    private CSVManager _roundCsvManager;
    private User _currentUser;
    private Round _currentRound;
    private Difficulty _lastDifficultyChosen;
    

    private void Awake()
    {
        _userCsvManager = gameObject.AddComponent<CSVManager>();
        _roundCsvManager = gameObject.AddComponent<CSVManager>();
        
        if (simonGame)
        {
            simonGame.RoundHasStarted += StartRound;
            simonGame.RoundHasEnded += EndRound;
            simonGame.AttemptHasEnded += StoreData;
            simonGame.AttemptHasEnded += AppendRoundData;
            simonGame.DifficultyWasSet += SetDifficulty;
            
            simonGame.SetVersion(isVrVersion);
        }

        if (isVrVersion)
        {
            vrRig.SetActive(true);
            CameraRig.SetActive(false);
        }
        else
        {
            vrRig.SetActive(false);
            CameraRig.SetActive(true);
        }
        
        StartStudy();
    }

    private void OnDestroy()
    {
        if (simonGame)
        {
            simonGame.RoundHasStarted -= StartRound;
            simonGame.RoundHasEnded -= EndRound;
            simonGame.AttemptHasEnded -= StoreData;
            simonGame.AttemptHasEnded -= AppendRoundData;
            simonGame.DifficultyWasSet -= SetDifficulty;
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
        
        if (instructionPanel) instructionPanel.EndOfStudy(isVrVersion);
    }

    private void StartRound()
    {
        _currentRound = new Round(_currentUser);
        
        if (_roundCsvManager) _roundCsvManager.Initialize(_currentRound);
        if (simonGame) simonGame.SetRound(_currentRound);
    }

    private void EndRound()
    {
        //StoreData();
        if (_roundCsvManager) _roundCsvManager.AppendReport();
        
        _currentRound = null;
    }

    private void StoreData()
    {
        if (_userCsvManager) _userCsvManager.UpdateData(_currentUser);
        if (_roundCsvManager) _roundCsvManager.UpdateData(_currentRound);
    }

    private void AppendRoundData()
    {
        if (_roundCsvManager) _roundCsvManager.AppendReport();
    }

    private IEnumerator StartStudyCoroutine()
    {
        if (!videoPanel || !simonGame) yield break;

        yield return StartCoroutine(customButton.WaitForButtonActivation());                        //Delay before beginning video

        yield return StartCoroutine(videoPanel.Enable(isVrVersion));                        //Watch instruction video
        yield return StartCoroutine(simonGame.PlayTutorial(tutorialDifficulty)); //Play practice round
        yield return StartCoroutine(headsetDetection.EnableDetection(isVrVersion));         //start checking for headset
        yield return StartCoroutine(triggerActivation.Enable());                 //Wait for player to activate trigger
        yield return StartCoroutine(simonGame.PlayRound(level1));                //Play level 1
        yield return StartCoroutine(headsetDetection.EnableDetection(isVrVersion));         //start checking for headset
        yield return StartCoroutine(triggerActivation.Enable());                 //Wait for player to activate trigger
        yield return StartCoroutine(simonGame.PlayRound(level2));                //Play level 2
        yield return StartCoroutine(headsetDetection.EnableDetection(isVrVersion));         //start checking for headset
        yield return StartCoroutine(triggerActivation.Enable());                 //Wait for player to activate trigger
        yield return StartCoroutine(simonGame.PlayRound(level3));                //Play level 3
        yield return StartCoroutine(headsetDetection.EnableDetection(isVrVersion));         //start checking for headset
        yield return StartCoroutine(triggerActivation.Enable());                 //Wait for player to activate trigger
        yield return StartCoroutine(simonGame.PlayRound(level4));                //Play level 4
        yield return StartCoroutine(headsetDetection.EnableDetection(isVrVersion));         //start checking for headset
        yield return StartCoroutine(triggerActivation.Enable());                 //Wait for player to activate trigger
        yield return StartCoroutine(simonGame.PlayRound(level5));                //Play level 5
        yield return StartCoroutine(headsetDetection.EnableDetection(isVrVersion));         //start checking for headset
        yield return StartCoroutine(triggerActivation.Enable());                 //Wait for player to activate trigger
        yield return StartCoroutine(simonGame.PlayRound());                      //Choose Difficulty, play round
        yield return StartCoroutine(simonGame.PlayRound(_lastDifficultyChosen)); //Play round at last difficulty
        yield return StartCoroutine(simonGame.PlayRound(_lastDifficultyChosen)); //Play round at last difficulty
        yield return StartCoroutine(simonGame.PlayRound(_lastDifficultyChosen)); //Play round at last difficulty
        EndStudy();
    }

    private void SetDifficulty(Difficulty difficulty)
    {
        if (!difficulty) return;

        _lastDifficultyChosen = difficulty;
    }
}
