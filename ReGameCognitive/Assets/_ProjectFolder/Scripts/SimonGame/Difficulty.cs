using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Difficulty", order = 1)]
public class Difficulty : ScriptableObject
{
    public int level;
    public int numberOfButtons;
    public int baseSequence;
    public int maxSequence;
    public float sessionTimeLimit;
    public LevelColors levelColors;
}
