using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class FinalSimon : MonoBehaviour
{
    [SerializeField] private Feedback[] feedbacks;
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
    
    private const int NULL_BUTTON_INDEX = -1;
    

    // Start is called before the first frame update
    void OnEnable()
    {
        Initialize();
        StartCoroutine(PlaySequence());
    }

    // Update is called once per frame
    void Update()
    {
        Timer();
        roundText.text = (_numSequences + 1).ToString();
        
        CheckForButtonPushed();
    }

    private void Initialize()
    {
        _currentSequenceIndex = 0;
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
        if (WasCorrectButtonPushed())
        {
            if (_currentSequenceIndex == _numSequences)
            {
                _currentSequenceIndex = 0;
                _numSequences++;
                StartCoroutine(PlaySequence());
            }
            else
            {
                _currentSequenceIndex++;
            }

            _buttonPushedIndex = NULL_BUTTON_INDEX;
        }
        else if (WasIncorrectButtonPushed())
        {
            if (_numSequences == 0)
            {
                _buttonPushedIndex = NULL_BUTTON_INDEX;
            }
            else
            {
                _numSequences--;
                _currentSequenceIndex = 0;
                _buttonPushedIndex = NULL_BUTTON_INDEX;
            }
            
            StartCoroutine(PlaySequence());
        }
    }

    private bool WasIncorrectButtonPushed()
    {
        return _buttonPushedIndex > NULL_BUTTON_INDEX && 
               _sequence.Length > 0 && 
               _buttonPushedIndex != _sequence[_currentSequenceIndex];
    }

    private bool WasCorrectButtonPushed()
    {
        return _buttonPushedIndex > NULL_BUTTON_INDEX && 
               _sequence.Length > 0 && 
               _buttonPushedIndex == _sequence[_currentSequenceIndex];
    }

    private void HowManyCubes()
    {
        for (int i = 0; i < buttonGameObjects.Length; i++)
        {
            if (i < _numberOfButtons)
            {
                buttonGameObjects[i].SetActive(true);
            }
            else
            {
                buttonGameObjects[i].SetActive(false);
            }
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

    public void Timer()
    {
        _timeRemaining -= Time.deltaTime;
        _timeInSequence += Time.deltaTime;
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
    
    public void PlayFeedback(int index, bool playHaptics)
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
    
    public void PlayFeedback(int index, Feedback feedback, bool playHaptics)
    {
        if (buttonGameObjects.Length <= index) return;

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
    
    public void StopFeedback(int index)
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
    
    public void StopFeedback(int index, Feedback feedback)
    {
        if (buttonGameObjects.Length < index) return;

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

    IEnumerator PlaySequence()
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
}
