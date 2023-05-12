using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherResourceState : AgentState
{
    private ResourceSource Resource;
    private float CollectionDelay = 1;

    public GatherResourceState(ResourceSource resource)
    {
        Resource = resource;
    }

    public override void ActivateState(Agent agent)
    {
        Debug.Log("WORKER: Started to gather from " + Resource.gameObject.name);
        agent.StartCoroutine(CollectResource(agent));
    }

    public override void Update(Agent agent)
    {
        // if the tree has resources left, find another tree
        if (Resource.IsDepleted())
        {
            ResourceSource closest = ((Worker)agent).FindClosestResource();
            agent.ChangeState(new WalkToResourceState(closest));
        }
    }

    public override void OnTriggerEnter(Agent agent, Collider other)
    {
        // not implemented
    }

    public override void EndState(Agent agent)
    {
        // Nothing additional to do
    }

    public IEnumerator CollectResource(Agent agent)
    {
        Worker worker = ((Worker)agent);

        // return to base if you cannot hold any more
        if (!worker.CanHoldMoreResources())
        {
            worker.ChangeState(new WorkerReturnToHomeState());
            yield return null;
        }
        // otherwise, keep gathering more
        else
        {
            // wait
            yield return new WaitForSeconds(CollectionDelay);

            // add to resource total
            Resource.TakeResource();
            worker.PickUpResource();
            // keep gathering
            agent.StartCoroutine(CollectResource(agent));
        }  
    }
}
