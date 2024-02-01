using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manual : MonoBehaviour
{
    [SerializeField] private TMP_Text Title, Body;
    [SerializeField] private Image Picture;

    public void LoadManualTitle(string t)
    {
        Title.text = t;
    }

    public void LoadManualBody(string b)
    {
        Body.text = b;
    }

    public void LoadManualImage(Sprite s)
    {
        Picture.sprite = s;
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
