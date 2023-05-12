using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Agent
{
    [SerializeField] private Worker Target;
    [SerializeField] private GameObject Projectile;
    [SerializeField] private float MinDistance;
    [SerializeField] private int CurrentHealth, MaxHealth = 2;
    // Start by idling
    private void Start()
    {
        // Create new idle state
        ChangeState(new EnemyIdleState());
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage()
    {
        // Take damage, check if dead
        CurrentHealth--;
        if(CurrentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
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
                MinDistance = distanceToClosest;
            }
        }

        return closest;
    }

    public Worker GetTarget()
    {
        return Target;
    }

    public void SetTarget(Worker target)
    {
        Target = target;
    }

    public GameObject GetProjectile()
    {
        return Projectile;
    }

    public float GetMinDistance()
    {
        return MinDistance;
    }
}
