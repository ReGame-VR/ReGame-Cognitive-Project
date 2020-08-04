using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalSimon : MonoBehaviour
{
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
    public float CurrentTime;
    public float StartingTime;
    public TextMesh CountdownNumberText;
    public TextMesh RoundText;
    public GameObject SimonGame;
    public GameObject buttonColliderParent;
    public GameObject StopLights;

    // Start is called before the first frame update
    void OnEnable()
    {
        currentSequenceIndex = 0;
        CurrentTime = StartingTime;
        DisableHands();
        HowManyCubes();
        SetupSequence();
        StartCoroutine(PlaySequence());
    }

    // Update is called once per frame
    void Update()
    {
        Timer();
        RoundText.text = (numSequences+1).ToString();
        
        CheckForButtonPushed();
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
        while (currentSequenceIndex < NumberOfButtons)
        {
            Button[currentSequenceIndex].SetActive(true);
            currentSequenceIndex++;
            Sequence = new int[MaxSequence];
        }
        currentSequenceIndex = 0;
    }

    private void SetupSequence()
    {
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
        CurrentTime -= 1 * Time.deltaTime;
        string mintues = ((int)CurrentTime / 60).ToString();
        string seconds = (CurrentTime % 60).ToString("00");
        CountdownNumberText.text = mintues + ":" + seconds;

        if (CurrentTime <= 0)    //Restart from "Choose difficulty"
        {
            buttonColliderParent.SetActive(false);
            EnableHands();
            StopLights.SetActive(true);
            SimonGame.SetActive(false);
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
                yield return new WaitForSeconds(TimeCubeLit);
                Button[litCubeIndex - 1].GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
                yield return new WaitForSeconds(TimeCubeLit);
                Button[litCubeIndex - 1].GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
                currentSequenceIndex++;
                litCubeIndex = 1;
            }
            else
            {
                litCubeIndex++;
            }
        }

        currentSequenceIndex = 0;
        EnableHands();
    }
}
