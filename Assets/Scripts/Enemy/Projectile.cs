using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private void Update()
    {
        if(transform.position.y < -4f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Worker>() != null)
        {
            other.gameObject.GetComponent<Worker>().TakeDamage();
        }
        Destroy(this.gameObject);
    }
}
