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
            if (watch.text == "00:00")
            {
                watch.text = "     ";
            }
            else
            {
                watch.text = "00:00";
            }
        }
        else { 
            watch.text = timeToString(timeleft);
            timeleft -= Time.deltaTime;
        }

        if (timeleft - Time.deltaTime <= time_inminutes * 60.0f + time_offset_inseconds) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
    }

    string timeToString(float time) {
        string front = (Mathf.Floor(timeleft / 60) <= 9 ? "0" : "") + Mathf.Floor(timeleft / 60);
        string decimals = (Mathf.Floor((timeleft % 60)) <= 9 ? "0" : "") + Mathf.Floor((timeleft % 60));
        return front + ":" + decimals;
    }
}
