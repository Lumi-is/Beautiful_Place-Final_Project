using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public bool ignoreCameraMovement;
    [SerializeField] Transform yOrientation;
    public Transform listeningOrientation;

    public float GlobalSensitivity;

    float SensitivityX;
    float SensitivityY;

    float MousePosX;
    float MousePosY;

    float xRotation;
    float yRotation;

    void Update()
    {
        if(ignoreCameraMovement) return;
        
        SensitivityX = GlobalSensitivity;
        SensitivityY = GlobalSensitivity;
    
        MousePosX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * SensitivityX;
        MousePosY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * SensitivityY;

        yRotation += MousePosX;
        xRotation -= MousePosY;
        xRotation = Mathf.Clamp(xRotation,-72f,72f);

        transform.rotation = Quaternion.Euler(xRotation,yRotation,0);
        yOrientation.rotation = Quaternion.Euler(0,yRotation,0);
    }

    public void LookAtNPC(Transform npc)
    {
        ignoreCameraMovement = true;
        listeningOrientation.LookAt(npc.position+new Vector3(0,1f,0));
        transform.rotation = Quaternion.Slerp(transform.rotation, listeningOrientation.rotation, Time.deltaTime);
    }
}
