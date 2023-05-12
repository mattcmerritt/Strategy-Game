using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Agent
{
    [SerializeField] private Worker Target;
    [SerializeField] private GameObject Projectile;
    // Start by idling
    private void Start()
    {
        // Create new idle state
        ChangeState(new EnemyIdleState());
    }

    // Helper method to find the closest tree
    public Worker FindClosestTarget()
    {
        // Find the closest tree
        Worker[] workers = FindObjectsOfType<Worker>();
        float distanceToClosest = Mathf.Infinity;
        Worker closest = null;
        foreach (Worker worker in workers)
        {
            float distanceToWorker = Vector3.Magnitude(worker.transform.position - transform.position);
            if (distanceToWorker <= distanceToClosest)
            {
                closest = worker;
                distanceToClosest = distanceToWorker;
                Target = closest;
            }
        }

        return closest;
    }

    public Worker GetTarget()
    {
        return Target;
    }

    public GameObject GetProjectile()
    {
        return Projectile;
    }
}
