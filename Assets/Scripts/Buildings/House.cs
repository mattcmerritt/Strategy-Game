using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class House : Building
{
    [SerializeField] private GameObject WorkerPrefab;

    public GameObject GetWorkerPrefab()
    {
        return WorkerPrefab;
    }

    public void CreateWorker()
    {
        ResourceManager rm = FindObjectOfType<ResourceManager>();
        if (rm.GetResourceStockpile(Resource.Food) >= 30)
        {
            GameObject workerObj = Instantiate(WorkerPrefab, FindObjectOfType<TerrainGeneration>().GetCenter(), Quaternion.identity);
            workerObj.GetComponent<NavMeshAgent>().enabled = true;
            rm.RemoveResource(Resource.Food, 30);
        }
    }
}
