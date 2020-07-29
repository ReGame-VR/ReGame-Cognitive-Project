using System;
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
    public string timeInStudy = "";
    public bool sessionCompleted = false;
    public int currentSequence;
    public int sequencesCompleted;
    public int correctSequences;
    public List<string> sequences = new List<string>();

    public float SuccessPercentage
    {
        get
        {
            if (sequencesCompleted == 0) return 0;
            
            return (float) Decimal.Divide(correctSequences, sequencesCompleted);
        }
    }
    
    public Session(User user)
    {
        if (user == null) return;
        
        //Data from user
        currentSequence = user.sequencesCompleted;
        userId = user.userId;
        
        SetStartTime();
        sequences = new List<string>();
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