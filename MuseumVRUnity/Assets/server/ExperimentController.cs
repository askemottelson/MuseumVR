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

        StartCoroutine(FillOut());
    }

    IEnumerator FillOut()
    {
        // let's wait untill things are loaded
        yield return new WaitForSeconds(1);

        sh.UpdateCanvas();

        // answer all questions with "1", untill no more quetions
        while (sh.AnswerQuestion(1))
        {
            // update canvas with next question...
            sh.UpdateCanvas();

            // simulate a little latency, so we can see what's happening
            yield return new WaitForSeconds(3);
        }

        // now questionnaire is done, send to backend server
        sh.SetTitle("Done.");
        sh.SetLabelLeft("");
        sh.SetLabelRight("");
        sh.HideButtons();

        sh.sendToServer();
    }

    void Update()
    {
        
    }
}
