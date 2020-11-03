using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    public string userId;
    public string localStartTime = "";
    public string localEndTime = "";
    public string utcStartTime = "";
    public string utcEndTime = "";
    public string timeInStudy = "";
    public bool studyCompleted = false;
    public int totalSequencesAttempted;
    public int totalSequencesCorrect;
    public float sequenceSuccessPercentage;
    public int totalRoundsAttempted;


    public void SetSequenceSuccessPercentage()
    {
        if (totalSequencesAttempted == 0)
        {
            sequenceSuccessPercentage = 0;
            return;
        }
            
        sequenceSuccessPercentage = (float) Decimal.Divide(totalSequencesCorrect, totalSequencesAttempted);
    }
    
    /*public void SetSessionSuccessPercentage()
    {
        if (totalSessionsAttempted == 0) sessionSuccessPercentage = 0;
            
        sessionSuccessPercentage = (float) Decimal.Divide(totalSessionsCorrect, totalSessionsAttempted);
    }*/
    
    public User()
    {
        userId = GenerateRandomUserId();
        SetStartTime();
    }
    
    public void SetEndTime()
    {
        localEndTime = DateTime.Now.ToString();
        utcEndTime = DateTime.UtcNow.ToString();

        var start = DateTime.Parse(localStartTime);
        timeInStudy = DateTime.Now.Subtract(start).ToString();
    }

    private void SetStartTime()
    {
        localStartTime = DateTime.Now.ToString();
        utcStartTime = DateTime.UtcNow.ToString();
    }

    private static string GenerateRandomUserId()
    {
        return Guid.NewGuid().ToString("N");
    }
}
