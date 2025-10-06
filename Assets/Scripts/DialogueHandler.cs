using System.Collections.Generic;
using System.IO.Pipes;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Interactor))]
public class DialogueHandler : MonoBehaviour
{

    [Header("Setup")]
    [Tooltip("The text box where dialogue should be displayed.")]
    [SerializeField] TMP_Text dialogueBox;

    [Tooltip("Text box for dialogue option A.")]
    [SerializeField] TMP_Text dialogueOptionA;

    [Tooltip("Text box for dialogue option B.")]
    [SerializeField] TMP_Text dialogueOptionB;

    [Header("Attributes")]
    [Tooltip("How long by default a text should be shown for.")]
    [SerializeField] float dialogueLingering;

    public bool dialogueOnScreen;

    void Start()
    {
        EmptyDialogue();
    }

    public void DisplayObjectDialogue(string dialogue)
    {
        dialogueOnScreen = true;
        dialogueBox.text = dialogue;
        Invoke("EmptyDialogue",dialogueLingering);
    }

    public void DisplayNPCDialogue(string dialogue, string dialogueOptionAtext, string dialogueOptionBtext)
    {
        dialogueOnScreen = true;
        dialogueBox.text = dialogue;
        dialogueOptionA.text = dialogueOptionAtext;
        dialogueOptionB.text = dialogueOptionBtext;
    }

    public void EmptyDialogue()
    {
        dialogueBox.text = "";
        dialogueOnScreen = false;
    }
}
