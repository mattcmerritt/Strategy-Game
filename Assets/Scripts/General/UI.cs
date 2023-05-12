using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    [SerializeField] private TMP_Text WoodStockpile, FoodStockpile;
    [SerializeField] private ResourceManager ResourceManager;

    [SerializeField] private TMP_Text UnitTitle, UnitDetails;
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
            // House h = selected.GetComponent<House>();
            // ArcheryRange ar = selected.GetComponent<ArcheryRange>();

            if (w != null)
            {
                UnitTitle.text = "Worker";
                UnitDetails.text = $"Health: {w.GetHealthString()}\nResources: {w.GetResourcesString()}";
            }
            else if (a != null)
            {
                UnitTitle.text = "Archer";
                UnitDetails.text = $"Health: {a.GetHealthString()}";
            }
            else
            {
                UnitTitle.text = "Nothing Selected";
                UnitDetails.text = $"Select a unit or building.";
            }
        }
    }
}
