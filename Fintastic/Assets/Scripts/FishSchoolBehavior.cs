using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSchoolBehavior : MonoBehaviour
{
    private List<GameObject> fishes;
    public Transform target;
    public float fishSpeed = 1;


    // Start is called before the first frame update
    void Start()
    {
        fishes = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject go = transform.GetChild(i).gameObject;
            if (go && go.tag == "Fish")
            {
                fishes.Add(go);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var fish in fishes)
        {
            fish.transform.LookAt(target);
            fish.transform.position = fish.transform.position + fish.transform.forward * fishSpeed * Time.deltaTime;
        }
    }
}
