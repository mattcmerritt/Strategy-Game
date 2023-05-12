using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPursueState : AgentState
{
    [SerializeField] private float Range = 15f; // distance at which the enemy will start firing
    [SerializeField] private float HorizontalForceMultiplier = -0.75f; // initial power of the projectile
    [SerializeField] private float HorizontalForceMax = 20f; // max horizontal power of the projectile
    [SerializeField] private float VerticalForceMultiplier = 0.5f; // initial power of the projectile
    [SerializeField] private float FiringDelay = 0f; // current firing delay
    [SerializeField] private float FireCooldown = 2f; // max firing delay
    [SerializeField] private float StandDistance = 7f; // distance where enemy will not get closer

    public override void ActivateState(Agent agent)
    {
        
    }

    public override void Update(Agent agent)
    {
        Enemy enemy = (Enemy) agent;
        Agent target = enemy.GetTarget();

        if (enemy.GetTarget() != null)
        {
            agent.GetNavAgent().SetDestination(target.transform.position);

            // tell the enemy to stop chasing if they are close enough to the target
            

            if (Vector3.Distance(enemy.transform.position, target.transform.position) < StandDistance)
            {
                agent.GetNavAgent().isStopped = true;
                // turn to face target
                agent.transform.LookAt(target.transform);
            }
            else
            {
                agent.GetNavAgent().isStopped = false;
                agent.transform.LookAt(target.transform);
            }

            if (FiringDelay <= 0f)
            {
                if(Vector3.Distance(agent.transform.position, target.transform.position) < Range)
                {
                    GameObject projectileToFire = GameObject.Instantiate(enemy.GetProjectile());
                    projectileToFire.transform.position = enemy.transform.position + enemy.transform.forward;
                    projectileToFire.transform.LookAt(enemy.transform);

                    float shotVerticalForce = Vector3.Distance(agent.transform.position, target.transform.position) * VerticalForceMultiplier;
                    float shotHorizontalForce = HorizontalForceMax + Vector3.Distance(agent.transform.position, target.transform.position) * HorizontalForceMultiplier;

                    projectileToFire.GetComponent<Rigidbody>().AddForce(enemy.transform.forward * shotHorizontalForce + Vector3.up * shotVerticalForce, ForceMode.Impulse);
                    FiringDelay = FireCooldown;
                }
            }
            else
            {
                FiringDelay -= Time.deltaTime;
            }
        }
        else
        {
            agent.ChangeState(new EnemyIdleState());
        }
        
    }

    public override void OnTriggerEnter(Agent agent, Collider other)
    {
        // Do nothing, just stay at home
    }

    public override void EndState(Agent agent)
    {
        // Ensure that the agent is not frozen
        agent.GetNavAgent().isStopped = false;
    }
}
