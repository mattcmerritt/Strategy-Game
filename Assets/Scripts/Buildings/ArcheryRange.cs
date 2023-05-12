using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ArcheryRange : Building
{
    [SerializeField] private GameObject ArcherPrefab;

    public GameObject GetArcherPrefab()
    {
        return ArcherPrefab;
    }

    public void CreateArcher()
    {
        ResourceManager rm = FindObjectOfType<ResourceManager>();
        if (rm.GetResourceStockpile(Resource.Wood) >= 10 && rm.GetResourceStockpile(Resource.Food) >= 30)
        {
            GameObject workerObj = Instantiate(ArcherPrefab, FindObjectOfType<TerrainGeneration>().GetCenter(), Quaternion.identity);
            workerObj.GetComponent<NavMeshAgent>().enabled = true;
            rm.RemoveResource(Resource.Food, 30);
            rm.RemoveResource(Resource.Wood, 10);
        } 
    }
}
