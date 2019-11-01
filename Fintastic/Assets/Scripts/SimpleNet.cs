using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleNet : MonoBehaviour
{
    public Transform caughtPos;
    public GameObject caughtFish;

    void Start() {
        caughtFish = null;
        Rigidbody rb = transform.parent.gameObject.GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.367f, 0);
    }

    private void Update()
    {
        if (caughtFish)
        {
            caughtFish.transform.position = caughtPos.position;
            caughtFish.transform.rotation = caughtPos.rotation;
        }
    }

    public void Deliver()
    {
        Destroy(caughtFish);
        caughtFish = null;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!caughtFish && other.CompareTag("Fish"))
        {
            Debug.Log("caught!");
            caughtFish = Instantiate(other.gameObject.GetComponent<FishCollision>().School.fishPrefab);

            Debug.Log(caughtFish);
            Destroy(other.gameObject);
        }
    }
}
