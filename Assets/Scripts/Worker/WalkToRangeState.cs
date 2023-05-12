using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkToRangeState : AgentState
{
    private ArcheryRange Range;
    private float MinDistanceFromHome = 4f;

    public WalkToRangeState(ArcheryRange range)
    {
        Range = range;
    }

    public override void ActivateState(Agent agent)
    {
        // if nothing is left, return home
        Debug.Log("WORKER: Started to walk towards " + Range.gameObject.name);
        agent.GetNavAgent().SetDestination(Range.transform.position);
    }

    public override void Update(Agent agent)
    {
        // If sufficiently close to home, despawn and spawn new archer
        Vector3 home = Range.transform.position;
        home.y = agent.transform.position.y;
        if (Vector3.Magnitude(home - agent.transform.position) < MinDistanceFromHome)
        {
            Vector3 spawnPos = ((Worker)agent).transform.position;
            // Debug.Log($"spos: {spawnPos}");
            agent.ChangeState(null);
            GameObject.Destroy(((Worker)agent).gameObject);
            GameObject archer = GameObject.Instantiate(Range.GetArcherPrefab());
            archer.transform.position = spawnPos;
            // Debug.Log($"apos: {archer.transform.position}");
            archer.GetComponent<NavMeshAgent>().enabled = true;
        }
    }

    public override void OnTriggerEnter(Agent agent, Collider other)
    {
        // Nothing additional to do
    }

    public override void EndState(Agent agent)
    {
        // Nothing additional to do
    }
}
