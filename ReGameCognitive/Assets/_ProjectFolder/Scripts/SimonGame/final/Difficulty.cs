using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Difficulty", order = 1)]
public class Difficulty : ScriptableObject
{
    public int level;
    public int numberOfButtons;
    public int baseSequence;
    public int maxSequence;
    public float sessionTimeLimit;
    public string colorString;
    public LevelColors levelColors;
    public Feedback handFeedback;
    public int[] sequenceSet0;
    public int[] sequenceSet1;
    public int[] sequenceSet2;
    public int[] sequenceSet3;
    public List<int[]> predeterminedSequences = new List<int[]>();

    private int _currentIndex = 0;
    
    private const int MAX_ROUNDS = 4;


    private void OnEnable()
    {
        _currentIndex = 0;
        
        predeterminedSequences.Add(sequenceSet0);
        predeterminedSequences.Add(sequenceSet1);
        predeterminedSequences.Add(sequenceSet2);
        predeterminedSequences.Add(sequenceSet3);
    }

    public int[] GetNextSequenceSet()
    {
        var currentSequenceSet = predeterminedSequences[_currentIndex];
        _currentIndex++;
        
        return currentSequenceSet;
    }
    
    [Button]
    private void SetPredeterminedSequences()
    {
        predeterminedSequences = new List<int[]>(MAX_ROUNDS);
        sequenceSet0 = new int[maxSequence];
        sequenceSet1 = new int[maxSequence];
        sequenceSet2 = new int[maxSequence];
        sequenceSet3 = new int[maxSequence];

        for (var i = 0; i < maxSequence; i++)
        {
            sequenceSet0[i] = Random.Range(0, numberOfButtons);
            sequenceSet1[i] = Random.Range(0, numberOfButtons);
            sequenceSet2[i] = Random.Range(0, numberOfButtons);
            sequenceSet3[i] = Random.Range(0, numberOfButtons);
        }
        
        predeterminedSequences.Add(sequenceSet0);
        predeterminedSequences.Add(sequenceSet1);
        predeterminedSequences.Add(sequenceSet2);
        predeterminedSequences.Add(sequenceSet3);
    }

    [Button]
    private void PrintSequences(int index)
    {
        if (index >= MAX_ROUNDS) return;
        if (index >= predeterminedSequences.Count) return;
        
        for (var j = 0; j < maxSequence; j++)
        {
            Debug.Log($"[{predeterminedSequences[index][j]}]");
        }
    }
}
