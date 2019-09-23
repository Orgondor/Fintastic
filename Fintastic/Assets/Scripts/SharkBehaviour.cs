using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkBehaviour : MonoBehaviour
{
    private GameObject player = null;
    private Vector3 playerPosition = new Vector3(0.0f,0.0f,0.0f);
    private Vector3 sharkPosition = new Vector3(0.0f,0.0f,0.0f);
    private Vector3 direction = new Vector3(0.0f,0.0f,0.0f);

    public float velocity;
    public float minDistance;
    public float maxDistance;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GameObject.Find("Player");

            /*playerPosition = player.transform.position;
            sharkPosition = this.transform.position;
            direction = playerPosition - sharkPosition;

            Debug.Log("INIT - PLAYER: " + playerPosition);
            Debug.Log("INIT - SHARK: " + sharkPosition);
            Debug.Log("INIT - DIRECTION: " + direction);*/
        }

    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log("Translating Shark...");
        sharkPosition = this.transform.position;
        playerPosition = player.transform.position;
        direction = sharkPosition - playerPosition;

        this.transform.position = this.transform.position + direction * velocity;
        
        //ROTATE SHARK TO THE FACE OF THE PLAYER
        

        //CHECK IF SHARK IS TOO CLOSE/FAR
        Vector3 distance = this.transform.position - playerPosition;
        float length = distance.x * distance.x + distance.y * distance.y + distance.z * distance.z;
        if (length <= minDistance || length >= maxDistance) {
            direction = -direction;
        }
        
    }
}
