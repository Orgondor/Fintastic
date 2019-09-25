using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FishSchoolBehavior : MonoBehaviour
{
    private class Fish
    {
        public Rigidbody rb;
        public Transform transform;
        public float speed;
        public Vector3 velocity;
    }

    private struct FishDist
    {
        public Fish fish;
        public float sqrDist;
    }

    private List<Fish> fishes;
    public Transform target;
    public float speedMin = 0.4f;
    public float speedMax = 0.6f;
    public int numberOfCloseFish = 3;
    public float niceDistance = 0.5f;
    public float targetAccelWeight = 4;
    public float closeFishAccelWeight = 4;
    public float avoidanceAccelWeight = 2;
    private float weightSum = 0;


    // Start is called before the first frame update
    void Start()
    {
        weightSum = targetAccelWeight + closeFishAccelWeight + avoidanceAccelWeight;

        fishes = new List<Fish>();
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform trans = transform.GetChild(i);
            GameObject go = trans.gameObject;
            if (go && go.tag == "Fish")
            {
                Fish tmp = new Fish();
                tmp.transform = trans;
                tmp.rb = go.GetComponent<Rigidbody>();
                tmp.speed = Random.Range(speedMin, speedMax);
                tmp.velocity = trans.forward;
                fishes.Add(tmp);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // foreach (var fish in fishes)
        // {
        //     if (fish.transform)
        //     {
        //         fish.transform.LookAt(target);
        //         fish.transform.position = fish.transform.position + fish.transform.forward * fish.speed * Time.deltaTime;
        //     }
        // }
        for (int i = fishes.Count - 1; i >= 0; i--)
        {
            if (!fishes[i].transform)
            {
                fishes.Remove(fishes[i]);
            }
        }
        foreach (var fish in fishes)
        {
            updateFishVelocity(fish);
            // fish.transform.LookAt(fish.transform.position + fish.velocity);
            // fish.transform.position = fish.transform.position + fish.velocity * Time.deltaTime;

            //fish.rb.velocity = fish.accel * Time.deltaTime;
            //fish.transform.LookAt(fish.transform.position + fish.accel);

            // Project the acceletation vector on to the forward-right plane
            Vector3 projected = fish.velocity - Vector3.Dot(fish.velocity, fish.transform.up) * fish.transform.up;
            float Yaw = Mathf.Acos(Vector3.Dot(projected, fish.transform.forward) * (1 / projected.magnitude));
            if (Vector3.Dot(fish.transform.right, projected) < 0)
                Yaw = -Yaw;
            if (Yaw != Yaw)
                Yaw = 0;

            // Project the acceletation vector on to the forward-up plane
            projected = fish.velocity - Vector3.Dot(fish.velocity, fish.transform.right) * fish.transform.right;
            float Pitch = Mathf.Acos(Vector3.Dot(projected, fish.transform.forward) * (1 / projected.magnitude));
            if (Vector3.Dot(fish.transform.up, projected) > 0)
                Pitch = -Pitch;
            if (Pitch != Pitch)
                Pitch = 0;


            fish.rb.velocity = fish.velocity;
            Quaternion rotator = Quaternion.Euler(Pitch, Yaw, -fish.rb.rotation.eulerAngles.z);
            fish.rb.MoveRotation(fish.rb.rotation * rotator);
        }
    }

    private void updateFishVelocity(Fish fish)
    {
        // Toward goal
        Vector3 toTargetAccel = (Vector3.Normalize(target.position - fish.transform.position) * fish.speed) - fish.velocity;

        List<Fish> closest;
        findClosestFish(fish, numberOfCloseFish, out closest);

        // Velocity matching & Collision avoidance
        Vector3 clostestAverage = new Vector3(0,0,0);
        Vector3 avoidanceVec = new Vector3(0,0,0);
        foreach (var close in closest)
        {
            clostestAverage += close.velocity;
            // Add for avoidance if in front
            Vector3 toClose = close.transform.position - fish.transform.position;
            if(Vector3.Dot(fish.transform.forward, toClose) > 0)
            {
                float closeDist = Vector3.Distance(close.transform.position, fish.transform.position);
                Vector3 perpendicular = Vector3.Cross(fish.transform.forward, toClose);
                avoidanceVec += Vector3.Normalize(Vector3.Cross(toClose, perpendicular)) * (niceDistance / closeDist);
            }
        }
        clostestAverage *= 1 / closest.Count;
        Vector3 matchVelocityAccel = clostestAverage - fish.velocity;

        fish.velocity += (toTargetAccel * targetAccelWeight + matchVelocityAccel * closeFishAccelWeight + avoidanceVec * avoidanceAccelWeight) * (1/weightSum) * Time.deltaTime;
    }

    private void findClosestFish(Fish refFish, int num, out List<Fish> closest)
    {
        closest = new List<Fish>();
        List<FishDist> dist = new List<FishDist>();

        foreach (var fish in fishes)
        {
            if(fish.transform != refFish.transform)
            {
                Vector3 toRef = refFish.transform.position-fish.transform.position;
                FishDist fd = new FishDist();
                fd.fish = fish;
                fd.sqrDist = Vector3.Dot(toRef, toRef);
                dist.Add(fd);
            }
        }

        dist.Sort(SortSqrDist);

        for (int i = 0; i < Mathf.Min(num, dist.Count); i++)
        {
            closest.Add(dist[i].fish);
        }
    }

    static int SortSqrDist(FishDist f1, FishDist f2)
     {
         return f1.sqrDist.CompareTo(f2.sqrDist);
     }

    void OnDrawGizmos()
    {
        if (target)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(target.position, 0.1f);
        }

        if (EditorApplication.isPlaying)
        {
            foreach (var fish in fishes)
            {
                if (fish.transform)
                {
                    Gizmos.DrawLine(fish.transform.position, fish.transform.position + fish.velocity);
                }
            }
        }
    }
}
