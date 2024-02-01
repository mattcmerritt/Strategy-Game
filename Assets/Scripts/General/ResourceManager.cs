using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceManager : MonoBehaviour
{
    [SerializeField] private Dictionary<Resource, int> ResourcesCollected = new Dictionary<Resource, int>();

    // Setting up singleton reference
    public static ResourceManager Instance;

    private void Start()
    {
        Instance = this;

        ResourcesCollected[Resource.Food] = 0;
        ResourcesCollected[Resource.Wood] = 0;
    }

    public void AddResources(Resource type, int additional)
    {
        ResourcesCollected[type] += additional;
    }

    public int GetResourceStockpile(Resource type)
    {
        return ResourcesCollected[type];
    }

    public void RemoveResource(Resource type, int amount)
    {
        ResourcesCollected[type] -= amount;
    }
}
