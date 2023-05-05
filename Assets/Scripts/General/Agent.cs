using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Parent class for all nav agents in the scene
// Contains the basic information for the agent to interact with the nav mesh
public class Agent : MonoBehaviour
{
    [SerializeField] private NavMeshAgent NavAgent;
    protected AgentState ActiveState;

    // Helper method to retrieve the NavAgent component
    public NavMeshAgent GetNavAgent()
    {
        return NavAgent;
    }

    // Method to swap from one state to another
    public void ChangeState(AgentState state)
    {
        // End all coroutines currently running on the agent
        StopAllCoroutines();
        if (ActiveState != null)
        {
            ActiveState.EndState(this);
        }

        // Update state
        if (state != null)
        {
            state.ActivateState(this);
        }
        ActiveState = state;
    }

    // Delegate this task to the current state
    private void Update()
    {
        if (ActiveState != null)
        {
            ActiveState.Update(this);
        }
    }

    // Delegate this task to the current state
    private void OnTriggerEnter(Collider other)
    {
        if (ActiveState != null)
        {
            ActiveState.OnTriggerEnter(this, other);
        }
    }
}
