using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GameStageManager))]
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

    [Tooltip("Text box for dialogue option C.")]
    [SerializeField] TMP_Text dialogueOptionC;

    [Tooltip("Dialogue Option Highlighter.")]
    [SerializeField] GameObject dialogueHighlighter;

    [Header("Attributes")]
    [Tooltip("How many seconds dialogue is shown for per character. For example if this was set to 1, a word with 4 letters would linger on screen for 4 seconds.")]
    public float timeDialogueShownPerChar;
    [Tooltip("Default extra amount of seconds dialogue is shown for.")]
    public float extraDialogueLingering;

    public static bool isStaticDialogueOnScreen;
    public static bool isDynamicDialogueOnScreen;
    public bool isDialogueFinished = true;

    public int howManyOptionsOnScreen;

    public bool playerChoiceMade;
    public char playerChoice;

    public Color playerTextColor;
    public Color lucyTextColor;
    public Color russelTextColor;
    public Color defaultNpcTextColor;

    void Start()
    {
        EmptyDialogue();
    }

    void Update()
    {
        if(howManyOptionsOnScreen > 0)
        {
            dialogueHighlighter.SetActive(true);
            RectTransform rectTransform = dialogueHighlighter.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(-60, rectTransform.anchoredPosition.y + Input.mouseScrollDelta.y * 100);
            if(rectTransform.anchoredPosition.y > howManyOptionsOnScreen*100-10)
            {
                rectTransform.anchoredPosition = new Vector2(-60, 90);
            }
            if(rectTransform.anchoredPosition.y < 90)
            {
                rectTransform.anchoredPosition = new Vector2(-60, howManyOptionsOnScreen*100-10);
            }

            if(Input.GetKeyDown(KeyCode.Space))
            {
                playerChoiceMade = true;
                if(rectTransform.anchoredPosition.y == 90)
                {
                    playerChoice = 'a';
                } else if(rectTransform.anchoredPosition.y == 190)
                {
                    playerChoice = 'b';
                }else if(rectTransform.anchoredPosition.y == 290)
                {
                    playerChoice = 'c';
                }
            }
        } else
        {
            dialogueHighlighter.SetActive(false);
        }
    }

    public void DisplayObjectDialogue(string dialogue)
    {
        isDynamicDialogueOnScreen = true;
        dialogueBox.text = dialogue;
        Invoke(nameof(EmptyDialogue), extraDialogueLingering);
    }

    public void DisplayNPCDialogue(string dialogue, string npcName, bool shouldPlayerBeStatic)
    {
        if(shouldPlayerBeStatic)
        {
            isStaticDialogueOnScreen = true;
        } else
        {
            isDynamicDialogueOnScreen = true;
        }

        
        dialogueBox.text = npcName+": "+dialogue;

        if(npcName == "Lucy")
        {
            dialogueBox.color = lucyTextColor;
        } else if (npcName == "Russel")
        {
            dialogueBox.color = russelTextColor;
        } else
        {
            dialogueBox.color = defaultNpcTextColor;
        }
    }

    public void DisplayPlayerDialogue(string dialogue)
    {
        isDynamicDialogueOnScreen = true;
        dialogueBox.text = "Player: "+dialogue;

        dialogueBox.color = playerTextColor;
    }

    public void DisplayPlayerOptions(string dialogueOptionAtext)
    {
        isStaticDialogueOnScreen = true;
        howManyOptionsOnScreen = 1;
        dialogueOptionA.text = dialogueOptionAtext;

        dialogueOptionA.color = playerTextColor;
    }

    public void DisplayPlayerOptions(string dialogueOptionAtext, string dialogueOptionBtext)
    {
        isStaticDialogueOnScreen = true;
        howManyOptionsOnScreen = 2;
        dialogueOptionA.text = dialogueOptionAtext;
        dialogueOptionB.text = dialogueOptionBtext;

        dialogueOptionA.color = playerTextColor;
        dialogueOptionB.color = playerTextColor;
    }

    public void DisplayPlayerOptions(string dialogueOptionAtext, string dialogueOptionBtext, string dialogueOptionCtext)
    {
        isStaticDialogueOnScreen = true;
        howManyOptionsOnScreen = 3;
        dialogueOptionA.text = dialogueOptionAtext;
        dialogueOptionB.text = dialogueOptionBtext;
        dialogueOptionC.text = dialogueOptionCtext;

        dialogueOptionA.color = playerTextColor;
        dialogueOptionB.color = playerTextColor;
        dialogueOptionC.color = playerTextColor;
    }

    public void EmptyDialogue()
    {
        dialogueBox.text = "";
        dialogueOptionA.text = "";
        dialogueOptionB.text = "";
        dialogueOptionC.text = "";
        isStaticDialogueOnScreen = false;
        isDynamicDialogueOnScreen = false;
        howManyOptionsOnScreen = 0;

        dialogueBox.color = Color.white;
        dialogueOptionA.color = Color.white;
        dialogueOptionB.color = Color.white;
        dialogueOptionC.color = Color.white;
    }

    public IEnumerator PlayMainDialogue(List<DialogueState> dialogueStates)
    {
        isDialogueFinished = false;
        for(int i = 0; i < dialogueStates.Count; i++)
        {
            EmptyDialogue();

            foreach(string dialogue in dialogueStates[i].mainDialogue)
            {
                DisplayNPCDialogue(dialogue, dialogueStates[i].npcName, dialogueStates[i].shouldPlayerBeStatic);
                yield return new WaitForSeconds(dialogue.Length*timeDialogueShownPerChar+dialogueStates[i].textLingerDuration+extraDialogueLingering);
                EmptyDialogue();
            }

            if(dialogueStates[i].mainPlayerOptions != null)
            {
                if(dialogueStates[i].mainPlayerOptions.Count == 1) {
                    DisplayPlayerOptions(dialogueStates[i].mainPlayerOptions[0]);
                } else if (dialogueStates[i].mainPlayerOptions.Count == 2) {
                    DisplayPlayerOptions(dialogueStates[i].mainPlayerOptions[0],dialogueStates[i].mainPlayerOptions[1]);
                } else if (dialogueStates[i].mainPlayerOptions.Count == 3) {
                    DisplayPlayerOptions(dialogueStates[i].mainPlayerOptions[0],dialogueStates[i].mainPlayerOptions[1],dialogueStates[i].mainPlayerOptions[2]);
                }

                //Make event which turns playerChoiceMade to be true when choosing any of the options (WIP)
                yield return new WaitUntil(() => playerChoiceMade); playerChoiceMade = false; EmptyDialogue();

                List<string> mainAnswer;
                if(playerChoice == 'a')
                {
                    mainAnswer = dialogueStates[i].mainAnswerA;
                } else if(playerChoice == 'b')
                {
                    mainAnswer = dialogueStates[i].mainAnswerB;
                } else 
                {
                    mainAnswer = dialogueStates[i].mainAnswerC;
                }

                foreach(string dialogue in mainAnswer)
                {
                    DisplayPlayerDialogue(dialogue);
                    yield return new WaitForSeconds(dialogue.Length*timeDialogueShownPerChar+dialogueStates[i].textLingerDuration+extraDialogueLingering);
                    EmptyDialogue();
                }   
            }
        }
        isDialogueFinished = true;   
    }
}


