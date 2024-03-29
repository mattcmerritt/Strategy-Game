using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Agent
{
    [SerializeField] private int CurrentHealth, MaxHealth = 5;
    [SerializeField] private Enemy Target;
    [SerializeField] private GameObject Projectile;
    [SerializeField] private Vector3 NewPosition;

    // Start by idling
    private void Start()
    {
        // Create new idle state
        ChangeState(new ArcherIdleState());

        CurrentHealth = MaxHealth;
    }

    public void TakeDamage()
    {
        // invincible if in tower
        if (IsSafe)
        {
            return;
        }

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

    public string GetHealthString()
    {
        return $"{CurrentHealth}/{MaxHealth}";
    }

    public Vector3 GetNewPosition()
    {
        return NewPosition;
    }

    public void SetNewPosition(Vector3 pos)
    {
        NewPosition = pos;
    }
}
