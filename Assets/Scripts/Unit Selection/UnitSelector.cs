using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitSelector : MonoBehaviour
{
    [SerializeField] private Material SelectedMaterial, PreviousMaterial;
    [SerializeField] private GameObject CurrentSelectedObject;

    private void Update()
    {
        // Worker behavior
        if(CurrentSelectedObject != null && CurrentSelectedObject.GetComponent<Worker>() != null)
        {
            Worker worker = CurrentSelectedObject.GetComponent<Worker>();
            NavMeshAgent agent = CurrentSelectedObject.GetComponent<NavMeshAgent>();

            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Debug.Log("ray.origin " + ray.origin + " ray.direction " + ray.direction);
                Debug.DrawRay(ray.origin,ray.direction,Color.red);
                if (Physics.Raycast(ray, out hit))
                {
                    // If a resource was clicked, start gathering resources
                    if(hit.collider.gameObject.GetComponent<ResourceSource>() != null)
                    {
                        // Set home to nearest building position so that we drop off the resources
                        WalkToResourceState assignedTask = new WalkToResourceState(hit.collider.gameObject.GetComponent<ResourceSource>());
                        worker.ChangeState(assignedTask);
                    }
                    // If nothing interesting was hit, move to position clicked
                    else
                    {
                        worker.SetHomePosition(hit.point);
                        agent.SetDestination(hit.point);
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if(hit.collider.gameObject.GetComponent<Worker>() != null)
                {
                    Select(hit.collider.gameObject);
                }
                else
                {
                    CurrentSelectedObject = null;
                }
            }
        }
    }

    private void Select(GameObject selectedObject)
    {
        if(CurrentSelectedObject != null)
        {
            CurrentSelectedObject.GetComponent<MeshRenderer>().material = PreviousMaterial;
        }

        CurrentSelectedObject = selectedObject;
        PreviousMaterial = CurrentSelectedObject.GetComponent<MeshRenderer>().material;
        CurrentSelectedObject.GetComponent<MeshRenderer>().material = SelectedMaterial;
    }

    
}
