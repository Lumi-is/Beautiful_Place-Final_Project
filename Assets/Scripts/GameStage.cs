using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

[CreateAssetMenu(fileName = "GameStage", menuName = "ScriptableObjects/Create New Gamestage")]
public class GameStage : ScriptableObject
{
    public int sceneIndex;
    public List<Objective> objectives;
    public List<DialogueState> dialogueStates;
    public List<NPCStage> npcStages;
    [Tooltip("If put to 0, doesnt change time.")]
    public int inGameTime = 200;
}
