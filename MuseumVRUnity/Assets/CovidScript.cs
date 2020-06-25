using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CovidScript : MonoBehaviour
{
    public GameObject virus;
    public GameObject bat;
    public GameObject batFeet;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    float max_bat_z = 5.0f;
    float base_bat_z = 230.0f;
    int orientation = 1;
    void Update()
    {
        // rotate corona virus
        virus.transform.Rotate(.02f, .02f, .0f, Space.Self);

        // dangle bat
        if(bat.transform.eulerAngles.z > base_bat_z + max_bat_z )
        {
            orientation *= -1;
        }
        if (bat.transform.eulerAngles.z < base_bat_z - max_bat_z)
        {
            orientation *= -1;
        }


        //bat.transform.Rotate(.0f, .0f, .005f * orientation);
        bat.transform.RotateAround(batFeet.transform.forward, .0001f * orientation);
    }
}
