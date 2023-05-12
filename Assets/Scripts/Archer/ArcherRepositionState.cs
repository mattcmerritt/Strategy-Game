using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ArcherRepositionState : AgentState
{
    private float MinDistanceFromPos = 0.25f;

    public override void ActivateState(Agent agent)
    {
        // do nothing
    }

    public override void Update(Agent agent)
    {
        Vector3 newPos = ((Archer)agent).GetNewPosition();
        newPos.y = agent.transform.position.y;
        agent.GetNavAgent().SetDestination(newPos);
        if (Vector3.Magnitude(newPos - agent.transform.position) < MinDistanceFromPos)
        {
            agent.ChangeState(new ArcherIdleState());
        }
    }

    public override void OnTriggerEnter(Agent agent, Collider other)
    {
        // Nothing additional to do
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
