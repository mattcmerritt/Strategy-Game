using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Tower : Building
{
    [SerializeField] private Vector3 TowerPosition;
    [SerializeField] private GameObject GarrisonedUnit;
    [SerializeField] private Vector3 GroundPosition;

    public GameObject GetGarrisonedUnit()
    {
        return GarrisonedUnit;
    }

    public void SetGarrisonedUnit(GameObject unit)
    {
        // prevent double garrisons
        if (GarrisonedUnit != null)
        {
            return;
        }

        GarrisonedUnit = unit;
        GroundPosition = unit.transform.position;

        // parenting the new unit to the tower and moving it to position
        GarrisonedUnit.transform.SetParent(transform);
        GarrisonedUnit.transform.localPosition = TowerPosition;

        // prevent character from sliding away
        Rigidbody rb = GarrisonedUnit.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // activate invincibility
        GarrisonedUnit.GetComponent<Agent>().SetSafe(true);

        // force unit to idle by disabling nav agent temporarily
        GarrisonedUnit.GetComponent<NavMeshAgent>().enabled = false;
    }

    public GameObject ReleaseGarrisonedUnit()
    {
        GameObject prev = GarrisonedUnit;
        GarrisonedUnit = null;

        if (prev != null)
        {
            prev.transform.SetParent(null);
            prev.transform.position = GroundPosition;

            // restore any agency to the unit
            prev.GetComponent<NavMeshAgent>().enabled = true;

            // deactivate invincibility
            prev.GetComponent<Agent>().SetSafe(false);
        }

        return prev;
    }
}
