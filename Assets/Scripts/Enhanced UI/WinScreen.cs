using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WinScreen : MonoBehaviour
{
    [SerializeField] private GameObject WinPanel;
    [SerializeField] private TMP_Text WinText;
    [SerializeField] private int MaxWavesToWin = 5;
    [SerializeField] private int MinPopulationToWin = 10;
    [SerializeField] private int ResourceSumToWin = 250;
    [SerializeField] private bool CombatWin, PopWin, ResWin;

    private void Start()
    {
        CombatWin = false;
        PopWin = false;
        ResWin = false;
    }

    private void Update()
    {
        if(!CombatWin && FindObjectOfType<EnemyManager>().GetWaveCounter() >= MaxWavesToWin && FindObjectsOfType<Enemy>().Length == 0)
        {
            CombatWin = true;
            WinText.text = $"You have defeated {MaxWavesToWin} waves of enemies and defended your village for a long time! You have achieved a defensive victory!";
            WinPanel.SetActive(true);
        }
        else if(!PopWin && FindObjectsOfType<Worker>().Length + FindObjectsOfType<Archer>().Length >= MinPopulationToWin)
        {
            PopWin = true;
            WinText.text = $"You have grown your village to a size of {MinPopulationToWin} people! You have achieved an expansion victory!";
            WinPanel.SetActive(true);
        }
        else if(!ResWin && FindObjectOfType<ResourceManager>().GetResourceStockpile(Resource.Food) + FindObjectOfType<ResourceManager>().GetResourceStockpile(Resource.Wood) >= ResourceSumToWin)
        {
            ResWin = true;
            WinText.text = $"You have built up a surplus of {ResourceSumToWin} resources, and are well equipped for the future! You have achieved an economic victory!";
            WinPanel.SetActive(true);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Level Scene");
    }

    public void ContinueGame()
    {
        WinPanel.SetActive(false);
    }
}
