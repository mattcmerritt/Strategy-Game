using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AgentState
{
    public abstract void ActivateState(Agent agent);
    public abstract void Update(Agent agent);
    public abstract void OnTriggerEnter(Agent agent, Collider other);
    public abstract void EndState(Agent agent);
}
