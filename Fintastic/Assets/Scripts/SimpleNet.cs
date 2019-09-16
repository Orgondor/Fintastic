using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleNet : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collission!!!");

        if (other.CompareTag("Fish"))
        {

            Debug.Log("Fish!!!");
            Destroy(other.gameObject);
        }
    }
}
