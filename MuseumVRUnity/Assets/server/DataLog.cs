﻿using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

[System.Serializable]
public class DataLog
{
    public Answer[] answers;
    public bool test;
    public long end;
    public string device;
    public string IP;
    public int condition;
    public long start;

    public DataLog(List<Answer> answers, bool isTest, long start, int condition)
    {
        this.device = SystemInfo.deviceModel;
        this.answers = answers.ToArray();
        this.test = isTest;
        this.end = new System.DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
        this.IP = new WebClient().DownloadString("http://icanhazip.com").Trim();
        this.start = start;
        this.condition = condition;

        Debug.Log(this);
    }
}