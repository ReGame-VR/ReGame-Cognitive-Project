using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SimonGame : MonoBehaviour
{
    [SerializeField] private Feedback[] feedbacks;
    [SerializeField] private Feedback wrongFeedback;
    [SerializeField] private StartController startController;
    [SerializeField] private StopController stopController;
    [SerializeField] private InstructionPanel instructionPanel;
    [SerializeField] private CustomTextCanvas timerCustomTextCanvas;
    [SerializeField] private CustomTextCanvas roundCustomTextCanvas;
    [SerializeField] private CustomTextCanvas scoreCustomTextCanvas;
    [SerializeField] private Feedback defaultHandFeedback;
    [SerializeField] private Transform panelParentTransform;
    [SerializeField] private GameObject difficultyColliderParent;
    [SerializeField] private GameObject buttonModelParent;
    [SerializeField] private GameObject buttonColliderParent;
    [SerializeField] private GameObject[] hands;
    [SerializeField] private GameObject[] buttonModelGameObjects;
    [SerializeField] private float timeBetweenCubeLit;
    [SerializeField] private float timeCubeLit;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private bool usePredeterminedSequences;
    

    private User _currentUser;
    private Session _currentSession;
    private Feedback _handFeedback;
    private Difficulty _currentDifficulty;
    private int[] _sequence;
    private float _timeInSequence;
    private int _numberOfButtons;
    private int _currentSequenceIndex = 0;
    private int _maxSequence;
    private int _buttonPushedIndex;
    private int _numSequences;
    private int _litCubeIndex;
    private float _timeRemaining;
    private float _timeLimit;
    private int _round;
    private bool _isActive;
    private bool _responseIsBeingProcessed;
    
    private const int NULL_BUTTON_INDEX = -1;
    private const float CHECK_INTERVAL = 1;

    public delegate void StateHandler();
    public delegate void DifficultyHandler(Difficulty difficulty);
    public event DifficultyHandler DifficultyWasSet;
    public event StateHandler SessionHasStarted;
    public event StateHandler SessionHasEnded;
    public event StateHandler RoundHasStarted;
    public event StateHandler RoundHasEnded;
    public event StateHandler ButtonWasPushed;
    
    

    void FixedUpdate()
    {
        if (!_isActive) return;
        
        Timer();
        CheckForButtonPushed();
    }

    public void SetUser(User user)
    {
        _currentUser = user;
    }

    public void SetSession(Session session)
    {
        _currentSession = session;
    }
    
    public IEnumerator PlayTutorial(Difficulty difficulty)
    {
        if (difficulty == null)
        {
            yield return StartCoroutine(PlayRound());
            yield break;
        }
        
        StartFromChooseDifficulty(difficulty);
        SetDifficulty(difficulty);

        //Wait for Game to start
        while (!_isActive)
        {
            yield return new WaitForSeconds(CHECK_INTERVAL);
        }
        
        //Wait for game to end
        while (_isActive)
        {
            yield return new WaitForSeconds(CHECK_INTERVAL);
        }
    }

    public IEnumerator PlayRound(Difficulty difficulty)
    {
        if (difficulty == null)
        {
            yield return StartCoroutine(PlayRound());
            yield break;
        }
        
        StartFromChooseDifficulty(difficulty);

        //Wait for Game to start
        while (!_isActive)
        {
            yield return new WaitForSeconds(CHECK_INTERVAL);
        }
        
        //Wait for game to end
        while (_isActive)
        {
            yield return new WaitForSeconds(CHECK_INTERVAL);
        }
    }
    
    public IEnumerator PlayRound()
    {
        StartFromChooseDifficulty();

        //Wait for Game to start
        while (!_isActive)
        {
            yield return new WaitForSeconds(CHECK_INTERVAL);
        }
        
        //Wait for game to end
        while (_isActive)
        {
            yield return new WaitForSeconds(CHECK_INTERVAL);
        }
    }
    
    [Button]
    public void StartFromStopSequence()
    {
        if (buttonModelParent) buttonModelParent.SetActive(true);
        if (buttonColliderParent) buttonColliderParent.SetActive(false);
        if (stopController) stopController.PlayStopSequence();

        ActivateHands();
    }

    public void StartFromStartSequence()
    {
        if (buttonModelParent) buttonModelParent.SetActive(true);
        if (buttonColliderParent) buttonColliderParent.SetActive(false);
        
        ActivateHands();
    }

    public void StartFromChooseDifficulty()
    {
        if (buttonModelParent) buttonModelParent.SetActive(true);
        if (buttonColliderParent) buttonColliderParent.SetActive(false);
        if (stopController) stopController.SetupDifficultyButtons();

        ActivateHands();
    }
    
    public void StartFromChooseDifficulty(Difficulty difficulty)
    {
        if (!difficulty) return;

        var level = difficulty.level;
        
        if (instructionPanel) instructionPanel.SetDifficulty(difficulty);
        if (buttonModelParent) buttonModelParent.SetActive(true);
        if (buttonColliderParent) buttonColliderParent.SetActive(false);
        if (stopController) stopController.SetupDifficultyButtons(level);

        ActivateHands();
    }

    [Button]
    public void StartGame()
    {
        _isActive = true;
        Initialize();
        StartCoroutine(PlaySequence());

        SessionHasStarted?.Invoke();
        RoundHasStarted?.Invoke();
    }

    [Button]
    public void StopGame()
    {
        _isActive = false;
        
        if (buttonColliderParent) buttonColliderParent.SetActive(false);
        if (stopController) stopController.PlayStopSequence();
        
        if (timerCustomTextCanvas) timerCustomTextCanvas.Disable();
        if (roundCustomTextCanvas) roundCustomTextCanvas.Disable();
        if (scoreCustomTextCanvas) scoreCustomTextCanvas.Enable();

        ResetHands();
        ActivateHands();
        UpdateSessionTime();
        EndSession();
        
        SessionHasEnded?.Invoke();
    }
    
    public void ButtonPress(ButtonData buttonData)
    {
        if (!buttonData || 
            _buttonPushedIndex != NULL_BUTTON_INDEX || 
            _responseIsBeingProcessed) return;
        
        _buttonPushedIndex = buttonData.cubeNumber;
        PlayFeedback(buttonData.cubeNumber, true);
    }

    public void ButtonRelease(ButtonData buttonData)
    {
        if (!buttonData) return;
        
        _buttonPushedIndex = NULL_BUTTON_INDEX;
        StopFeedback(buttonData.cubeNumber);
    }

    public void PlayFeedback(int index, Feedback feedback, bool playHaptics)
    {
        if (buttonModelGameObjects.Length <= index || !feedback) return;

        var button = buttonModelGameObjects[index];
        if (!button) return;

        var buttonRenderer = button.GetComponent<Renderer>();
        if (buttonRenderer)
        {
            var material = buttonRenderer.material;
            var audioSource = button.GetComponent<AudioSource>();
            feedback.Play(audioSource, material, playHaptics);
        }
    }
    
    public void StopFeedback(int index, Feedback feedback)
    {
        if (buttonModelGameObjects.Length <= index || !feedback) return;

        var button = buttonModelGameObjects[index];
        if (!button) return;
        
        var buttonRenderer = button.GetComponent<Renderer>();
        if (buttonRenderer)
        {
            var material = buttonRenderer.material;
            var audioSource = button.GetComponent<AudioSource>();
            feedback.Stop(audioSource, material);
        }
    }

    public void PlaySilentFeedback(int index)
    {
        if (buttonModelGameObjects.Length <= index) return;

        var button = buttonModelGameObjects[index];
        if (!button) return;

        var buttonRenderer = button.GetComponent<Renderer>();
        if (buttonRenderer)
        {
            var material = buttonRenderer.material;
            feedbacks[index].Play(null, material, false);
        }
    }
    
    public void SetDifficulty(Difficulty difficulty)
    {
        if (!difficulty) return;

        if (difficulty.handFeedback) _handFeedback = difficulty.handFeedback;
        if (difficulty.levelColors) difficulty.levelColors.SetLevelColor(panelParentTransform);

        _numberOfButtons = difficulty.numberOfButtons;
        _numSequences = difficulty.baseSequence;
        _timeLimit = difficulty.sessionTimeLimit;
        _maxSequence = difficulty.maxSequence;
        
        if (difficultyColliderParent) difficultyColliderParent.SetActive(false);
        if (startController) startController.PlayStartSequence();
        if (instructionPanel) instructionPanel.Disable();

        _currentDifficulty = difficulty;
        
        DifficultyWasSet?.Invoke(difficulty);
    }

    private void UpdateUserTime()
    {
        _currentUser?.SetEndTime();
    }
    
    private void EndSession()
    {
        if (_currentSession == null || _currentUser == null) return;
        
        _currentSession.SetEndTime();
        _currentSession.timeInSequence = CustomTextCanvas.FormatTimeToString(_timeInSequence);
        _currentSession.sessionCompleted = true;
    }

    private void UpdateSessionTime()
    {
        if (_currentSession == null) return;
        
        _currentSession.SetEndTime();
        _currentSession.timeInSequence = CustomTextCanvas.FormatTimeToString(_timeInSequence);
    }

    private void StoreButtonPushData()
    {
        if (_currentSession == null || _currentUser == null) return;

        var sequenceAttempted = "";
        for (var i = 0; i <= _numSequences; i++)
        {
            sequenceAttempted += "[" + _sequence[i] + "] ";
        }
        _currentSession.sequencesAttempted = sequenceAttempted;
        _currentSession.totalSequencesAttempted = _round;

        _currentUser.totalSequencesAttempted = _round;
    }

    private void StoreCorrectSequence()
    {
        if (_currentSession == null || _currentUser == null) return;
        
        _currentSession.sequencesMissed = "";
        _currentSession.totalSequencesCorrect++;
        _currentSession.SetSequenceSuccessPercentage();

        _currentUser.totalSequencesCorrect++;
        _currentUser.SetSequenceSuccessPercentage();

        if (scoreCustomTextCanvas) scoreCustomTextCanvas.SetBody(CustomTextCanvas.FormatDecimalToPercent(_currentSession.sequenceSuccessPercentage));
    }

    private void StoreIncorrectSequence()
    {
        if (_currentSession == null || _currentUser == null) return;

        var sequenceEntered = "";
        for (var i = 0; i < _currentSequenceIndex; i++)
        {
            sequenceEntered += "[" + _sequence[i] + "] ";
        }
        _currentSession.sequencesMissed = sequenceEntered + "[" + _buttonPushedIndex + "]";
        _currentSession.SetSequenceSuccessPercentage();
        
        _currentUser.SetSequenceSuccessPercentage();

        if (scoreCustomTextCanvas) scoreCustomTextCanvas.SetBody(CustomTextCanvas.FormatDecimalToPercent(_currentSession.sequenceSuccessPercentage));
    }

    private void Initialize()
    {
        _round = 1;
        if (roundCustomTextCanvas) roundCustomTextCanvas.SetBody(_round.ToString());
        
        _currentSequenceIndex = 0;
        _timeRemaining = _timeLimit;
        _buttonPushedIndex = NULL_BUTTON_INDEX;
        
        DeactivateHands();
        SetupColors();
        HowManyCubes();
        SetupSequence();
        if (timerCustomTextCanvas) timerCustomTextCanvas.Enable();
        if (roundCustomTextCanvas) roundCustomTextCanvas.Enable();
        if (scoreCustomTextCanvas) scoreCustomTextCanvas.Disable();
    }

    private void SetupColors()
    {
        for (int i = 0; i < buttonModelGameObjects.Length; i++)
        {
            StopFeedback(i, feedbacks[i]);
        }
    }

    private void CheckForButtonPushed()
    {
        if (!WasButtonPushed() || _responseIsBeingProcessed) return;
        
        _responseIsBeingProcessed = true;
        StoreButtonPushData();
        
        if (WasCorrectButtonPushed())
        {
            if (_currentSequenceIndex == _numSequences)    // Is last sequence
            {
                StoreCorrectSequence();
                
                //Add to sequence, play from beginning
                _numSequences++;
                StartCoroutine(StartNextRound(true));
            }
            else
            {
                //Check for next sequence
                _currentSequenceIndex++;
                _responseIsBeingProcessed = false;
            }
        }
        else
        {
            _numSequences = _numSequences > 0 ? _numSequences - 1 : 0;
            
            StoreIncorrectSequence();
            StartCoroutine(StartNextRound(false));
        }

        _buttonPushedIndex = NULL_BUTTON_INDEX;
    }

    private IEnumerator StartNextRound(bool wasCorrect)
    {
        yield return new WaitForSeconds(timeCubeLit);
        
        StopAllFeedback();
        DeactivateHands();
        
        if (!wasCorrect) yield return StartCoroutine(WrongResponse());

        yield return new WaitForSeconds(timeCubeLit);
        
        RoundHasEnded?.Invoke();
        
        _currentSequenceIndex = 0;
        _responseIsBeingProcessed = false;
        _round++;
        if (roundCustomTextCanvas) roundCustomTextCanvas.SetBody(_round.ToString());
        
        RoundHasStarted?.Invoke();
        
        StartCoroutine(PlaySequence());
    }

    private IEnumerator WrongResponse()
    {
        yield return new WaitForSeconds(timeCubeLit);
        
        for (int i = 0; i < _numberOfButtons; i++)
        {
            PlayFeedback(i, wrongFeedback, false);
        }
        
        yield return new WaitForSeconds(timeCubeLit);
        
        SetupColors();
    }

    private bool WasButtonPushed()
    {
        return _buttonPushedIndex > NULL_BUTTON_INDEX &&
               _sequence != null &&
               _sequence.Length > 0;
    }

    private bool WasCorrectButtonPushed()
    {
        return _buttonPushedIndex == _sequence[_currentSequenceIndex];
    }

    private void HowManyCubes()
    {
        for (int i = 0; i < buttonModelGameObjects.Length; i++)
        {
            var state = i < _numberOfButtons;
            buttonModelGameObjects[i].SetActive(state);
        }

        buttonColliderParent.SetActive(true);
        for (var i = 0; i < buttonColliderParent.transform.childCount; i++)
        {
            var state = i < _numberOfButtons;
            buttonColliderParent.transform.GetChild(i).gameObject.SetActive(state);
        }
    }

    private void SetupSequence()
    {
        if (usePredeterminedSequences && _currentDifficulty)
        {
            _sequence = _currentDifficulty.GetNextSequenceSet();
        }
        else
        {
            _sequence = new int[_maxSequence];
            while (_currentSequenceIndex < _maxSequence)
            {
                _sequence[_currentSequenceIndex] = Random.Range(0, _numberOfButtons);
                _currentSequenceIndex++;
            }
            _currentSequenceIndex = 0;
        }
    }

    private void ActivateHands()
    {
        if (!_handFeedback) return;
        
        foreach (var hand in hands)
        {
            var collider = hand.GetComponent<Collider>();
            if (collider) collider.enabled = true;

            var renderer = hand.GetComponent<Renderer>();
            if (renderer && renderer.material) renderer.material.color = _handFeedback.litColor;
        }
    }
    
    private void DeactivateHands()
    {
        if (!_handFeedback) return;
        
        foreach (var hand in hands)
        {
            var collider = hand.GetComponent<Collider>();
            if (collider) collider.enabled = false;

            var renderer = hand.GetComponent<Renderer>();
            if (renderer && renderer.material) renderer.material.color = _handFeedback.unlitColor;
        }
    }

    private void Timer()
    {
        _timeRemaining -= Time.fixedDeltaTime;
        _timeInSequence += Time.fixedDeltaTime;
        timerCustomTextCanvas.SetBody(CustomTextCanvas.FormatTimeToString(_timeRemaining));
        
        UpdateSessionTime();
        UpdateUserTime();

        if (_timeRemaining <= 0)    //Restart from "Choose difficulty"
        {
            StopGame();
        }
    }

    private void PlayFeedback(int index, bool playHaptics)
    {
        if (buttonModelGameObjects.Length <= index) return;

        var button = buttonModelGameObjects[index];
        if (!button) return;

        var buttonRenderer = button.GetComponent<Renderer>();
        if (buttonRenderer)
        {
            var material = buttonRenderer.material;
            var audioSource = button.GetComponent<AudioSource>();
            feedbacks[index].Play(audioSource, material, playHaptics);
        }
    }

    private void StopFeedback(int index)
    {
        if (buttonModelGameObjects.Length < index) return;

        var button = buttonModelGameObjects[index];
        if (!button) return;
        
        var buttonRenderer = button.GetComponent<Renderer>();
        if (buttonRenderer)
        {
            var material = buttonRenderer.material;
            var audioSource = button.GetComponent<AudioSource>();
            feedbacks[index].Stop(audioSource, material);
        }
    }

    private IEnumerator PlaySequence()
    {
        yield return new WaitForSeconds(timeBetweenCubeLit);
        DeactivateHands();
        while (_currentSequenceIndex <= _numSequences &&
               _isActive)
        {
            if (_sequence[_currentSequenceIndex] == _litCubeIndex)
            {
                var currentIndex = _litCubeIndex;
                yield return new WaitForSeconds(timeCubeLit);
                PlayFeedback(currentIndex, feedbacks[currentIndex], false);
                yield return new WaitForSeconds(timeCubeLit);
                StopFeedback(currentIndex, feedbacks[currentIndex]);
                _currentSequenceIndex++;
                _litCubeIndex = 0;
            }
            else
            {
                _litCubeIndex++;
            }
        }

        _currentSequenceIndex = 0;
        _timeInSequence = 0;
        ActivateHands();
        
        if(_isActive)
            audioManager.auditoryStart.Play();
    }

    private void StopAllFeedback()
    {
        for (int i = 0; i < _numberOfButtons; i++)
        {
            StopFeedback(i);
        }
    }

    //TODO implement
    private bool WasSessionPassed()
    {
        return true;
    }

    private void ResetHands()
    {
        if (!defaultHandFeedback) return;
        
        _handFeedback = defaultHandFeedback;
    }
}
