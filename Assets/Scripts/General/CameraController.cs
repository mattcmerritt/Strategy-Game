using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField, Range(1, 100)] private int CameraSpeed;
    [SerializeField] private float EdgepanRange = 0.1f;

    private void Update()
    {
        // Move with WASD / arrow keys
        Vector3 MoveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        transform.position += MoveDirection.normalized * Time.deltaTime * CameraSpeed;

        // Edge panning
        // Debug.Log($"Mouse position: {Input.mousePosition}");

        // horizontal
        if(Input.mousePosition.x < Screen.width * EdgepanRange)
        {
            transform.position += Vector3.left * Time.deltaTime * CameraSpeed;
        }
        else if(Input.mousePosition.x > Screen.width - (Screen.width * EdgepanRange))
        {
            transform.position += Vector3.right * Time.deltaTime * CameraSpeed;
        }

        // vertical
        if(Input.mousePosition.y < Screen.height * EdgepanRange)
        {
            transform.position += Vector3.back * Time.deltaTime * CameraSpeed;
        }
        else if(Input.mousePosition.y > Screen.height - (Screen.height * EdgepanRange))
        {
            transform.position += Vector3.forward * Time.deltaTime * CameraSpeed;
        }
    }
}
