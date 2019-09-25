using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkBehaviour : MonoBehaviour
{

    private bool targettingPlayer;
    private Vector3 target;

    private GameObject player = null;
    private Vector3 playerPosition;

    private float z = 0;
    private bool clockwise;

    private float previousDotProduct = 0;
    private Vector3 crossProduct;

    private bool circling;
    private int segmentcounter;

    //SHARK ATTRIBUTES
    public float velocity;
    public Vector3 seedPoint;

    //SHARK MOVEMENT ATTRIBUTES
    public float amplitude;
    public float frequency;
    public int circles;

    //SHARK BEHAVIOUR ATTRIBUTES
    public float minDistanceToTarget; //determines how close the shark gets to its target

    // Start is called before the first frame update
    void Start()
    {

        if (player == null)
        {

            player = GameObject.Find("Player");

            playerPosition = player.transform.position;

            target = playerPosition;
            targettingPlayer = true;

            circling = false;
            segmentcounter = 2 * circles;

        }

        this.transform.position = seedPoint;
        this.transform.LookAt(playerPosition);

    }

    // Update is called once per frame
    void Update()
    {
        if (circling)
        {

            if (segmentcounter <= 0)
            {
                circling = false;
                targettingPlayer = !targettingPlayer;
                target = targettingPlayer ? playerPosition : seedPoint;
                z = 0;
                this.transform.LookAt(target);
                segmentcounter = 2 * circles;

            }
            else
            {

                Vector3 direction = clockwise ? (target - this.transform.position) : (this.transform.position - target);
                Vector3 up = new Vector3(0.0f, 1.0f, 0.0f);

                Vector3 looking = this.transform.position + Vector3.Cross(up, direction);
                Vector3 move = new Vector3(0.0f, 0.0f, velocity);

                this.transform.LookAt(looking);
                this.transform.Translate(move);

                /*float currentDotProduct = Vector3.Dot(crossProduct, this.transform.forward);
                Debug.Log("DOTS-- current " + currentDotProduct + " previous " + previousDotProduct);
                if (currentDotProduct >= 0 && previousDotProduct < 0)
                {
                    segmentcounter--;
                }
                else if (currentDotProduct <= 0 && previousDotProduct > 0) {
                    segmentcounter--;
                }
                previousDotProduct = currentDotProduct;*/

            }
        }
        else
        {
            z += velocity;
            float x = amplitude * Mathf.Sin(z);

            Vector3 move = new Vector3(0.0f, 0.0f, velocity);
            Vector3 looking = new Vector3(x, 0.0f, 0.0f) + target;

            this.transform.LookAt(looking);
            this.transform.Translate(move);

            //checking distance
            Vector3 currPosition = this.transform.position;

            Vector3 direction = target - currPosition;
            float distanceToTarget = Mathf.Sqrt(direction.x * direction.x + direction.y * direction.y + direction.z * direction.z);
            if (distanceToTarget <= minDistanceToTarget)
            {
                circling = true;
                crossProduct = Vector3.Cross(new Vector3(0.0f, 1.0f, 0.0f), this.transform.forward);
                previousDotProduct = Vector3.Dot(crossProduct, this.transform.forward);
                clockwise = Random.Range(-10.0f, 10.0f) > 0;
            }
        }

    }
}