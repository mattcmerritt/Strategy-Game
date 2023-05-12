using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerReturnToHomeState : AgentState
{
    private float MinDistanceFromHome = 4f;

    public override void ActivateState(Agent agent)
    {
        Vector3 closestBuildingPos = ((Worker)agent).FindClosestBuilding().transform.position;
        ((Worker)agent).SetHomePosition(closestBuildingPos);
        Debug.Log("WORKER: Returning home now");
        agent.GetNavAgent().SetDestination(((Worker)agent).GetHomePosition());
    }

    public override void Update(Agent agent)
    {
        // If sufficiently close to home, drop off resources and go back out
        Vector3 home = ((Worker)agent).GetHomePosition();
        home.y = agent.transform.position.y;
        // Debug.Log("Distance to home: " + Vector3.Magnitude(home - agent.transform.position));
        if (Vector3.Magnitude(home - agent.transform.position) < MinDistanceFromHome)
        {
            ((Worker)agent).DropResources();
            agent.ChangeState(new WalkToResourceState(agent));
        }
    }

    public override void OnTriggerEnter(Agent agent, Collider other)
    {
        // Do nothing, just keep running to home
    }

    public override void EndState(Agent agent)
    {
        // Nothing additional to do
    }
}
