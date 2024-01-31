using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickIndicator : MonoBehaviour
{
    [SerializeField] private Canvas ArrowCanvas;

    private void OnEnable()
    {
        // set event camera
        ArrowCanvas = GetComponent<Canvas>();
        ArrowCanvas.worldCamera = Camera.current;


    }

    private void Update()
    {

    }

    public void DisableArrowCanvas()
    {
        ArrowCanvas.gameObject.SetActive(false);
    }
}
