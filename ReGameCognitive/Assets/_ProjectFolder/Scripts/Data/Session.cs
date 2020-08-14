﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Session
{
    public string userId;
    public string localStartTime = "";
    public string localEndTime = "";
    public string utcStartTime = "";
    public string utcEndTime = "";
    public string timeInSequence = "";
    public bool sessionCompleted = false;
    public int currentSequence;
    public int totalSequencesAttempted;
    public int totalSequencesCorrect;
    public string sequencesAttempted;
    public string sequencesMissed;
    public string handUsed;
    public float sequenceSuccessPercentage;


    public void SetSequenceSuccessPercentage()
    {
        if (totalSequencesAttempted == 0) sequenceSuccessPercentage = 0;
            
        sequenceSuccessPercentage = (float) Decimal.Divide(totalSequencesCorrect, totalSequencesAttempted);
    }
    
    public Session(User user)
    {
        if (user == null) return;
        
        //Data from user
        currentSequence = user.totalSequencesAttempted + 1;
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