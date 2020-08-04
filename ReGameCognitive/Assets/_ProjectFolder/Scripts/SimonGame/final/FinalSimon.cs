using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalSimon : MonoBehaviour
{
    [SerializeField] private Feedback[] feedbacks;
    
    public GameObject[] Button;
    public int[] Sequence;
    public int NumberOfButtons;
    public int currentSequenceIndex = 0;
    public int MaxSequence;
    public int buttonPushedIndex;
    public int numSequences;
    public float TimeBetweenCubeLit;
    public float TimeCubeLit;
    public int litCubeIndex;
    public GameObject[] Hands;
    public Material[] HandColors;
    public float timeRemaining;
    public float timeLimit;
    public TextMesh CountdownNumberText;
    public TextMesh RoundText;
    public GameObject SimonGame;
    public GameObject buttonColliderParent;
    public GameObject StopLights;

    private float _timeInSequence;

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
        RoundText.text = (numSequences+1).ToString();
        
        CheckForButtonPushed();
    }

    private void Initialize()
    {
        currentSequenceIndex = 0;
        timeRemaining = timeLimit;
        DisableHands();
        SetupColors();
        HowManyCubes();
        SetupSequence();
    }

    private void SetupColors()
    {
        for (int i = 0; i < Button.Length; i++)
        {
            StopFeedback(i, feedbacks[i]);
        }
    }

    private void CheckForButtonPushed()
    {
        if (WasCorrectButtonPushed())
        {
            if (currentSequenceIndex == numSequences)
            {
                currentSequenceIndex = 0;
                numSequences++;
                StartCoroutine(PlaySequence());
            }
            else
            {
                currentSequenceIndex++;
            }

            buttonPushedIndex = 0;
        }
        else if (WasIncorrectButtonPushed())
        {
            if (numSequences == 0)
            {
                buttonPushedIndex = 0;
            }
            else
            {
                numSequences--;
                currentSequenceIndex = 0;
                buttonPushedIndex = 0;
            }
            
            StartCoroutine(PlaySequence());
        }
    }

    private bool WasIncorrectButtonPushed()
    {
        return buttonPushedIndex > 0 && buttonPushedIndex != Sequence[currentSequenceIndex];
    }

    private bool WasCorrectButtonPushed()
    {
        return buttonPushedIndex == Sequence[currentSequenceIndex];
    }

    private void HowManyCubes()
    {
        for (int i = 0; i < Button.Length; i++)
        {
            if (i < NumberOfButtons)
            {
                Button[i].SetActive(true);
            }
            else
            {
                Button[i].SetActive(false);
            }
        }
    }

    private void SetupSequence()
    {
        Sequence = new int[MaxSequence];
        while (currentSequenceIndex < MaxSequence)
        {
            Sequence[currentSequenceIndex] = Random.Range(1, NumberOfButtons+1);    //Sequence is base 1
            currentSequenceIndex++;
        }
        currentSequenceIndex = 0;
    }

    private void DisableHands()
    {
        Hands[0].GetComponent<BoxCollider>().isTrigger = false;
        Hands[1].GetComponent<BoxCollider>().isTrigger = false;
        Hands[0].GetComponent<Renderer>().material = HandColors[1];
        Hands[1].GetComponent<Renderer>().material = HandColors[1];
    }

    private void EnableHands()
    {
        Hands[0].GetComponent<BoxCollider>().isTrigger = true;
        Hands[1].GetComponent<BoxCollider>().isTrigger = true;
        Hands[0].GetComponent<Renderer>().material = HandColors[0]; ;
        Hands[1].GetComponent<Renderer>().material = HandColors[0]; ;
    }

    public void Timer()
    {
        timeRemaining -= Time.deltaTime;
        _timeInSequence += Time.deltaTime;
        string mintues = ((int)timeRemaining / 60).ToString();
        string seconds = (timeRemaining % 60).ToString("00");
        CountdownNumberText.text = mintues + ":" + seconds;

        if (timeRemaining <= 0)    //Restart from "Choose difficulty"
        {
            buttonColliderParent.SetActive(false);
            EnableHands();
            StopLights.SetActive(true);
            SimonGame.SetActive(false);
        }
    }

    public void PlaySilentFeedback(int index)
    {
        if (Button.Length <= index) return;

        var button = Button[index];
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
        if (Button.Length <= index) return;

        var button = Button[index];
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
        if (Button.Length <= index) return;

        var button = Button[index];
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
        if (Button.Length < index) return;

        var button = Button[index];
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
        if (Button.Length < index) return;

        var button = Button[index];
        if (!button) return;
        
        var buttonRenderer = button.GetComponent<Renderer>();
        if (buttonRenderer)
        {
            var material = buttonRenderer.material;
            var audioSource = button.GetComponent<AudioSource>();
            feedback.Stop(audioSource, material);
        }
    }

    IEnumerator PlaySequence()
    {
        yield return new WaitForSeconds(TimeBetweenCubeLit);
        DisableHands();
        while (currentSequenceIndex <= numSequences)
        {
            if (Sequence[currentSequenceIndex] == litCubeIndex)
            {
                var currentIndex = litCubeIndex - 1;
                yield return new WaitForSeconds(TimeCubeLit);
                PlayFeedback(currentIndex, feedbacks[currentIndex], false);
                yield return new WaitForSeconds(TimeCubeLit);
                StopFeedback(currentIndex, feedbacks[currentIndex]);
                currentSequenceIndex++;
                litCubeIndex = 1;
            }
            else
            {
                litCubeIndex++;
            }
        }

        currentSequenceIndex = 0;
        _timeInSequence = 0;
        EnableHands();
    }
}
