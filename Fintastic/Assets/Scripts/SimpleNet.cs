using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleNet : MonoBehaviour
{
    void Start() {
        Rigidbody rb = transform.parent.gameObject.GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.367f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fish"))
        {
            Destroy(other.gameObject);
        }
    }
}
