using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class NPC : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator animator;
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    public NPCStage npcStage;

    bool isFollowing;
    bool actionComplete;

    public void StartNewGameStage()
    {
        actionComplete = true;
        StartCoroutine(Initialize());
    }

    void Update()
    {
        animator.SetFloat("Velocity",agent.velocity.magnitude);

        if(isFollowing)
        {
            agent.destination = FindAnyObjectByType<Camera>().transform.position;
        }
    }

    public IEnumerator Initialize()
    {
        foreach (string behaviourType in npcStage.BehaviourTypes)
        {
            actionComplete = false;
            
            agent.stoppingDistance = npcStage.proximityThreshold;

            if(behaviourType == "LoopPos")
            {
                isFollowing = false;
                //action only completed upon gameStage changing WIP
                StartCoroutine(LoopPos());
            } else if(behaviourType == "WalkToPos")
            {
                isFollowing = false;
                StartCoroutine(WalkToPos());
            } else if(behaviourType == "RunToPos")
            {
                isFollowing = false;
                StartCoroutine(RunToPos());
            } else if(behaviourType == "FollowWalk")
            {
                isFollowing = true;
                agent.speed = walkSpeed;
            } else if(behaviourType == "FollowRun")
            {
                isFollowing = true;
                agent.speed = runSpeed;
            } else
            {
                isFollowing = false;
                actionComplete = true;
            }
            yield return new WaitUntil(() => actionComplete);
        }
    }

    public IEnumerator LoopPos()
    {
        while(!actionComplete)
        {
            foreach(Vector3 targetLocation in npcStage.npcTargetLocations)
            {
                agent.destination = targetLocation;
                agent.speed = walkSpeed;
                yield return new WaitUntil(() => Vector3.Distance(transform.position, targetLocation) <= npcStage.proximityThreshold+0.5f);
            }
        }
    }

    public IEnumerator WalkToPos()
    {
        Debug.Log("Target location amount:"+npcStage.npcTargetLocations.Count);

        foreach(Vector3 targetLocation in npcStage.npcTargetLocations)
        {
            agent.destination = targetLocation;
            agent.speed = walkSpeed;
            Debug.Log("Test");
            yield return new WaitUntil(() => Vector3.Distance(transform.position, targetLocation) <= npcStage.proximityThreshold+0.5f);
        }
        actionComplete = true;
    }

    public IEnumerator RunToPos()
    {
        foreach(Vector3 targetLocation in npcStage.npcTargetLocations)
        {
            agent.destination = targetLocation;
            agent.speed = runSpeed;
            yield return new WaitUntil(() => Vector3.Distance(transform.position, targetLocation) <= npcStage.proximityThreshold+0.5f);
        }
        actionComplete = true;
    }
}
