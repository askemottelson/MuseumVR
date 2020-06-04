using System;
using System.Collections.Generic;

[System.Serializable]
public class DataLog
{
    public Answer[] answers;
    public bool test;
    public long timestamp;

    public DataLog(List<Answer> answers, bool isTest)
    {
        this.answers = answers.ToArray();
        this.test = isTest;
        this.timestamp = new System.DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
    }
}