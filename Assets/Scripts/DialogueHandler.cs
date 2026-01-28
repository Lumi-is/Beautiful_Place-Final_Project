using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(GameStageManager))]
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

    public int howManyOptionsOnScreen;

    public bool playerChoiceMade;
    public char playerChoice;

    public Color playerTextColor;
    public Color lucyTextColor;
    public Color russelTextColor;
    public Color defaultNpcTextColor;

    public bool isDialogueFinished;
    private bool lineOver;

    [SerializeField] CameraMovement cameraMovement;
    private Transform talkingNPC;

    void Update()
    {
        if(isStaticDialogueOnScreen)
        {
            cameraMovement.LookAtNPC(talkingNPC);
        } else
        {
            cameraMovement.ignoreCameraMovement = false;
        }

        if(Input.GetKeyDown(KeyCode.G)) SkipDialogue();

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

    private void SkipDialogue()
    {
        lineOver = true;
    }

    public void DisplayObjectDialogue(string dialogue)
    {
        isDynamicDialogueOnScreen = true;
        dialogueBox.text = dialogue;
        Invoke(nameof(EmptyDialogue), extraDialogueLingering);
    }

    public void DisplayNPCDialogue(string dialogue, string npcName, bool shouldPlayerBeStatic)
    {
        talkingNPC = GetComponent<GameStageManager>().npcDictionary[npcName];
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

    public void DisplayPlayerOptions(string dialogueOptionAtext, bool shouldPlayerBeStatic)
    {
        if(shouldPlayerBeStatic)
        {
            isStaticDialogueOnScreen = true;
        } else
        {
            isDynamicDialogueOnScreen = true;
        }
        
        howManyOptionsOnScreen = 1;
        dialogueOptionA.text = dialogueOptionAtext;

        dialogueOptionA.color = playerTextColor;
    }

    public void DisplayPlayerOptions(string dialogueOptionAtext, string dialogueOptionBtext, bool shouldPlayerBeStatic)
    {
        if(shouldPlayerBeStatic)
        {
            isStaticDialogueOnScreen = true;
        } else
        {
            isDynamicDialogueOnScreen = true;
        }

        isStaticDialogueOnScreen = true;
        howManyOptionsOnScreen = 2;
        dialogueOptionA.text = dialogueOptionAtext;
        dialogueOptionB.text = dialogueOptionBtext;

        dialogueOptionA.color = playerTextColor;
        dialogueOptionB.color = playerTextColor;
    }

    public void DisplayPlayerOptions(string dialogueOptionAtext, string dialogueOptionBtext, string dialogueOptionCtext, bool shouldPlayerBeStatic)
    {
        if(shouldPlayerBeStatic)
        {
            isStaticDialogueOnScreen = true;
        } else
        {
            isDynamicDialogueOnScreen = true;
        }

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
        lineOver = false;
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
                StartCoroutine(LineTimer(dialogue,dialogueStates[i]));
                yield return new WaitUntil(()=> lineOver);
                EmptyDialogue();
            }

            if(dialogueStates[i].mainPlayerOptions.Count != 0)
            {
                if(dialogueStates[i].mainPlayerOptions.Count == 1) {
                    DisplayPlayerOptions(dialogueStates[i].mainPlayerOptions[0], dialogueStates[i].shouldPlayerBeStatic);
                } else if (dialogueStates[i].mainPlayerOptions.Count == 2) {
                    DisplayPlayerOptions(dialogueStates[i].mainPlayerOptions[0],dialogueStates[i].mainPlayerOptions[1], dialogueStates[i].shouldPlayerBeStatic);
                } else if (dialogueStates[i].mainPlayerOptions.Count == 3) {
                    DisplayPlayerOptions(dialogueStates[i].mainPlayerOptions[0],dialogueStates[i].mainPlayerOptions[1],dialogueStates[i].mainPlayerOptions[2], dialogueStates[i].shouldPlayerBeStatic);
                }

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
                    StartCoroutine(LineTimer(dialogue,dialogueStates[i]));
                    yield return new WaitUntil(()=> lineOver);
                    EmptyDialogue();
                }   
            }
            Debug.Log("Dialogue Was Finished: '"+dialogueStates[0].mainDialogue[0]+"'");
        }
        isDialogueFinished = true;
    }

    public IEnumerator LineTimer(string dialogue, DialogueState dialogueState)
    {
        for(float i = 0; i < dialogue.Length*timeDialogueShownPerChar+dialogueState.textLingerDuration+extraDialogueLingering; i+=0.1f)
        {
            if(lineOver)
            {
                yield return null;
            } else
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
        lineOver = true;
    }
}


