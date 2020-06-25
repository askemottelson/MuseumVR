using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

[System.Serializable]
public class DataLog
{
    public Answer[] answers;
    public bool test;
    public long timestamp;
    public string device;
    public string IP;

    public DataLog(List<Answer> answers, bool isTest)
    {
        this.device = SystemInfo.deviceModel;
        this.answers = answers.ToArray();
        this.test = isTest;
        this.timestamp = new System.DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
        this.IP = new WebClient().DownloadString("http://icanhazip.com");

        Debug.Log(this);
    }
}