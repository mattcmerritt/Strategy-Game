using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Agent
{
    [SerializeField] private int CurrentHealth, MaxHealth = 5;
    [SerializeField] private Enemy Target;
    [SerializeField] private GameObject Projectile;

    // Start by idling
    private void Start()
    {
        // Create new idle state
        ChangeState(new WorkerIdleState());

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

    public Enemy GetTarget()
    {
        return Target;
    }

    public void SetTarget(Enemy t)
    {
        Target = t;
    }

    public GameObject GetProjectile()
    {
        return Projectile;
    }
}
