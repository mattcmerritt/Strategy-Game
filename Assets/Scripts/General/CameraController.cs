using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CameraController : MonoBehaviour
{
    [SerializeField, Range(1, 100)] private int CameraSpeed;
    [SerializeField] private float EdgepanRangeX = 0.05f, EdgepanRangeY = 0.1f;
    [SerializeField] private float MinX = 0f, MaxX = 100f, MinZ = -20f, MaxZ = 80f;
    [SerializeField] private bool EdgepanningEnabled;
    [SerializeField] private TMP_Text EdgepanningButtonText;

    private void Start()
    {
        EdgepanningEnabled = true;
    }

    private void Update()
    {
        Vector3 movedPosition = transform.position;

        // Move with WASD / arrow keys
        Vector3 MoveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        movedPosition += MoveDirection.normalized * Time.deltaTime * CameraSpeed;

        // Edge panning
        // Debug.Log($"Mouse position: {Input.mousePosition}");
        if(EdgepanningEnabled)
        {
            // horizontal
            if(Input.mousePosition.x < Screen.width * EdgepanRangeX)
            {
                movedPosition += Vector3.left * Time.deltaTime * CameraSpeed;
            }
            else if(Input.mousePosition.x > Screen.width - (Screen.width * EdgepanRangeX))
            {
                movedPosition += Vector3.right * Time.deltaTime * CameraSpeed;
            }

            // vertical
            if(Input.mousePosition.y < Screen.height * EdgepanRangeY)
            {
                movedPosition += Vector3.back * Time.deltaTime * CameraSpeed;
            }
            else if(Input.mousePosition.y > Screen.height - (Screen.height * EdgepanRangeY))
            {
                movedPosition += Vector3.forward * Time.deltaTime * CameraSpeed;
            }
        }

        // camera locking to map so you can't lose the play area
        if(movedPosition.x < MinX)
        {
            movedPosition.x = MinX;
        }
        else if(movedPosition.x > MaxX)
        {
            movedPosition.x = MaxX;
        }
        if(movedPosition.z < MinZ)
        {
            movedPosition.z = MinZ;
        }
        else if(movedPosition.z > MaxZ)
        {
            movedPosition.z = MaxZ;
        }

        transform.position = movedPosition;
    }

    public void ToggleEdgepanning()
    {
        EdgepanningEnabled = !EdgepanningEnabled;
        EdgepanningButtonText.text = "Edgepanning: ";
        if(EdgepanningEnabled)
        {
            EdgepanningButtonText.text += "ON";
        }
        else
        {
            EdgepanningButtonText.text += "OFF";
        }
    }
}
