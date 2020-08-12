using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class FinalSimon : MonoBehaviour
{
    [SerializeField] private Feedback[] feedbacks;
    [SerializeField] private Feedback wrongFeedback;
    [SerializeField] private GameObject startLightGameObject;
    [SerializeField] private GameObject stopLights;
    [SerializeField] private GameObject difficultyButtonParent;
    [SerializeField] private GameObject buttonColliderParent;
    [SerializeField] private GameObject[] hands;
    [SerializeField] private GameObject[] buttonGameObjects;
    [SerializeField] private Transform wallParentTransform;
    [SerializeField] private TextMesh countdownNumberText;
    [SerializeField] private TextMesh roundText;
    [SerializeField] private Material[] handColors;
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
    
    private const int NULL_BUTTON_INDEX = -1;
    

    // Start is called before the first frame update
    void OnEnable()
    {
        Initialize();
        StartCoroutine(PlaySequence());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Timer();
        roundText.text = (_round).ToString();
        
        CheckForButtonPushed();
    }
    
    public void ButtonPress(ButtonData buttonData)
    {
        _buttonPushedIndex = buttonData.cubeNumber;
        PlayFeedback(buttonData.cubeNumber, true);
    }

    public void ButtonRelease(ButtonData buttonData)
    {
        _buttonPushedIndex = NULL_BUTTON_INDEX;
        StopFeedback(buttonData.cubeNumber);
    }

    public void PlayFeedback(int index, Feedback feedback, bool playHaptics)
    {
        if (buttonGameObjects.Length <= index || !feedback) return;

        var button = buttonGameObjects[index];
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
        if (buttonGameObjects.Length <= index || !feedback) return;

        var button = buttonGameObjects[index];
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
        if (buttonGameObjects.Length <= index) return;

        var button = buttonGameObjects[index];
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
        
        if (difficultyButtonParent) difficultyButtonParent.SetActive(false);
        if (startLightGameObject) startLightGameObject.SetActive(true);
    }

    private void Initialize()
    {
        _currentSequenceIndex = 0;
        _round = 1;
        _timeRemaining = _timeLimit;
        _buttonPushedIndex = NULL_BUTTON_INDEX;
        DisableHands();
        SetupColors();
        HowManyCubes();
        SetupSequence();
    }

    private void SetupColors()
    {
        for (int i = 0; i < buttonGameObjects.Length; i++)
        {
            StopFeedback(i, feedbacks[i]);
        }
    }

    private void CheckForButtonPushed()
    {
        if (!WasButtonPushed()) return;

        if (WasCorrectButtonPushed())
        {
            //Debug.Log("WasCorrectButtonPushed()");
            if (_currentSequenceIndex == _numSequences)    // Is last sequence
            {
                //Add to sequence, play from beginning
                _numSequences++;

                _currentSequenceIndex = 0;
                _round++;
                StartCoroutine(StartNextRound(true));
            }
            else
            {
                //Check for next sequence
                _currentSequenceIndex++;
            }
        }
        else
        {
            _numSequences = _numSequences > 0 ? _numSequences - 1 : 0;
            
            _currentSequenceIndex = 0;
            _round++;
            StartCoroutine(StartNextRound(false));
        }

        _buttonPushedIndex = NULL_BUTTON_INDEX;
    }

    private IEnumerator StartNextRound(bool wasCorrect)
    {
        yield return new WaitForSeconds(timeCubeLit);
        
        DisableHands();
        
        if (!wasCorrect) yield return StartCoroutine(WrongResponse());
        
        yield return new WaitForSeconds(timeCubeLit);
        
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
        
        for (int i = 0; i < _numberOfButtons; i++)
        {
            StopFeedback(i, wrongFeedback);
        }
        
        yield return new WaitForSeconds(timeCubeLit);
        
        SetupColors();
    }

    private bool WasButtonPushed()
    {
        return _buttonPushedIndex > NULL_BUTTON_INDEX && 
               _sequence.Length > 0;
    }

    private bool WasCorrectButtonPushed()
    {
        return _buttonPushedIndex == _sequence[_currentSequenceIndex];
    }

    private void HowManyCubes()
    {
        for (int i = 0; i < buttonGameObjects.Length; i++)
        {
            var state = i < _numberOfButtons;
            buttonGameObjects[i].SetActive(state);
        }

        for (var i = 0; i < buttonColliderParent.transform.childCount; i++)
        {
            var state = i < _numberOfButtons;
            buttonColliderParent.transform.GetChild(i).gameObject.SetActive(state);
        }
    }

    [Button]
    private void SetupSequence()
    {
        _sequence = new int[_maxSequence];
        while (_currentSequenceIndex < _maxSequence)
        {
            _sequence[_currentSequenceIndex] = Random.Range(0, _numberOfButtons);
            Debug.Log(_sequence[_currentSequenceIndex]);
            _currentSequenceIndex++;
        }
        _currentSequenceIndex = 0;
    }

    private void DisableHands()
    {
        hands[0].GetComponent<BoxCollider>().isTrigger = false;
        hands[1].GetComponent<BoxCollider>().isTrigger = false;
        hands[0].GetComponent<Renderer>().material = handColors[1];
        hands[1].GetComponent<Renderer>().material = handColors[1];
    }

    private void EnableHands()
    {
        hands[0].GetComponent<BoxCollider>().isTrigger = true;
        hands[1].GetComponent<BoxCollider>().isTrigger = true;
        hands[0].GetComponent<Renderer>().material = handColors[0]; ;
        hands[1].GetComponent<Renderer>().material = handColors[0]; ;
    }

    private void Timer()
    {
        _timeRemaining -= Time.fixedDeltaTime;
        _timeInSequence += Time.fixedDeltaTime;
        string minutes = ((int)_timeRemaining / 60).ToString();
        string seconds = (_timeRemaining % 60).ToString("00");
        countdownNumberText.text = minutes + ":" + seconds;

        if (_timeRemaining <= 0)    //Restart from "Choose difficulty"
        {
            buttonColliderParent.SetActive(false);
            EnableHands();
            stopLights.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    private void PlayFeedback(int index, bool playHaptics)
    {
        if (buttonGameObjects.Length <= index) return;

        var button = buttonGameObjects[index];
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
        if (buttonGameObjects.Length < index) return;

        var button = buttonGameObjects[index];
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
        DisableHands();
        while (_currentSequenceIndex <= _numSequences)
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
        EnableHands();
    }

    private void StopAllFeedback()
    {
        for (int i = 0; i < _numberOfButtons; i++)
        {
            StopFeedback(i);
        }
    }
}
