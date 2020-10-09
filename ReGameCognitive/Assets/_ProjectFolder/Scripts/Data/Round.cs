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
    public string timeInSequence = "";
    public bool roundCompleted = false;
    public int currentRound;
    public int totalSequencesAttempted;
    public int totalSequencesCorrect;
    public string sequencesAttempted;
    public string buttonMissed;
    public float sequenceSuccessPercentage;


    public void SetSequenceSuccessPercentage()
    {
        if (totalSequencesAttempted == 0)
        {
            sequenceSuccessPercentage = 0;
            return;
        }
            
        sequenceSuccessPercentage = (float) Decimal.Divide(totalSequencesCorrect, totalSequencesAttempted);
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