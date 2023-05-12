using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcheryRange : MonoBehaviour
{
    [SerializeField] private GameObject ArcherPrefab;

    public GameObject GetArcherPrefab()
    {
        return ArcherPrefab;
    }
}
