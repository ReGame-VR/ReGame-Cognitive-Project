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
    [SerializeField] private Transform wallParentTransform;
    [SerializeField] private GameObject difficultyColliderParent;
    [SerializeField] private GameObject buttonModelParent;
    [SerializeField] private GameObject buttonColliderParent;
    [SerializeField] private GameObject[] canvasGameObjects;
    [SerializeField] private GameObject[] hands;
    [SerializeField] private GameObject[] buttonModelGameObjects;
    [SerializeField] private TextMesh countdownNumberText;
    [SerializeField] private TextMesh roundText;
    [SerializeField] private Material activatedHandMaterial;
    [SerializeField] private Material deactivatedHandMaterial;
    [SerializeField] private float timeBetweenCubeLit;
    [SerializeField] private float timeCubeLit;
    
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

    public delegate void StateHandler();
    
    public event StateHandler SessionHasStarted;
    public event StateHandler SessionHasEnded;
    

    void FixedUpdate()
    {
        if (!_isActive) return;
        
        Timer();
        CheckForButtonPushed();
    }

    [Button]
    public void Activate()
    {
        if (buttonModelParent) buttonModelParent.SetActive(true);
        if (buttonColliderParent) buttonColliderParent.SetActive(false);
        if (stopController) stopController.PlayStopSequence();

        ActivateHands();
    }

    [Button]
    public void StartGame()
    {
        _isActive = true;
        Initialize();
        StartCoroutine(PlaySequence());
        
        SessionHasStarted?.Invoke();
    }

    [Button]
    public void StopGame()
    {
        _isActive = false;
        
        if (buttonColliderParent) buttonColliderParent.SetActive(false);
        if (stopController) stopController.PlayStopSequence();
        
        ToggleCanvasObjects(false);
        ActivateHands();
        
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

        difficulty.levelColors.SetLevelColor(wallParentTransform);

        _numberOfButtons = difficulty.numberOfButtons;
        _numSequences = difficulty.baseSequence;
        _timeLimit = difficulty.sessionTimeLimit;
        _maxSequence = difficulty.maxSequence;
        
        if (difficultyColliderParent) difficultyColliderParent.SetActive(false);
        if (startController) startController.PlayStartSequence();
    }

    private void Initialize()
    {
        _round = 1;
        if (roundText) roundText.text = (_round).ToString();
        
        _currentSequenceIndex = 0;
        _timeRemaining = _timeLimit;
        _buttonPushedIndex = NULL_BUTTON_INDEX;
        
        DeactivateHands();
        SetupColors();
        HowManyCubes();
        SetupSequence();
        
        ToggleCanvasObjects(true);
    }

    private void ToggleCanvasObjects(bool state)
    {
        foreach (var canvasGameObject in canvasGameObjects)
        {
            canvasGameObject.SetActive(state);
        }
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
        
        if (WasCorrectButtonPushed())
        {
            if (_currentSequenceIndex == _numSequences)    // Is last sequence
            {
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
            
            StartCoroutine(StartNextRound(false));
        }

        _buttonPushedIndex = NULL_BUTTON_INDEX;
    }

    private IEnumerator StartNextRound(bool wasCorrect)
    {
        _currentSequenceIndex = 0;
        _round++;
        if (roundText) roundText.text = (_round).ToString();
        
        yield return new WaitForSeconds(timeCubeLit);
        
        StopAllFeedback();
        DeactivateHands();
        
        if (!wasCorrect) yield return StartCoroutine(WrongResponse());

        yield return new WaitForSeconds(timeCubeLit);
        
        _responseIsBeingProcessed = false;
        
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
        _sequence = new int[_maxSequence];
        while (_currentSequenceIndex < _maxSequence)
        {
            _sequence[_currentSequenceIndex] = Random.Range(0, _numberOfButtons);
            _currentSequenceIndex++;
        }
        _currentSequenceIndex = 0;
    }

    private void ActivateHands()
    {
        if (!activatedHandMaterial) return;
        
        foreach (var hand in hands)
        {
            var collider = hand.GetComponent<Collider>();
            if (collider) collider.enabled = true;

            var renderer = hand.GetComponent<Renderer>();
            if (renderer) renderer.material = activatedHandMaterial;
        }
    }
    
    private void DeactivateHands()
    {
        if (!deactivatedHandMaterial) return;
        
        foreach (var hand in hands)
        {
            var collider = hand.GetComponent<Collider>();
            if (collider) collider.enabled = false;

            var renderer = hand.GetComponent<Renderer>();
            if (renderer) renderer.material = deactivatedHandMaterial;
        }
    }

    private void Timer()
    {
        _timeRemaining -= Time.fixedDeltaTime;
        _timeInSequence += Time.fixedDeltaTime;
        var minutes = ((int)_timeRemaining / 60).ToString();
        var seconds = (_timeRemaining % 60).ToString("00");
        countdownNumberText.text = minutes + ":" + seconds;

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
    }

    private void StopAllFeedback()
    {
        for (int i = 0; i < _numberOfButtons; i++)
        {
            StopFeedback(i);
        }
    }
}
