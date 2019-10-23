using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{ 

    public float time;
    private float timeleft;

    // Start is called before the first frame update
    void Start()
    {
        timeleft = time;
    }

    // Update is called once per frame
    void Update()
    {
        timeleft -= Time.deltaTime;

        if (timeleft <= 0) {
            //do something here
            Debug.Log("TIME OVER");
        }
        
    }
}
