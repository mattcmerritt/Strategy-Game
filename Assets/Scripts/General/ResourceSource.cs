using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSource : MonoBehaviour
{
    [SerializeField] private int ResourcesLeft = 2;
    [SerializeField] private Resource Type;

    public void TakeResource()
    {
        ResourcesLeft--;

        if (ResourcesLeft <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public bool IsDepleted()
    {
        return ResourcesLeft <= 0;
    }

    public Resource GetResourceType()
    {
        return Type;
    }
}
