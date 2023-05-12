using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitSelector : MonoBehaviour
{
    [SerializeField] private Material SelectedMaterial, PreviousMaterial;
    [SerializeField] private GameObject CurrentSelectedObject;
    [SerializeField] private LayerMask PlayerLayer;

    private void Update()
    {
        // Worker behavior
        if(CurrentSelectedObject != null && CurrentSelectedObject.GetComponent<Worker>() != null)
        {
            Worker worker = CurrentSelectedObject.GetComponent<Worker>();
            NavMeshAgent agent = CurrentSelectedObject.GetComponent<NavMeshAgent>();
            LineRenderer line = CurrentSelectedObject.GetComponent<LineRenderer>();

            // draw path line
            Vector3[] lineWaypoints = agent.path.corners;
            if(lineWaypoints != null && lineWaypoints.Length > 1)
            {
                line.positionCount = lineWaypoints.Length;
                for (int i = 0; i < lineWaypoints.Length; i++)
                {
                    line.SetPosition(i, lineWaypoints[i]);
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Debug.Log("ray.origin " + ray.origin + " ray.direction " + ray.direction);
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
                        Vector3 newHome = new Vector3(hit.point.x, worker.transform.position.y, hit.point.z);
                        worker.SetHomePosition(newHome);
                        agent.SetDestination(newHome);
                        // Debug.Log(newHome);
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("here");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.collider.gameObject.name);
                if(hit.collider.gameObject.GetComponent<Worker>() != null)
                {
                    Select(hit.collider.gameObject);
                }
                else
                {
                    Select(null);
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
        
        if(CurrentSelectedObject != null)
        {
            PreviousMaterial = CurrentSelectedObject.GetComponent<MeshRenderer>().material;
            CurrentSelectedObject.GetComponent<MeshRenderer>().material = SelectedMaterial;
        }
    }
}
