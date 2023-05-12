using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentEnabler : MonoBehaviour
{
    public List<NavMeshAgent> agents;

    public void Start()
    {
        foreach (NavMeshAgent agent in agents)
        {
            agent.enabled = true;
        }
    }
}
