using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour
{
    public string userId;
    public string localStartTime = "";
    public string localEndTime = "";
    public string utcStartTime = "";
    public string utcEndTime = "";
    public string timeInStudy = "";
    public bool studyCompleted = false;
    public int sequencesCompleted = 0;
    public List<string> continueKeysPressed = new List<string>();
    public List<string> questions = new List<string>();
    public List<string> responseKeysEntered = new List<string>();
    public List<string> responsesSelected = new List<string>();
    public List<bool> answerCorrectness = new List<bool>();
    public int numberOfQuestions;
    public int numberOfCorrectAnswers;
    public float correctAnswerPercentage;
    public string url;
    public List<string> speechTranscriptions = new List<string>();
}
