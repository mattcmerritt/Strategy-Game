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
        else if(other.gameObject.GetComponent<Archer>() != null)
        {
            other.gameObject.GetComponent<Archer>().TakeDamage();
        }
        else if(other.gameObject.GetComponent<Enemy>() != null)
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage();
        }
        Destroy(this.gameObject);
    }
}
