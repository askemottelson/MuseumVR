using System;

[System.Serializable]
public class Answer
{
    public int button_value = 0;
    public long timestamp = 0;

    public Answer(int button_value)
    {
        this.button_value = button_value;
        this.timestamp = new System.DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
    }

}

