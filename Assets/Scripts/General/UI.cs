using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private TMP_Text WoodStockpile, FoodStockpile, EnemiesLabel;
    [SerializeField] private ResourceManager ResourceManager;

    [SerializeField] private TMP_Text UnitTitle, UnitDetails;
    [SerializeField] private GameObject CreateButton;
    [SerializeField] private UnitSelector UnitSelector;

    // Enemy wave spawning UI elements
    [SerializeField] private TMP_Text TimerText;

    // Setting up singleton
    public static UI Instance;

    private void Start()
    {
        Instance = this;
    }

    private void Update()
    {
        WoodStockpile.text = $"{ResourceManager.GetResourceStockpile(Resource.Wood)}";
        FoodStockpile.text = $"{ResourceManager.GetResourceStockpile(Resource.Food)}";

        // fetch the current number of enemies
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        EnemiesLabel.text = $"Active: {enemies.Length}";

        // Timer display
        float waveTimer = EnemyManager.Instance.GetTimeForWave();
        int minutes = Mathf.Clamp(((int) waveTimer / 60), 0, ((int) waveTimer / 60));
        int seconds = Mathf.Clamp(((int) waveTimer % 60), 0, ((int) waveTimer % 60));
        TimerText.text = $"Reinforces in: {string.Format("{0:0}:{1:00}", minutes, seconds)}";

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
