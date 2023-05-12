using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionRadius : MonoBehaviour
{
    [SerializeField] private Agent agent;

    private void OnTriggerEnter(Collider other)
    {
        // verify that the collider does not belong to the agent themselves
        if (agent != null && other.gameObject == agent.gameObject)
        {
            return;
        }
        // defer triggered events to agents
        else if(agent != null)
        {
            agent.OnTriggerEnter(other);
        }
        else
        {
            // do nothing - agent is being deconstructed
        }
    }
}
