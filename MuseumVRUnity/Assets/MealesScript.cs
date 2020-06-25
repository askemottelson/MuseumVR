using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MealesScript : MonoBehaviour
{
    public GameObject ship;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    int count = 0;
    int frames = 1000;
    int orientation = 1;
    float deltaX = 0.00001f;
    // Update is called once per frame
    void Update()
    {
        // make ship wavy
        count++;
        if (count == frames)
        {
            orientation *= -1;
            count = 0;
        }
        ship.transform.Rotate((frames - count) * orientation * deltaX, 0.0f, 0.0f);
    

    }
}
