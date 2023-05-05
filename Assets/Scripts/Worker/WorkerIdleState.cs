using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerIdleState : AgentState
{
    public override void ActivateState(Agent agent)
    {
        Debug.Log("WORKER: Waiting at home");
        agent.GetNavAgent().SetDestination(((Worker)agent).GetHomePosition());
    }

    public override void Update(Agent agent)
    {
        // Do nothing unless the resource goal is not met
        // If it is not met, go get more resources
        ResourceManager resManager = GameObject.FindObjectOfType<ResourceManager>();
        if (!resManager.HasMetResourceGoal())
        {
            ResourceSource closest = ((Worker)agent).FindClosestResource();
            if (closest != null)
            {
                agent.ChangeState(new WalkToResourceState(closest));
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
