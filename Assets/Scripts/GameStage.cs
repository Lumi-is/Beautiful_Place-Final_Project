using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

[CreateAssetMenu(fileName = "GameStage", menuName = "ScriptableObjects/Create New Gamestage")]
public class GameStage : ScriptableObject
{
    public List<Objective> objectives;
    public List<DialogueState> dialogueStates;
    public List<NPCStage> npcStages;
    public int inGameTime = 200;
}
