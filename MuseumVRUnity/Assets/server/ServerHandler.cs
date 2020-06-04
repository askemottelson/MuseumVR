using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ServerHandler : MonoBehaviour
{
    public TextAsset jsonFile;
    public Canvas canvas;


    // make sure this one is false for deployment
    private bool TESTING = Application.isEditor;

    private Questionnaires qs;
    private string server = "https://museum-vr.ue.r.appspot.com/entry";

    private Log log = new Log();

    void Start()
    {
        this.qs = JsonUtility.FromJson<Questionnaires>(jsonFile.text);
    }

    int current_questionnaire_count = 0;
    int current_question_count = 0;

    private Questionnaire GetCurrentQuestionnaire()
    {
        return this.qs.questionnaires[current_questionnaire_count];
    }

    private Question GetCurrentQuestion()
    {
        return GetCurrentQuestionnaire().questions[current_question_count];
    }

    public void UpdateCanvas()
    {
        Questionnaire questionnaire;
        Question question;

        Debug.Log("UpdateCanvas()");

        try { 
            questionnaire = GetCurrentQuestionnaire();
            question = GetCurrentQuestion();
        }
        catch (Exception e) {
            // no more questions
            Debug.Log("No more questions");
            Debug.Log(e.Message);
            return;
        }
        
        SetTitle(question.question);
        SetLabelLeft(question.low + " ...");
        SetLabelRight("... " + question.high);
    }

    public void SetLabelLeft(string label)
    {
        Text label0 = canvas.transform.Find("Panel/LeftLabel").GetComponent<Text>();
        label0.text = label;
    }

    public void SetLabelRight(string label)
    {
        Text label1 = canvas.transform.Find("Panel/RightLabel").GetComponent<Text>();
        label1.text = label;
    }

    public void SetTitle(string title)
    {
        Text title0 = canvas.transform.Find("Panel/Title").GetComponent<Text>();
        title0.text = title;
    }

    void LogQuestions()
    {
        foreach (Questionnaire q in qs.questionnaires)
        {
            foreach (Question question in q.questions)
            {
                Debug.Log("Question: " + question.question + " " + question.buttons);
            }
        }
    }

    public bool AnswerQuestion(int button_id)
    {
        log.NewAnswer(button_id);

        if (current_question_count == GetCurrentQuestionnaire().questions.Length -1)
        {
            current_questionnaire_count++;
            current_question_count = 0;

            if (current_questionnaire_count == this.qs.questionnaires.Length -1)
            {
                current_questionnaire_count = 0;
                return false;
            }
        }
        else
        {
            current_question_count++;
        }

        return true;
    }

    public void HideButtons()
    {
        GameObject b1 = canvas.transform.Find("Panel/Button 1").gameObject;
        GameObject b2 = canvas.transform.Find("Panel/Button 2").gameObject;
        GameObject b3 = canvas.transform.Find("Panel/Button 3").gameObject;
        GameObject b4 = canvas.transform.Find("Panel/Button 4").gameObject;
        GameObject b5 = canvas.transform.Find("Panel/Button 5").gameObject;

        b1.SetActive(false);
        b2.SetActive(false);
        b3.SetActive(false);
        b4.SetActive(false);
        b5.SetActive(false);
    }

    public string toJSON()
    {
        return JsonUtility.ToJson(log.ToDataLog(TESTING));
    }

    public void sendToServer()
    {
        StartCoroutine(SendJSONToServer());
        Debug.Log("Done.");
    }

    IEnumerator SendJSONToServer()
    {
        string data = toJSON();

        Debug.Log(data);

        UnityWebRequest www = new UnityWebRequest(server);
        www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(data));
        www.downloadHandler = new DownloadHandlerBuffer();
        www.method = UnityWebRequest.kHttpVerbPOST;

        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Print Body
            Debug.Log(www.downloadHandler.text);
        }
    }
}