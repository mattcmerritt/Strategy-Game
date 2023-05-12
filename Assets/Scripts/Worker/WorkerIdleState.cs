using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerIdleState : AgentState
{
    public override void ActivateState(Agent agent)
    {
        if(agent.GetComponent<Worker>() != null)
        {
            Debug.Log("WORKER: Waiting at home");
            agent.GetNavAgent().SetDestination(((Worker)agent).GetHomePosition());
        }
    }

    public override void Update(Agent agent)
    {
        // Should stay in idle forever unless externally commanded
        // See UnitSelector.cs
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
