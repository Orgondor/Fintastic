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
    public GameObject fishPrefab;

    public Transform target;
    public Transform spawnPoint;
    public float respawnThreshold = 0.7f;
    public float respawnTimeMin = 0.5f;
    public float respawnTimeMax = 5.0f;
    public float speedMin = 0.4f;
    public float speedMax = 0.6f;
    public int numberOfCloseFish = 3;
    public float niceDistance = 0.5f;
    public float targetAccelWeight = 4;
    public float closeFishAccelWeight = 4;
    public float avoidanceAccelWeight = 2;
    private float weightSum = 0;
    public float avoidTridentWeight = 10;

    private int initialSchoolSize = 0;
    private float respawnTime = 0.0f;

    //public GameObject tridentPrefab;
    //public GameObject trident;
    //public Transform triPos;

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

        initialSchoolSize = fishes.Count;

        //if (trident == null) //finds the trident object
        //    trident = GameObject.FindWithTag("Trident");
        //Instantiate(tridentPrefab, trident.transform.position, trident.transform.rotation);
    }

    void Update()
    {
        respawnTime = Mathf.Max(0.0f, respawnTime - Time.deltaTime);

        // Respawn new fish
        if (respawnTime <= 0.0f && fishes.Count <= initialSchoolSize * respawnThreshold)
        {
            GameObject go = GameObject.Instantiate(fishPrefab, spawnPoint.position, spawnPoint.rotation);
            go.transform.SetParent(transform);

            Fish tmp = new Fish();
            tmp.transform = go.transform;
            tmp.rb = go.GetComponent<Rigidbody>();
            tmp.speed = Random.Range(speedMin, speedMax);
            tmp.velocity = go.transform.forward;
            fishes.Add(tmp);

            respawnTime = Random.Range(respawnTimeMin, respawnTimeMax);
        }
    }

    void FixedUpdate()
    {
        // Remove any fish that don't exist anymore
        for (int i = fishes.Count - 1; i >= 0; i--)
        {
            if (!fishes[i].transform)
            {
                fishes.Remove(fishes[i]);
            }
        }

        // Movement
        foreach (var fish in fishes)
        {
            updateFishVelocity(fish);

            fish.rb.velocity = fish.velocity;
            fish.rb.MoveRotation(Quaternion.LookRotation(fish.velocity, new Vector3(0,1,0)));
        }

    }

    private void updateFishVelocity(Fish fish)
    {
        // Toward goal
        Vector3 toTargetAccel = (Vector3.Normalize(target.position - fish.transform.position) * fish.speed) - fish.velocity;

        Vector3 awayFromTrident = new Vector3(0, 0, 0);

        //if (Vector3.Distance(fish.transform.position, trident.transform.position)<1) //scare the fish with the trident.
        //{
        //    awayFromTrident += Vector3.Normalize(fish.transform.position - trident.transform.position)*10;
        //}

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

        fish.velocity += (toTargetAccel * targetAccelWeight + matchVelocityAccel * closeFishAccelWeight + avoidanceVec * avoidanceAccelWeight + awayFromTrident* avoidTridentWeight) * (1/weightSum) * Time.deltaTime;
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
        if (spawnPoint)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(spawnPoint.position, 0.1f);
            Gizmos.DrawLine(spawnPoint.position, spawnPoint.position + spawnPoint.forward);
        }

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
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(fish.transform.position, fish.transform.position + fish.velocity);
                }
            }
        }
    }

}
