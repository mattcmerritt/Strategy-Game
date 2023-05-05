using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : Agent
{
    [SerializeField] private int HeldResources = 0, ResourceCapacity = 5;
    [SerializeField] private Vector3 HomePosition;
    [SerializeField] private bool IsGuarded = false;
    [SerializeField] private bool IsScared = false;

    // Visual representation of inventory
    [SerializeField] private List<GameObject> LogsCollected;

    // Start by idling
    private void Start()
    {
        // Save the starting location as a home position
        HomePosition = transform.position;

        // Create new idle state
        ChangeState(new WorkerIdleState());
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
        resManager.AddResources(HeldResources);

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

    // Helper method for guarding and preventing too many guards from following one worker
    public void StartGuarding()
    {
        IsGuarded = true;
    }

    // Helper method for guarding and preventing too many guards from following one worker
    public void StopGuarding()
    {
        IsGuarded = false;
    }

    // Helper method for guarding and preventing too many guards from following one worker
    public bool CheckIfGuarded()
    {
        return IsGuarded;
    }

    // Method to determine what happens when attacked
    public void Scare()
    {
        // Drops all the logs
        HeldResources = 0;

        // Hide the logs from the player
        for (int i = 0; i < LogsCollected.Count; i++)
        {
            LogsCollected[i].SetActive(false);
        }

        // Freeze up
        IsScared = true;
        ChangeState(new ScaredState());
    }

    // Method to determine what happens when attacked
    public bool GetIsScared()
    {
        return IsScared;
    }

    // Method to unscare worker
    public void CalmDown()
    {
        IsScared = false;
    }
}
