using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceManager : MonoBehaviour
{
    [SerializeField] private int ResourceGoal, ResourcesCollected;

    [SerializeField] private TMP_Text Collected, Target;

    public void AddResources(int additional)
    {
        ResourcesCollected += additional;
        Collected.text = "" + ResourcesCollected;
    }

    public bool HasMetResourceGoal()
    {
        return ResourcesCollected >= ResourceGoal;
    }

    public void IncrementResourceGoal()
    {
        ResourceGoal += 5;
        Target.text = "" + ResourceGoal;
    }

    public void DeccrementResourceGoal()
    {
        ResourceGoal -= 5;
        Target.text = "" + ResourceGoal;
    }
}
