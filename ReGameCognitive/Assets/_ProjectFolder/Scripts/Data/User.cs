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
    
    public User()
    {
        userId = GenerateRandomUserId();
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

    private static string GenerateRandomUserId()
    {
        return Guid.NewGuid().ToString("N");
    }
}
