using UnityEngine;

[CreateAssetMenu(fileName = "Objective", menuName = "ScriptableObjects/Create New Objective")]
public class Objective : ScriptableObject 
{
    [Tooltip("Types: ItemInInventory, Timer, NPCLocation, PlayerLocation, ProximityToNPC, InteractedWith")]
    public string type;
    public bool hasBeenFulfilled = false;


    //Type data
    [Tooltip("What the item's tag should be for 'ItemInInvetory' type")]
    public string itemTag;
    [Tooltip("How many seconds until next gamestage")]
    public int timer;


    [Tooltip("Which NPC the types which require an npc, make a reference to.")]
    public string npcName;


    [Tooltip("Location targeted by 'NPCLocation' and 'PlayerLocation' types.")]
    public Vector3 targetLocation;
    [Tooltip("Overrides the default distance from the target position in which objective is counted as 'fullfilled'.")]
    public float proximityThreshold;
    [Tooltip("Interactable item's name for 'InteractedWith' type")]
    public string interactable;
}
