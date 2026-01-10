using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameStage", menuName = "ScriptableObjects/Create New Gamestage")]
public class GameStage : ScriptableObject
{
    public List<Objective> objectives;
    public List<DialogueState> dialogueStates;
    public List<NPCStage> npcStages;
}
