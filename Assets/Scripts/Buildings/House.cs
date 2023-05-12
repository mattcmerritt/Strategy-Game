using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    [SerializeField] private GameObject WorkerPrefab;

    public GameObject GetWorkerPrefab()
    {
        return WorkerPrefab;
    }
}
