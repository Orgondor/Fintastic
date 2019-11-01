using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{ 

    public float time_inminutes;
    public float time_offset_inseconds;
    public TextMesh watch;
    private float timeleft;
    private float timeOffset = 0.0f;
    private float prevTimeOffset = 0.0f;

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
 	    timeOffset += Time.deltaTime;
            if (timeOffset - prevTimeOffset >= 0.5)
            {
		if (watch.text == "00:00"){
			watch.text = "     ";
		}                
            	else
            	{
                	watch.text = "00:00";
            	}
	    	prevTimeOffset = timeOffset;
	    }	
        }
        else { 
            watch.text = timeToString(timeleft);
        }

	timeleft -= Time.deltaTime;	
        /*if (timeleft + time_offset_inseconds <= 0) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	    timeleft = time_inminutes * 60.0f;
            watch.text = timeToString(time_inminutes);
        }*/
        
    }

    string timeToString(float time) {
        string front = (Mathf.Floor(timeleft / 60) <= 9 ? "0" : "") + Mathf.Floor(timeleft / 60);
        string decimals = (Mathf.Floor((timeleft % 60)) <= 9 ? "0" : "") + Mathf.Floor((timeleft % 60));
        return front + ":" + decimals;
    }
}
