using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : AgentState
{
    public override void ActivateState(Agent agent)
    {
        
    }

    public override void Update(Agent agent)
    {
        if(((Enemy)agent).FindClosestTarget() != null)
        {
            agent.ChangeState(new EnemyPursueState());
        }
    }

    public override void OnTriggerEnter(Agent agent, Collider other)
    {
        // Do nothing, just stay at home
    }
    public override void OnTriggerStay(Agent agent, Collider other)
    {
        // not implemented
    }

    public override void EndState(Agent agent)
    {
        // Nothing additional to do
    }
}
