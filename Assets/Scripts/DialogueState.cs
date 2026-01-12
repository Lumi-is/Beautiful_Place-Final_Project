using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueState", menuName = "ScriptableObjects/Create New Dialoguestate")]
public class DialogueState : ScriptableObject
{
    public string npcName;
    public float textLingerDuration;
    
    public bool shouldPlayerBeStatic;
    
    public List<string> mainDialogue;
    public List<string> mainPlayerOptions;
    public List<string> mainAnswerA;
    public List<string> mainAnswerB;
    public List<string> mainAnswerC;

    public List<string> optionalDialogue;
    public List<string> optionalPlayerOptions;
    public List<string> optionalAnswerA;
    public List<string> optionalAnswerB;
    public List<string> optionalAnswerC;
}
