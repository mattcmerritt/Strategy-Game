using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Agent
{
    [SerializeField] private Agent Target;
    [SerializeField] private GameObject Projectile;
    [SerializeField] private float MinDistance;
    [SerializeField] private int CurrentHealth, MaxHealth = 2;
    public static int DeathCount = 0;

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
            DeathCount++;
            Destroy(this.gameObject);
        }
    }

    // Helper method to find the closest tree
    public Agent FindClosestTarget()
    {
        // Find the closest tree
        Worker[] workers = FindObjectsOfType<Worker>();
        float distanceToClosest = Mathf.Infinity;
        Agent closest = null;
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

        Archer[] archers = FindObjectsOfType<Archer>();
        foreach (Archer archer in archers)
        {
            float distanceToWorker = Vector3.Magnitude(archer.transform.position - transform.position);
            if (distanceToWorker <= distanceToClosest)
            {
                closest = archer;
                distanceToClosest = distanceToWorker;
                Target = closest;
                MinDistance = distanceToClosest;
            }
        }

        return closest;
    }

    public Agent GetTarget()
    {
        return Target;
    }

    public void SetTarget(Agent target)
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
