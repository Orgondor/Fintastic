using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishCollision : MonoBehaviour
{
    private Vector3 CollisionAvoidanceDir;
    public Vector3 CollisionAvoidanceVec;
    public float CollisionAvoidanceStartSpeed = 100.0f;
    public float SpeedDecayTime = 0.1f;
    private float SpeedDecayFactor;
    private float CollisionAvoidanceSpeed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        CollisionAvoidanceDir = Vector3.zero;
        CollisionAvoidanceVec = Vector3.zero;
        SpeedDecayFactor = 1.0f / SpeedDecayTime;
    }

    // Update is called once per frame
    void Update()
    {
        CollisionAvoidanceSpeed = Mathf.Max(0.0f, CollisionAvoidanceSpeed - CollisionAvoidanceSpeed * Time.deltaTime * SpeedDecayFactor);
        CollisionAvoidanceVec = CollisionAvoidanceDir * CollisionAvoidanceSpeed;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Fish")
        {
            ContactPoint contact = collision.contacts[0];
            CollisionAvoidanceDir = Vector3.Normalize(transform.position - contact.point);
            CollisionAvoidanceSpeed = CollisionAvoidanceStartSpeed;
            CollisionAvoidanceVec = CollisionAvoidanceDir * CollisionAvoidanceSpeed;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + CollisionAvoidanceVec);
    }
}
