using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaredState : AgentState
{
    private float FreezeDelay = 10f;

    public override void ActivateState(Agent agent)
    {
        // no movement while unconscious
        Debug.Log("WORKER: Worker is terrified!");
        agent.GetNavAgent().isStopped = true;
        agent.StartCoroutine(FreezeUp(agent));
    }

    public override void Update(Agent agent)
    {
        // Do nothing
    }

    public override void OnTriggerEnter(Agent agent, Collider other)
    {
        // Do nothing
    }

    public override void EndState(Agent agent)
    {
        agent.GetNavAgent().isStopped = false;
    }

    public IEnumerator FreezeUp(Agent agent)
    {
        Worker worker = ((Worker)agent);

        yield return new WaitForSeconds(FreezeDelay);

        // Regain control and go back home
        worker.CalmDown();
        worker.ChangeState(new WorkerReturnToHomeState());
    }
}
