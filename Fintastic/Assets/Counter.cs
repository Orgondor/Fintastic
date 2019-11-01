using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour
{
    public enum FishType { RedBlack, RedBetta, Discus};

    public TextMesh counter_text;
    public FishType type;

    private int fishCaught = 0;

    // Start is called before the first frame update
    void Start()
    {
        counter_text.text = fishCaught.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        //TODO specify type of fish and change in CompareTag()
        //declare own variable for that
        bool correctFish = true;

        if (other.CompareTag("Net") && correctFish)
        {
            SimpleNet net = other.GetComponentInChildren<SimpleNet>();
            if (net && net.caughtFish)
            {
                fishCaught++;
                counter_text.text = fishCaught.ToString();
                net.Deliver();
            }
            //TODO let fish appear inside of basket
            //TODO remove fish from net
        }

    }

}
