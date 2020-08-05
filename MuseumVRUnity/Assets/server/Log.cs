using System;
using System.Collections.Generic;



public class Log
{
    private List<Answer> answers = new List<Answer>();
    private long start;
    public int condition;

    public Log()
    {
        Random rnd = new Random();
        
        this.start = new System.DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
        this.condition = rnd.Next(0, 5);
    }

    public void NewAnswer(int answer, string name)
    {
        this.answers.Add(
            new Answer(answer, name)
        );
    }

    public DataLog ToDataLog(bool isTest)
    {
        return new DataLog(answers, isTest, start, condition);
    }

    public int NumAnswers()
    {
        return answers.Count;
    }
}

