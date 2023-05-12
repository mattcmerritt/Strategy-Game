using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public GameObject cubePrefab;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(cubePrefab, Vector3.zero, Quaternion.identity);
        Collider[] hits = Physics.OverlapBox(Vector3.zero, cubePrefab.transform.localScale / 2);

        Debug.Log(hits.Length);
    }
}
