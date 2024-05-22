using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoviePlayer : MonoBehaviour
{
    public float sensitivity = 2.0f;
    public float maxYAngle = 80.0f;


    private Vector2 currentRotation;

    public float zoomSpeed = 10;
    public bool isCursorVisible;

    [SerializeField] new Camera camera;

    private void Start()
    {
        currentRotation = new Vector2(transform.eulerAngles.y, transform.eulerAngles.x);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            isCursorVisible = !isCursorVisible;
            Cursor.visible = isCursorVisible;
            Cursor.lockState = isCursorVisible ? CursorLockMode.None : CursorLockMode.Locked;
        }

        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        currentRotation.x += mouseX;
        currentRotation.y -= mouseY;
        currentRotation.y = Mathf.Clamp(currentRotation.y, -maxYAngle, maxYAngle);

        transform.rotation = Quaternion.Euler(currentRotation.y, currentRotation.x, 0);

        if (camera.orthographic)
        {
            camera.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        }
        else
        {
            camera.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        }

    }
}