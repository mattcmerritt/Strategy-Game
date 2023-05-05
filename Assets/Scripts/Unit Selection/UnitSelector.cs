using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelector : MonoBehaviour
{
    [SerializeField] private Material SelectedMaterial, PreviousMaterial;
    [SerializeField] private GameObject CurrentSelectedObject;

    private void Update()
    {
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
