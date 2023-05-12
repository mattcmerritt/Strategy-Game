using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField, Range(1, 10)] private int CameraSpeed;
    private void Update()
    {
        Vector3 MoveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        transform.position += MoveDirection.normalized * Time.deltaTime * CameraSpeed;
    }
}
