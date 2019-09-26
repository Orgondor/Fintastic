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
    private float segmentcounter;

    //SHARK ATTRIBUTES
    public float velocity;
    public Vector3 seedPoint;
    public GameObject nose;

    //SHARK MOVEMENT ATTRIBUTES
    public float amplitude;
    public float frequency;
    public int circles;
    private Animator animator;

    //SHARK BEHAVIOUR ATTRIBUTES
    public float minDistanceToTarget; //determines how close the shark gets to its target

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        animator.speed = velocity;

        if (player == null)
        {

            player = GameObject.Find("Player");

            playerPosition = player.transform.position;

            target = playerPosition;
            targettingPlayer = true;

            circling = false;

            segmentcounter = (2.0f * minDistanceToTarget * 3.1415f) * circles;
            //segmentcounter = 2 * circles;

        }

        this.transform.position = seedPoint;
        this.transform.LookAt(playerPosition);

    }

    // Update is called once per frame
    void Update()
    {
        if (circling)
        {
            //reset and move back to previous target
            if (segmentcounter <= 0)
            {
                circling = false;
                targettingPlayer = !targettingPlayer;
                target = targettingPlayer ? playerPosition : seedPoint;
                z = 0;
                this.transform.LookAt(target);
                segmentcounter = (2.0f * minDistanceToTarget * 3.1415f) * circles;
                //segmentcounter = 2 * circles;

            }
            //circling around the target
            else
            {
                Vector3 direction = clockwise ? (target - this.transform.position) : (this.transform.position - target);
                Vector3 up = new Vector3(0.0f, 1.0f, 0.0f);

                Vector3 looking = this.transform.position + Vector3.Cross(up, direction);
                Vector3 move = new Vector3(0.0f, 0.0f, velocity);

                this.transform.LookAt(looking);
                this.transform.Translate(move);

                segmentcounter-=velocity;

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
            //checking distance
            Vector3 currPosition = nose.transform.position;
            Vector3 direction = target - currPosition;
            float distanceToTarget = Mathf.Sqrt(direction.x * direction.x + direction.y * direction.y + direction.z * direction.z);

            Vector3 move = new Vector3(0.0f, 0.0f, velocity);

            if (distanceToTarget >= minDistanceToTarget)
            {
                z += velocity;
                float x = amplitude * Mathf.Sin(z / frequency);

                Vector3 looking = new Vector3(x, 0.0f, 0.0f) + target;

                this.transform.LookAt(looking);
                this.transform.Translate(move);                
            }


            if (distanceToTarget <= minDistanceToTarget)
            {
                circling = true;
                clockwise = true;
            }

            //close to target
            /*if (distanceToTarget <= 2 * minDistanceToTarget)
            {
                //calculate dotproduct of direction and nose vector of shark
                Vector3 targetToOrigin = targettingPlayer ? (seedPoint - target) : (target - seedPoint);
                Vector3 targetToNose = nose.transform.position - target;
                                
                Vector3 crossOrigin = Vector3.Cross(targetToOrigin, new Vector3(0.0f, 1.0f, 0.0f));
                Vector3 crossNose = Vector3.Cross(targetToNose, new Vector3(0.0f, 1.0f, 0.0f));
                float originLength = Mathf.Sqrt(crossOrigin.x * crossOrigin.x + crossOrigin.y * crossOrigin.y + crossOrigin.z * crossOrigin.z);
                float noseLength = Mathf.Sqrt(crossNose.x * crossNose.x + crossNose.y * crossNose.y + crossNose.z * crossNose.z);

                float angle = Mathf.Acos(Vector3.Dot(crossOrigin, crossNose)/(originLength * noseLength));
                Debug.Log("ANGLE " + angle);

                float dotProduct = Vector3.Dot(crossOrigin, crossNose);
                Debug.Log("DOTPRODUCT " + dotProduct);

                if (angle < 0.01 && angle > -0.01)
                {
                    circling = true;
                    Debug.Log("LETS GO CIRCLE");
                    clockwise = true;// Random.Range(-10.0f, 10.0f) > 0;

                    //this.transform.LookAt(target + (direction / distanceToTarget) * minDistanceToTarget);
                    //this.transform.Translate(move);

                    crossProduct = Vector3.Cross(new Vector3(0.0f, 1.0f, 0.0f), this.transform.forward);
                    previousDotProduct = Vector3.Dot(crossProduct, this.transform.forward);
                }

            }*/
        }

    }
}