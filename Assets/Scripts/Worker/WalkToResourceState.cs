using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkToResourceState : AgentState
{
    private ResourceSource Resource;

    public WalkToResourceState(ResourceSource resource)
    {
        Resource = resource;
    }

    public override void ActivateState(Agent agent)
    {
        // if nothing is left, return home
        if (Resource != null)
        {
            Debug.Log("WORKER: Started to walk towards " + Resource.gameObject.name);
            agent.GetNavAgent().SetDestination(Resource.transform.position);
        }
    }

    public override void Update(Agent agent)
    {
        if (Resource == null)
        {
            agent.ChangeState(new WorkerReturnToHomeState());
        }
        // if the tree has resources left, find another tree
        if (Resource != null && Resource.IsDepleted())
        {
            ResourceSource closest = ((Worker)agent).FindClosestResource();
            agent.ChangeState(new WalkToResourceState(closest));
        }
    }

    public override void OnTriggerEnter(Agent agent, Collider other)
    {
        // if agent has reached its destination, start gathering
        if (Resource != null && other.gameObject == Resource.gameObject)
        {
            agent.ChangeState(new GatherResourceState(Resource));
        }
    }

    public override void EndState(Agent agent)
    {
        // Nothing additional to do
    }
}
