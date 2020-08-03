using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CovidScript : MonoBehaviour
{
    public GameObject virus;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame


    void Update()
    {
        // rotate corona virus
        virus.transform.Rotate(.02f, .02f, .0f, Space.Self);




    }
}
