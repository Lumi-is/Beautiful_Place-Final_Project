using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(DialogueHandler))]
public class GameStageManager : MonoBehaviour
{
    public List<GameStage> gameStages;
    public int currentGameStage;
    public Scene currentScene;
    private DialogueHandler dialogueHandler;
    private readonly Interactor interactor;
    public static float inGameTime;

    [SerializeField] List<string> npcNames;
    [SerializeField] List<Transform> npcTransforms;
    public Dictionary<string, Transform> npcDictionary;

    void Start()
    {
        dialogueHandler = GetComponent<DialogueHandler>();

        try
        {
            npcDictionary = new();
            for (int i = 0; i < npcNames.Count; i++)
            {
                npcDictionary.Add(npcNames[i],npcTransforms[i]);
            }
        } catch (Exception e)
        {
            Debug.LogWarning("NPC Names and Transforms amount mismatch error:"+e);
        }
        // Start when game begins
        StartCoroutine(PlayStage(gameStages[currentGameStage]));
    }

    IEnumerator PlayStage(GameStage gameStage)
    {
        currentScene = SceneManager.GetActiveScene();

        if(currentScene.buildIndex != gameStage.sceneIndex)
        {
            SceneManager.LoadScene(gameStage.sceneIndex);
        }

        inGameTime = gameStage.inGameTime;
        SetNPCBehaviour(gameStage.npcStages);
        StartCoroutine(dialogueHandler.PlayMainDialogue(gameStage.dialogueStates));
        yield return new WaitUntil(() => dialogueHandler.isDialogueFinished);
        StartCoroutine(WaitForObjectives(gameStage.objectives));    
    }

    void SetNPCBehaviour(List<NPCStage> npcStages)
    {
        foreach (NPCStage npcStage in npcStages)
        {
            NPC npc;
            npc = npcDictionary[npcStage.npcTag].GetComponent<NPC>();
            Debug.Log(npc.name);
            npc.npcStage = npcStage;
            npc.StartNewGameStage();
        }
    }
    private IEnumerator WaitForObjectives(List<Objective> objectives)
    {
        for(int i = 0; i < objectives.Count; i++)
        {
            Objective objective = objectives[i];
            objective.hasBeenFulfilled = false;

            if(objective.type == "ItemInInventory")
            {
                StartCoroutine(ObjectiveItemInInventory(objective));
            } else if(objective.type == "Timer")
            {
                StartCoroutine(ObjectiveTimer(objective));

            }else if(objective.type == "NPCLocation")
            {
                StartCoroutine(ObjectiveNPCLocation(objective));

            }else if(objective.type == "PlayerLocation")
            {
                StartCoroutine(ObjectivePlayerLocation(objective));

            }else if(objective.type == "ProximityToNPC")
            {
                StartCoroutine(ObjectiveProximityToNPC(objective));

            }else if(objective.type == "InteractedWith")
            {
                StartCoroutine(ObjectiveInteractedWith(objective));

            } else
            {
                objective.hasBeenFulfilled = true;
            }
            yield return new WaitUntil(() => objective.hasBeenFulfilled);
        } 

        currentGameStage++;
        if(gameStages.Count > currentGameStage)
        {
            //Debug.Log("new stage was started");
            StartCoroutine(PlayStage(gameStages[currentGameStage]));
        }
    }

    private IEnumerator ObjectiveItemInInventory(Objective objective)
    {
        while (objective.hasBeenFulfilled == false)
        {
            yield return new WaitForSeconds(1);
            foreach (Grabbable grabbable in interactor.inventoryList)
            {
                if(grabbable.gameObject.CompareTag(objective.itemTag))
                {
                    objective.hasBeenFulfilled = true;
                }
            }
        }
    }

    private IEnumerator ObjectiveTimer(Objective objective)
    {
        yield return new WaitForSeconds(objective.timer);
        objective.hasBeenFulfilled = true;
    }

    private IEnumerator ObjectiveNPCLocation(Objective objective)
    {
        yield return new WaitUntil(() => Vector3.Distance(npcDictionary[objective.npcName].position,objective.targetLocation) <= objective.proximityThreshold);
        objective.hasBeenFulfilled = true;
    }

    private IEnumerator ObjectivePlayerLocation(Objective objective)
    {
        yield return new WaitUntil(() => Vector3.Distance(StaticConstants.playerTransform.position,objective.targetLocation) <= objective.proximityThreshold);
        objective.hasBeenFulfilled = true;
    }

    private IEnumerator ObjectiveProximityToNPC(Objective objective)
    {
        yield return new WaitUntil(() => Vector3.Distance(StaticConstants.playerTransform.position,npcDictionary[objective.npcName].position) <= objective.proximityThreshold);
        objective.hasBeenFulfilled = true;
    }

    private IEnumerator ObjectiveInteractedWith(Objective objective)
    {
        yield return new WaitUntil(() => interactor.lastInteractedObject.CompareTag(objective.itemTag));
        objective.hasBeenFulfilled = true;
    }
}
