using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private TMP_Text WoodStockpile, FoodStockpile;
    [SerializeField] private ResourceManager ResourceManager;

    [SerializeField] private TMP_Text UnitTitle, UnitDetails;
    [SerializeField] private GameObject CreateButton;
    [SerializeField] private UnitSelector UnitSelector;

    private void Update()
    {
        WoodStockpile.text = $"Wood Stockpile: {ResourceManager.GetResourceStockpile(Resource.Wood)}";
        FoodStockpile.text = $"Food Stockpile: {ResourceManager.GetResourceStockpile(Resource.Food)}";

        GameObject selected = UnitSelector.GetSelectedObject();
        if (selected != null)
        {
            Worker w = selected.GetComponent<Worker>();
            Archer a = selected.GetComponent<Archer>();
            House h = selected.GetComponent<House>();
            ArcheryRange ar = selected.GetComponent<ArcheryRange>();

            if (w != null)
            {
                UnitTitle.text = "Worker";
                UnitDetails.text = $"Health: {w.GetHealthString()}\nResources: {w.GetResourcesString()}\nCan be sent to the archery range to change classes to an archer.";
            }
            else if (a != null)
            {
                UnitTitle.text = "Archer";
                UnitDetails.text = $"Health: {a.GetHealthString()}\nCan be sent to the house to change classes to a worker.";
            }
            else if (h != null)
            {
                UnitTitle.text = "House";
                UnitDetails.text = "Can create a worker for 30 food. Press space to create.";

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    h.CreateWorker();
                }
            }
            else if (ar != null)
            {
                UnitTitle.text = "Archery Range";
                UnitDetails.text = "Can create an archer for 30 food and 10 wood. Press space to create.";
                
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    ar.CreateArcher();
                }
            }
        }
        else
        {
            UnitTitle.text = "Nothing Selected";
            UnitDetails.text = $"Select a unit or building.";
        }
    }
}
