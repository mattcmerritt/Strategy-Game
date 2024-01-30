using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRandomly : MonoBehaviour
{
    public Vector3 start;
    public float amp;

    void Start()
    {
        start = transform.position;
    }

    void Update()
    {
        transform.position = new Vector3(0, Mathf.Sin(Time.time), 0) + start;
    }
}
