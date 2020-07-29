using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NoMoreQuestions : Exception
{
    //
}

public class ServerHandler : MonoBehaviour
{
    public TextAsset jsonFile;

    // references to UI
    public GameObject title;
    public GameObject button1;
    public GameObject button2;
    public GameObject button3;
    public GameObject button4;
    public GameObject button5;

    public ServerResponse sr;
    

    // make sure this one is false for deployment
    private bool TESTING = Application.isEditor;

    private Questionnaires qs;
    private string server = "https://museum-vr.ue.r.appspot.com/entry";

    // stores data
    private Log log = new Log();

    // internal counters
    int current_questionnaire_count = 0;
    int current_question_count = 0;

    public int GetCondition()
    {
        return this.log.condition;
    } 

    public void Reset()
    {
        this.current_questionnaire_count = 0;
        this.current_question_count = 0;
    }

    void Start()
    {
        this.qs = JsonUtility.FromJson<Questionnaires>(jsonFile.text);
        LogQuestions();
    }

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
        string[] button_titles =
        {
            question.b1, question.b2, question.b3, question.b4, question.b5
        };
        setButtonTitles(
            button_titles
        );
    }

    public void setButtonTitles(string[] values)
    {
        // first make all invis
        HideButtons();

        GameObject[] buttons =
        {
            button1, button2, button3, button4, button5
        };

        for(var i = 0; i < values.Length; i++)
        {
            string val = values[i];
            if (val == "")
            {
                // never mind this button
                continue;
            }

            buttons[i].SetActive(true);

            try
            {
                buttons[i].GetComponent<TMPro.TextMeshPro>().text = val;
            }
            catch (Exception e)
            {
                // if this errors, the text elem is probably on a child obj
                foreach (Transform child in buttons[i].transform)
                {
                    TMPro.TextMeshPro c = child.GetComponent<TMPro.TextMeshPro>();
                    if (c != null)
                    {
                        // found
                        c.text = val;
                        break;
                    }
                }
            }
        }

    }

    public void SetTitle(string str_title)
    {
        TMPro.TextMeshPro title0 = title.GetComponent<TMPro.TextMeshPro>();
        title0.text = str_title;
    }

    void LogQuestions()
    {
        foreach (Questionnaire q in qs.questionnaires)
        {
            foreach (Question question in q.questions)
            {
                //Debug.Log("Question: " + question.question);
            }
        }
        
        UpdateCanvas();
    }

    public bool AnswerQuestion(int button_id)
    {
        log.NewAnswer(button_id);

        if (current_question_count == GetCurrentQuestionnaire().questions.Length -1)
        {
            current_questionnaire_count++;
            current_question_count = 0;

            if (current_questionnaire_count == this.qs.questionnaires.Length)
            {
                current_questionnaire_count = -1;
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
        button1.SetActive(false);
        button2.SetActive(false);
        button3.SetActive(false);
        button4.SetActive(false);
        button5.SetActive(false);
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
            this.sr = JsonUtility.FromJson<ServerResponse>(www.downloadHandler.text);            
        }
    }
}

public class ServerResponse
{
    public string PID;
    public string status;
}