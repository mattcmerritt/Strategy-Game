using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkToHouseState : AgentState
{
    private House Home;
    private float MinDistanceFromHome = 4f;

    public WalkToHouseState(House home)
    {
        Home = home;
    }

    public override void ActivateState(Agent agent)
    {
        // if nothing is left, return home
        Debug.Log("ARCHER: Started to walk towards " + Home.gameObject.name);
        agent.GetNavAgent().SetDestination(Home.transform.position);
    }

    public override void Update(Agent agent)
    {
        // If sufficiently close to home, despawn and spawn new archer
        Vector3 home = Home.transform.position;
        home.y = agent.transform.position.y;
        if (Vector3.Magnitude(home - agent.transform.position) < MinDistanceFromHome)
        {
            Vector3 spawnPos = ((Archer)agent).transform.position;
            agent.ChangeState(null);
            GameObject.Destroy(((Archer)agent).gameObject);
            GameObject worker = GameObject.Instantiate(Home.GetWorkerPrefab());
            worker.transform.position = spawnPos;
            worker.GetComponent<NavMeshAgent>().enabled = true;
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
