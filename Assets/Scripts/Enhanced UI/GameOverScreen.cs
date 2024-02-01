using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private int UnitProductionCost = 30; 

    private void Update()
    {
        if(FindObjectsOfType<Worker>().Length == 0 && FindObjectsOfType<Archer>().Length == 0 && FindObjectOfType<ResourceManager>().GetResourceStockpile(Resource.Food) < UnitProductionCost)
        {
            GameOverPanel.SetActive(true);
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene("Level Scene");
    }
}
