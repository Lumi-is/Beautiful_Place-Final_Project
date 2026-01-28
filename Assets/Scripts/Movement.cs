using UnityEngine;
using System;

public class Movement : MonoBehaviour
{
    public bool OverlayOpen;
    [SerializeField] Rigidbody rb; 

    [SerializeField] Transform orientation;

    private Vector3 moveDirection;

    public float WalkSpeed;
    public float RunSpeed;

    private float HorizontalInput;
    private float VerticalInput;

    public bool Crouching;

    // Update is called once per frame
    void Update()
    {
        if(!OverlayOpen)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            //Get all input information from client
            GetInputs();

            //Moves the player in regard to transform rotation "orientation" 
            MoveAction();
        } else {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }

    void GetInputs()
    {
        //Values ranging from -1 to 1
        HorizontalInput = Input.GetAxis("Horizontal");
        VerticalInput = Input.GetAxis("Vertical");
    

        if(Input.GetKey(KeyCode.LeftControl))
        {
            Crouching = true;
        } else {
            Crouching = false;
        }
    }

    void MoveAction()
    {

        moveDirection = orientation.forward * VerticalInput + orientation.right * HorizontalInput;

        if(DialogueHandler.isStaticDialogueOnScreen)
        {
            rb.linearVelocity = new Vector3(0,rb.linearVelocity.y,0); 
            return;
        } 
        if (Input.GetKey(KeyCode.LeftShift) && !Crouching)
        {
            if (Input.GetAxisRaw("Vertical") == -1 && Input.GetAxisRaw("Horizontal") == 0)
            {
                rb.linearVelocity = new Vector3(moveDirection.x * RunSpeed * 0.8f, rb.linearVelocity.y, moveDirection.z * RunSpeed * 0.8f);
            }
            else if (Math.Abs(Input.GetAxisRaw("Horizontal")) != Math.Abs(Input.GetAxisRaw("Vertical")))
            {
                rb.linearVelocity = new Vector3(moveDirection.x * RunSpeed, rb.linearVelocity.y, moveDirection.z * RunSpeed);
            }
            else
            {
                rb.linearVelocity = new Vector3(moveDirection.x * RunSpeed * 0.5f, rb.linearVelocity.y, moveDirection.z * RunSpeed * 0.5f);
            }


        }
        else
        {
            if (Input.GetAxisRaw("Horizontal") != Input.GetAxisRaw("Vertical") && Input.GetAxisRaw("Horizontal") != -1 * Input.GetAxisRaw("Vertical"))
            {
                rb.linearVelocity = new Vector3(moveDirection.x * WalkSpeed, rb.linearVelocity.y, moveDirection.z * WalkSpeed);
            }
            else
            {
                rb.linearVelocity = new Vector3(moveDirection.x * WalkSpeed * 0.7f, rb.linearVelocity.y, moveDirection.z * WalkSpeed * 0.7f);
            }
        }
    }
}
