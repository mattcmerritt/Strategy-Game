using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherIdleState : AgentState
{
    [SerializeField] private float DetectionRange = 20f;

    public override void ActivateState(Agent agent)
    {
        if(agent.GetComponent<Worker>() != null)
        {
            Debug.Log("ARCHER: Waiting");
        }
    }

    public override void Update(Agent agent)
    {
        // Should stay in idle unless an enemy comes near
        Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            float distanceToEnemy = Vector3.Magnitude(enemy.transform.position - agent.transform.position);
            if (distanceToEnemy <= DetectionRange)
            {
                ((Archer)agent).SetTarget(enemy);
                ArcherAttackState guardTask = new ArcherAttackState();
                agent.ChangeState(guardTask);
            }
        }
    }

    public override void OnTriggerEnter(Agent agent, Collider other)
    {
        // Do nothing, just stay at home
    }

    public override void EndState(Agent agent)
    {
        // Nothing additional to do
    }
}
