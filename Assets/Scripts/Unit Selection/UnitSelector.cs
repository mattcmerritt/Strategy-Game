using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitSelector : MonoBehaviour
{
    [SerializeField] private Material SelectedMaterial, PreviousMaterial;
    [SerializeField] private GameObject CurrentSelectedObject;
    [SerializeField] private LayerMask DetectLayer;

    [SerializeField] public static UnitSelector Instance;

    [SerializeField] private GameObject CursorPrefab;

    private void OnEnable()
    {
        Instance = this;
    }

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
                // can only perform actions if not garrisoned
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~DetectLayer) && !worker.CheckIsSafe())
                {
                    // If a resource was clicked, start gathering resources
                    if(hit.collider.gameObject.GetComponent<ResourceSource>() != null)
                    {
                        // Set home to nearest building position so that we drop off the resources
                        WalkToResourceState assignedTask = new WalkToResourceState(hit.collider.gameObject.GetComponent<ResourceSource>());
                        worker.ChangeState(assignedTask);
                    }
                    // Turn worker into an archer if the range was clicked
                    else if(hit.collider.gameObject.GetComponent<ArcheryRange>() != null)
                    {
                        // TODO: add cost
                        WalkToRangeState assignedTask = new WalkToRangeState(hit.collider.gameObject.GetComponent<ArcheryRange>());
                        worker.ChangeState(assignedTask);
                    }
                    // Garrison in tower if clicked
                    else if (hit.collider.gameObject.GetComponent<Tower>() != null)
                    {
                        WalkToTowerState assignedTask = new WalkToTowerState(hit.collider.gameObject.GetComponent<Tower>());
                        worker.ChangeState(assignedTask);
                    }
                    // If nothing interesting was hit, move to position clicked if not garrisoned
                    else
                    {
                        Vector3 newHome = new Vector3(hit.point.x, worker.transform.position.y, hit.point.z);
                        worker.SetHomePosition(newHome);
                        agent.SetDestination(newHome);
                        // Debug.Log(newHome);
                    }

                    // Regardless of what was done prior, play the cursor animation there
                    Instantiate(CursorPrefab, new Vector3(hit.point.x, worker.transform.position.y, hit.point.z), Quaternion.identity);
                }
            }
        }

        // Archer behavior
        if(CurrentSelectedObject != null && CurrentSelectedObject.GetComponent<Archer>() != null)
        {
            Archer archer = CurrentSelectedObject.GetComponent<Archer>();
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
                // can only perform actions if not garrisoned
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~DetectLayer) && !archer.CheckIsSafe())
                {
                    // Turn archer into an worker if the house was clicked
                    if(hit.collider.gameObject.GetComponent<House>() != null)
                    {
                        // TODO: add cost
                        WalkToHouseState assignedTask = new WalkToHouseState(hit.collider.gameObject.GetComponent<House>());
                        archer.ChangeState(assignedTask);
                    }
                    // Attack a specific enemy that was clicked on
                    else if(hit.collider.gameObject.GetComponent<Enemy>() != null)
                    {
                        archer.SetTarget(hit.collider.gameObject.GetComponent<Enemy>());
                        ArcherAttackState assignedTask = new ArcherAttackState();
                        archer.ChangeState(assignedTask);
                    }
                    // Garrison in tower if clicked
                    else if (hit.collider.gameObject.GetComponent<Tower>() != null)
                    {
                        WalkToTowerState assignedTask = new WalkToTowerState(hit.collider.gameObject.GetComponent<Tower>());
                        archer.ChangeState(assignedTask);
                    }
                    // If nothing interesting was hit, move to position clicked
                    else 
                    {
                        Vector3 newPos = new Vector3(hit.point.x, archer.transform.position.y, hit.point.z);
                        archer.SetNewPosition(newPos);
                        archer.ChangeState(new ArcherRepositionState());
                    }

                    // Regardless of what was done prior, play the cursor animation there
                    Instantiate(CursorPrefab, new Vector3(hit.point.x, archer.transform.position.y, hit.point.z), Quaternion.identity);
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            UI ui = FindObjectOfType<UI>();

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~DetectLayer))
            {
                Debug.Log(hit.collider.gameObject.name);
                if(hit.collider.gameObject.GetComponent<Worker>() != null)
                {
                    Select(hit.collider.gameObject);
                }
                else if(hit.collider.gameObject.GetComponent<Archer>() != null)
                {
                    Select(hit.collider.gameObject);
                }
                else if (hit.collider.gameObject.GetComponent<House>() != null)
                {
                    Select(hit.collider.gameObject);
                }
                else if (hit.collider.gameObject.GetComponent<ArcheryRange>() != null)
                {
                    Select(hit.collider.gameObject);
                }
                else if (hit.collider.gameObject.GetComponent<Tower>() != null)
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
        if(CurrentSelectedObject != null && (CurrentSelectedObject.GetComponent<Worker>() != null || CurrentSelectedObject.GetComponent<Archer>() != null))
        {
            CurrentSelectedObject.GetComponent<MeshRenderer>().material = PreviousMaterial;
        }

        CurrentSelectedObject = selectedObject;
        
        if(CurrentSelectedObject != null && (CurrentSelectedObject.GetComponent<Worker>() != null || CurrentSelectedObject.GetComponent<Archer>() != null))
        {
            PreviousMaterial = CurrentSelectedObject.GetComponent<MeshRenderer>().material;
            CurrentSelectedObject.GetComponent<MeshRenderer>().material = SelectedMaterial;
        }
    }

    public GameObject GetSelectedObject()
    {
        return CurrentSelectedObject;
    }
}
