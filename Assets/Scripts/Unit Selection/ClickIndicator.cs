using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickIndicator : MonoBehaviour
{
    [SerializeField] private Canvas ArrowCanvas;
    // [SerializeField] private MeshRenderer[] Renderers;

    private void OnEnable()
    {
        // set event camera
        ArrowCanvas = GetComponentInChildren<Canvas>();
        ArrowCanvas.worldCamera = Camera.current;
    }

    public void DisableArrowCanvas()
    {
        Destroy(this.gameObject);
        // ArrowCanvas.gameObject.SetActive(false);
    }
}
