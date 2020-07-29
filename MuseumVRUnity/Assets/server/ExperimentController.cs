using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentController : MonoBehaviour
{
    private ServerHandler sh;


    void Start()
    {
        // get reference to serverhandler component
        this.sh = gameObject.GetComponent<ServerHandler>();

        //StartCoroutine(FillOut());
    }

/*    IEnumerator FillOut()
    {
        //GameObject button = sh.button1;
        
        
            //int button_press = int.Parse(button.name.Substring(6, 7));
        
        
        // let's wait untill things are loaded
        yield return new WaitForSeconds(1);

        sh.UpdateCanvas();

        // answer all questions with "1", untill no more quetions
        while (sh.AnswerQuestion(1))
        {
            // update canvas with next question...
            sh.UpdateCanvas();

            // simulate a little latency, so we can see what's happening
            yield return new WaitForSeconds(1);
        }

        // now questionnaire is done, send to backend server
        sh.SetTitle("Done.");
        sh.HideButtons();

        sh.sendToServer();
    }*/

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            button1Pressed();
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            button2Pressed();
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            button3Pressed();
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            button4Pressed();
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            button5Pressed();
        }
        
    }

    public void button1Pressed()
    {
        _buttonPress(1);
    }
    public void button2Pressed()
    {
        _buttonPress(2);
    }
    public void button3Pressed()
    {
        _buttonPress(3);
    }
    public void button4Pressed()
    {
        _buttonPress(4);
    }
    public void button5Pressed()
    {
        _buttonPress(5);
    }

    IEnumerator CheckForServerResponse()
    {
        while (sh.sr == null)
        {
            // do nothing
            yield return new WaitForSeconds(1);
        }

        // now we have a server response
        Debug.Log("Participant ID: " + sh.sr.PID);

        // MAKE POPUP HERE

    }

    bool finishedQuestionnaireAlready = false;

    private void _buttonPress(int button_id)
    {
        bool more = false;
        try
        {
            more = sh.AnswerQuestion(button_id);
        }
        catch (Exception e)
        {
            // tried to answer question after all have been answered
            more = false;
        }
        
        if (!more)
        {
            if(finishedQuestionnaireAlready) { 
                sh.SetTitle("Thank you for participating! You may now take off your headset.");
                sh.HideButtons();
                sh.sendToServer();
                StartCoroutine(CheckForServerResponse());
            }
            else
            {
                // let's allow for a second fill out of questionnaire pre/post
                finishedQuestionnaireAlready = true;
                sh.Reset();
                Debug.Log("Filled out pre survey, DONE");
            }
        }
        else
        {
            sh.UpdateCanvas();            
        }
    }
    
}
