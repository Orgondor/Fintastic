using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour
{

    public TextMesh counter_text;

    private int fishCaught = 0;

    // Start is called before the first frame update
    void Start()
    {
        counter_text.text = fishCaught.ToString();
    }

    // Update is called once per frame
    void Update()
    {

        bool correctFish = true;

        if (correctFish) {
            fishCaught++;
        }

        counter_text.text = fishCaught.ToString();

    }
}
