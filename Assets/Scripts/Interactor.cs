using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(DialogueHandler))]
public class Interactor : MonoBehaviour
{
    LayerMask interactableMask;
    LayerMask NPCMask;
    Camera mainCam;
    DialogueHandler dialogueHandler;


    public List<Grabbable> inventoryList;
    public Transform heldObject;
    public Transform holster;
    public int inventorySize;
    public int inventoryItemsSize;

    void Start()
    {
        interactableMask = LayerMask.GetMask("Interactable", "Observable", "Grabbable");
        NPCMask = LayerMask.GetMask("NPC");


        mainCam = GetComponent<Camera>();
        dialogueHandler = GetComponent<DialogueHandler>();
    }

    // See Order of Execution for Event Functions for information on FixedUpdate() and Update() related to physics queries
    void Update()
    {
        if (heldObject != null)
        {
            heldObject.position = holster.position;
            heldObject.rotation = holster.rotation;

        }
        Ray ray = mainCam.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2, 0));

        if (Physics.Raycast(ray, out RaycastHit hit, 3, interactableMask) && Input.GetKeyDown(KeyCode.E))
        {
            if (hit.collider.GetComponent<Observable>() != null && !dialogueHandler.dialogueOnScreen)
            {
                dialogueHandler.DisplayObjectDialogue(hit.collider.GetComponent<Observable>().dialogueToShow);
            }

            if (hit.collider.GetComponent<Grabbable>() != null)
            {
                Grabbable grabbable = hit.collider.GetComponent<Grabbable>();

                if (grabbable.itemSize + inventoryItemsSize <= inventorySize)
                {
                    inventoryList.Add(grabbable);
                    inventoryItemsSize += grabbable.itemSize;
                    grabbable.GetComponent<Collider>().isTrigger = true;
                    grabbable.GetComponent<Rigidbody>().isKinematic = true;

                    if (heldObject == null)
                    {
                        heldObject = grabbable.transform;
                    }
                    else
                    {
                        heldObject.GetComponent<Renderer>().enabled = false;
                        heldObject = grabbable.transform;
                    }
                    
                }
            }
            
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            heldObject.GetComponent<Collider>().isTrigger = false;
            heldObject.GetComponent<Rigidbody>().isKinematic = false;

            Grabbable grabbable = heldObject.GetComponent<Grabbable>();
            inventoryItemsSize -= grabbable.itemSize;
            inventoryList.Remove(grabbable);

            if (inventoryList.Count == 0)
            {
                heldObject = null;
            }
            else
            {
                heldObject = inventoryList[^1].transform;
                heldObject.GetComponent<Renderer>().enabled = true;
            }
            
        }

        //Debug
            Debug.DrawRay(ray.origin, ray.direction * 3, Color.blue);
    }
}

