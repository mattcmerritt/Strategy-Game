using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkToTowerState : AgentState
{
    private Tower Tower;
    private float MinDistanceFromHome = 4f;

    public WalkToTowerState(Tower tower)
    {
        Tower = tower;
    }

    public override void ActivateState(Agent agent)
    {
        // if nothing is left, return home
        Debug.Log("ARCHER: Started to walk towards " + Tower.gameObject.name);
        agent.GetNavAgent().SetDestination(Tower.GetComponent<Collider>().ClosestPoint(agent.transform.position));
    }

    public override void Update(Agent agent)
    {
        Vector3 home = Tower.transform.position;
        home.y = agent.transform.position.y;

        // If sufficiently close to home, move into tower
        if (Vector3.Magnitude(home - agent.transform.position) < MinDistanceFromHome)
        {
            GameObject character = agent.gameObject;
            Tower.SetGarrisonedUnit(character);
            agent.ChangeState(null);
        }
    }

    public override void OnTriggerEnter(Agent agent, Collider other)
    {
        // Nothing additional to do
    }

    public override void OnTriggerStay(Agent agent, Collider other)
    {
        // Nothing additional to do
    }

    public override void EndState(Agent agent)
    {
        // Nothing additional to do
    }
}
