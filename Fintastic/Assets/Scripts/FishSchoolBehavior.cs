using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSchoolBehavior : MonoBehaviour
{
    private struct Fish
    {
        public Transform transform;
        public float speed;
        public Vector3 velocity;
        public Vector3 acceleration;
    }
    private List<Fish> fishes;
    public Transform target;
    public float speedMin = 1;
    public float speedMax = 2;


    // Start is called before the first frame update
    void Start()
    {
        fishes = new List<Fish>();
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform trans = transform.GetChild(i);
            GameObject go = trans.gameObject;
            if (go && go.tag == "Fish")
            {
                Fish tmp = new Fish();
                tmp.transform = trans;
                tmp.speed = Random.Range(speedMin, speedMax);
                tmp.velocity = trans.forward * tmp.speed;
                fishes.Add(tmp);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var fish in fishes)
        {
            if (fish.transform)
            {
                fish.transform.LookAt(target);
                fish.transform.position = fish.transform.position + fish.transform.forward * fish.speed * Time.deltaTime;
            }
        }

        //foreach (var fish in fishes)
        //{
        //    fish.transform.position = fish.transform.position + fish.velocity * Time.deltaTime;
        //}
    }

    private void updateFishVelocity(Fish fish)
    {
        Vector3 toTargetAccel = (Vector3.Normalize(target.position - fish.transform.position) * fish.speed) - fish.velocity;

        //Fish[3] closest;
    }

    void OnDrawGizmos()
    {
        if (target)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(target.position, 0.2f);
        }
    }
}
