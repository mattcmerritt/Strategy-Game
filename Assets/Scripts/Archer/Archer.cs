using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Agent
{
    [SerializeField] private int HeldResources = 0, ResourceCapacity = 5;
    [SerializeField] private Vector3 HomePosition;
    [SerializeField] private ResourceSource CurrentFocus;

    // Visual representation of inventory
    [SerializeField] private List<GameObject> LogsCollected;
    [SerializeField] private int CurrentHealth, MaxHealth = 5;

    // Start by idling
    private void Start()
    {
        // Save the starting location as a home position
        HomePosition = transform.position;

        // Create new idle state
        ChangeState(new WorkerIdleState());

        CurrentHealth = MaxHealth;
    }

    // Helper method to find the closest tree
    public ResourceSource FindClosestResource()
    {
        // Find the closest tree
        ResourceSource[] trees = FindObjectsOfType<ResourceSource>();
        float distanceToClosest = Mathf.Infinity;
        ResourceSource closest = null;
        foreach (ResourceSource tree in trees)
        {
            float distanceToTree = Vector3.Magnitude(tree.transform.position - transform.position);
            if (distanceToTree <= distanceToClosest)
            {
                closest = tree;
                distanceToClosest = distanceToTree;
            }
        }

        return closest;
    }

    // Helper method to check if the worker can pick up more
    public bool CanHoldMoreResources()
    {
        return HeldResources < ResourceCapacity;
    }

    // Helper method to pick a single resource
    public void PickUpResource()
    {
        HeldResources += 1;

        // Show the logs to the player
        for (int i = 0; i < HeldResources; i++)
        {
            LogsCollected[i].SetActive(true);
        }
    }

    // Helper method to remove resources from the worker's hands
    public void DropResources()
    {
        // Adding resources to totals
        ResourceManager resManager = FindObjectOfType<ResourceManager>();
        // resManager.AddResources(HeldResources);

        HeldResources = 0;

        // Hide the logs from the player
        for (int i = 0; i < LogsCollected.Count; i++)
        {
            LogsCollected[i].SetActive(false);
        }
    }

    // Helper method to retrieve home position
    public Vector3 GetHomePosition()
    {
        return HomePosition;
    }

    // Helper method to set new home position (where to stand still at next)
    public void SetHomePosition(Vector3 pos)
    {
        HomePosition = pos;
    }

    // Helper for getting current focused resource source
    public ResourceSource GetFocus()
    {
        return CurrentFocus;
    }

    // Helper for setting current focused resource source
    public void SetFocus(ResourceSource resource)
    {
        CurrentFocus = resource;
    }

    public void TakeDamage()
    {
        // Take damage, check if dead, drop the logs, and return to the building
        CurrentHealth--;
        ChangeState(new WorkerReturnToHomeState());
        if(CurrentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
        // Hide the logs from the player
        for (int i = 0; i < LogsCollected.Count; i++)
        {
            LogsCollected[i].SetActive(false);
        }
        HeldResources = 0;
    }
}
