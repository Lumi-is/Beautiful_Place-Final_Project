using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public bool OverlayOpen;
    [SerializeField] Transform orientation;

    public float GlobalSensitivity;

    float SensitivityX;
    float SensitivityY;

    float MousePosX;
    float MousePosY;

    float xRotation;
    float yRotation;

    void Update()
    {
        if(!OverlayOpen)
        {
            SensitivityX = GlobalSensitivity;
            SensitivityY = GlobalSensitivity;
        
            MousePosX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * SensitivityX;
            MousePosY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * SensitivityY;

            yRotation += MousePosX;
            xRotation -= MousePosY;
            xRotation = Mathf.Clamp(xRotation,-72f,72f);

            transform.rotation = Quaternion.Euler(xRotation,yRotation,0);
            orientation.rotation = Quaternion.Euler(0,yRotation,0);
        }
    }
}
