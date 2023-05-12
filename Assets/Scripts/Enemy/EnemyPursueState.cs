using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPursueState : AgentState
{
    [SerializeField] private float Range = 10f;
    [SerializeField] private float ForceMultiplier = 20f;
    [SerializeField] private float FiringDelay = 0f;
    public override void ActivateState(Agent agent)
    {
        
    }

    public override void Update(Agent agent)
    {
        if(((Enemy)agent).GetTarget() != null)
        {
            agent.GetNavAgent().SetDestination(((Enemy)agent).GetTarget().transform.position);
            if(FiringDelay <= 0f)
            {
                if(Vector3.Distance(agent.transform.position, ((Enemy)agent).GetTarget().transform.position) < Range)
                {
                    GameObject projectileToFire = GameObject.Instantiate(((Enemy)agent).GetProjectile());
                    projectileToFire.transform.position = ((Enemy)agent).transform.position + ((Enemy)agent).transform.forward;
                    projectileToFire.transform.LookAt(((Enemy)agent).transform);
                    projectileToFire.GetComponent<Rigidbody>().AddForce(((Enemy)agent).transform.forward * ForceMultiplier, ForceMode.Impulse);
                    FiringDelay = 1f;
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
        // Nothing additional to do
    }
}
