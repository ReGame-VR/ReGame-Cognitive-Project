using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round
{
    public string userId;
    public string localStartTime = "";
    public string localEndTime = "";
    public string utcStartTime = "";
    public string utcEndTime = "";
    public int currentRound;
    public string difficultyLevel = "";
    public string sequenceAttempted;
    public string buttonMissed;
    public string timeInSequence = "";
    public int totalSequencesAttemptedInRound;
    public int totalSequencesCorrectInRound;
    public float sequenceSuccessPercentageInRound;
    public bool roundCompleted = false;


    public void SetSequenceSuccessPercentage()
    {
        if (totalSequencesAttemptedInRound == 0)
        {
            sequenceSuccessPercentageInRound = 0;
            return;
        }
            
        sequenceSuccessPercentageInRound = (float) Decimal.Divide(totalSequencesCorrectInRound, totalSequencesAttemptedInRound);
    }
    
    public Round(User user)
    {
        if (user == null) return;
        
        //Data from user
        currentRound = user.totalRoundsAttempted + 1;
        userId = user.userId;
        
        SetStartTime();
    }
    
    public void SetEndTime()
    {
        localEndTime = DateTime.Now.ToString();
        utcEndTime = DateTime.UtcNow.ToString();
    }
    
    private void SetStartTime()
    {
        localStartTime = DateTime.Now.ToString();
        utcStartTime = DateTime.UtcNow.ToString();
    }
}