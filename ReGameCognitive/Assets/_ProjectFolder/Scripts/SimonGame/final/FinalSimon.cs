using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalSimon : MonoBehaviour
{
    public GameObject[] Button;
    public int[] Sequence;
    public int NumberOfButtons;
    public int i = 0;
    public int MaxSequence;
    public int ButtonPushed;
    public int CurrentSequence;
    public float TimeBetweenCubeLit;
    public float TimeCubeLit;
    public int a;
    public GameObject[] Hands;
    public Material[] HandColors;
    public float CurrentTime;
    public float StartingTime;
    public TextMesh CountdownNumberText;
    public TextMesh RoundText;
    public GameObject SimonGame;
    public GameObject ButtonSwitch;
    public GameObject StopLights;

    // Start is called before the first frame update
    void OnEnable()
    {
        i = 0;
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
        RoundText.text = (CurrentSequence+1).ToString();

        if (ButtonPushed == Sequence[i])
        {
            if (i == CurrentSequence)
            {
                i = 0;
                CurrentSequence++;
                StartCoroutine(PlaySequence());
            }

            else
            {
                i++;
            }

            ButtonPushed = 0;
        }

        else if (ButtonPushed > 0 && ButtonPushed != Sequence[i])
        {
            if (CurrentSequence == 0)
            {
                ButtonPushed = 0;
                StartCoroutine(PlaySequence());
            }

            else
            {
                CurrentSequence--;
                i = 0;
                ButtonPushed = 0;
                StartCoroutine(PlaySequence());
            }
        }
    }

    private void HowManyCubes()
    {
        while (i < NumberOfButtons)
        {
            Button[i].SetActive(true);
            i++;
            Sequence = new int[MaxSequence];
        }
        i = 0;
    }

    private void SetupSequence()
    {
        while (i < MaxSequence)
        {
            Sequence[i] = Random.Range(1, NumberOfButtons+1);
            i++;
        }
        i = 0;
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

        if (CurrentTime <= 0)
        {
            ButtonSwitch.SetActive(false);
            EnableHands();
            StopLights.SetActive(true);
            SimonGame.SetActive(false);
        }

    }

    IEnumerator PlaySequence()
    {
        yield return new WaitForSeconds(TimeBetweenCubeLit);
        DisableHands();
        while (i <= CurrentSequence)
        {
            if (Sequence[i] == a)
            {
                yield return new WaitForSeconds(TimeCubeLit);
                Button[a - 1].GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
                yield return new WaitForSeconds(TimeCubeLit);
                Button[a - 1].GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
                i++;
                a = 1;
            }

            else
            {
                a++;
            }
        }

        i = 0;
        EnableHands();
    }
}
