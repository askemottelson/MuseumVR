using System.Collections.Generic;

public class Log
{
    private List<Answer> answers = new List<Answer>();

    public void NewAnswer(int answer)
    {
        this.answers.Add(
            new Answer(answer)
        );
    }

    public DataLog ToDataLog(bool isTest)
    {
        return new DataLog(answers, isTest);
    }

    public int NumAnswers()
    {
        return answers.Count;
    }
}

