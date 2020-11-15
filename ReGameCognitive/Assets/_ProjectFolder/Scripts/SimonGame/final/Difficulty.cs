using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
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
    public string keyLetter;
    public LevelColors levelColors;
    public Feedback handFeedback;
    public int[] sequenceSet0;
    public int[] sequenceSet1;
    public int[] sequenceSet2;
    public int[] sequenceSet3;
    public int[] sequenceSet4;
    public List<int[]> predeterminedSequences = new List<int[]>();
    public Dictionary<int, string> sequenceColorReference = new Dictionary<int, string>
    {
        {0, "BLUE"}, {1, "GREEN"}, {2, "RED"},
        {3, "YELLOW"}, {4, "ORANGE"}, {5, "BLACK"},
        {6, "WHITE"}, {7, "PINK"}, {8, "SKY_BLUE"}
    };

    private int _currentIndex = 0;
    private const int MAX_ROUNDS = 5;


    private void OnEnable()
    {
        _currentIndex = 0;
        
        predeterminedSequences.Add(sequenceSet0);
        predeterminedSequences.Add(sequenceSet1);
        predeterminedSequences.Add(sequenceSet2);
        predeterminedSequences.Add(sequenceSet3);
        predeterminedSequences.Add(sequenceSet4);
        
        PrintSequences();
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
        sequenceSet4 = new int[maxSequence];

        for (var i = 0; i < maxSequence; i++)
        {
            sequenceSet0[i] = Random.Range(0, numberOfButtons);
            sequenceSet1[i] = Random.Range(0, numberOfButtons);
            sequenceSet2[i] = Random.Range(0, numberOfButtons);
            sequenceSet3[i] = Random.Range(0, numberOfButtons);
            sequenceSet4[i] = Random.Range(0, numberOfButtons);
        }
        
        predeterminedSequences.Add(sequenceSet0);
        predeterminedSequences.Add(sequenceSet1);
        predeterminedSequences.Add(sequenceSet2);
        predeterminedSequences.Add(sequenceSet3);
        predeterminedSequences.Add(sequenceSet4);
    }

    [Button]
    private void PrintSequenceButtons(int index)
    {
        if (index >= MAX_ROUNDS) return;
        if (index >= predeterminedSequences.Count) return;
        
        for (var j = 0; j < maxSequence; j++)
        {
            Debug.Log($"[{predeterminedSequences[index][j]}]");
        }
    }
    
    [Button]
    private void PrintSequences()
    {
        CheckIfFileExists();
        
        WriteString("-------------- Level " + level + " --------------\n");

        for (int x = 0; x < predeterminedSequences.Count; x++)
        {
            WriteString("-------------- Sequence " + x + " --------------\n");
            
            string currentSequence = "";
            for (int i = 0; i < baseSequence; i++)
            {
                currentSequence += $"[{sequenceColorReference[predeterminedSequences[x][i]]}] ";
            }
        
            for (var j = 0; j < maxSequence; j++)
            {
                currentSequence += $"[{sequenceColorReference[predeterminedSequences[x][j]]}] ";
                WriteString(currentSequence);
            }
            
            WriteString("\n");
        }
    }
    
    private void WriteString(string text)
    {
        string path = "Assets/Resources/" + "Level_" + level.ToString() + "_Sequences.txt";

        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(text);
        writer.Close();
    }

    private void CheckIfFileExists()
    {
        string path = "Assets/Resources/" + "Level_" + level.ToString() + "_Sequences.txt";
        
        if (File.Exists(path))
        {
            Debug.Log("File Exists....Deleting to create new file....");
            File.Delete(path);
            Debug.Log("Successfully deleted!");
        }
        else
        {
            Debug.Log($"File at {path} does not exist.");
        }
    }
}
