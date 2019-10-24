using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{ 

    public float time_inminutes;
    public TextMesh watch;
    private float timeleft;

    // Start is called before the first frame update
    void Start()
    {
        timeleft = time_inminutes * 60.0f;
        
        watch.text = timeToString(time_inminutes);
    }

    // Update is called once per frame
    void Update()
    {

        if (timeleft - Time.deltaTime <= 0)
        {
            //do something here
            Debug.Log("TIME OVER");
            watch.text = "00:00";
        }
        else { 
            watch.text = timeToString(timeleft);
            timeleft -= Time.deltaTime;
        }
        
    }

    string timeToString(float time) {
        string front = (Mathf.Floor(timeleft / 60) <= 9 ? "0" : "") + Mathf.Floor(timeleft / 60);
        string decimals = (Mathf.Floor((timeleft % 60)) <= 9 ? "0" : "") + Mathf.Floor((timeleft % 60));
        return front + ":" + decimals;
    }
}
