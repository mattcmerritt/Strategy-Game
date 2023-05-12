using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceManager : MonoBehaviour
{
    [SerializeField] private Dictionary<Resource, int> ResourcesCollected;

    public void AddResources(Resource type, int additional)
    {
        ResourcesCollected[type] += additional;
    }
}
