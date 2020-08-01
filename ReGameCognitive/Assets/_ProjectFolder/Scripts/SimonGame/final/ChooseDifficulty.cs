using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseDifficulty : MonoBehaviour
{
    public int DifficultyNumber;
    public int StartAtSequence;
    public FinalSimon FinalSimon;
    public GameObject DifficultyButtons;
    public GameObject StartLight;
    public float SetTime;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter()
    {
        FinalSimon.NumberOfButtons = DifficultyNumber;
        FinalSimon.CurrentSequence = StartAtSequence;
        FinalSimon.StartingTime = SetTime;
        DifficultyButtons.SetActive(false);
        StartLight.SetActive(true);
    }
}
