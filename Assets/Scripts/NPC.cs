using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    private static WaitForSeconds _waitForSeconds0_1 = new WaitForSeconds(0.1f);
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator animator;
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    public NPCStage npcStage;

    bool actionComplete;

    public void StartNewGameStage()
    {
        actionComplete = true;
        StartCoroutine(Initialize());
    }

    void Update()
    {
        animator.SetFloat("Velocity",agent.velocity.magnitude);
    }

    public IEnumerator Initialize()
    {
        foreach (string behaviourType in npcStage.BehaviourTypes)
        {
            actionComplete = false;
            
            agent.stoppingDistance = npcStage.proximityThreshold;

            if(behaviourType == "LoopPos")
            {
                //action only completed upon gameStage changing WIP
                StartCoroutine(LoopPos());
            } else if(behaviourType == "WalkToPos")
            {
                StartCoroutine(WalkToPos());
            } else if(behaviourType == "RunToPos")
            {
                StartCoroutine(RunToPos());
            } else if(behaviourType == "FollowWalk")
            {
                StartCoroutine(FollowWalk());
            } else if(behaviourType == "FollowRun")
            {
                StartCoroutine(FollowRun());
            } else
            {
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
                yield return new WaitUntil(() => Vector3.Distance(transform.position, targetLocation) < npcStage.proximityThreshold);
            }
        }
    }

    public IEnumerator WalkToPos()
    {
        foreach(Vector3 targetLocation in npcStage.npcTargetLocations)
        {
            agent.destination = targetLocation;
            agent.speed = walkSpeed;
            yield return new WaitUntil(() => Vector3.Distance(transform.position, targetLocation) < npcStage.proximityThreshold);
        }
        actionComplete = true;
    }

    public IEnumerator RunToPos()
    {
        foreach(Vector3 targetLocation in npcStage.npcTargetLocations)
        {
            agent.destination = targetLocation;
            agent.speed = runSpeed;
            yield return new WaitUntil(() => Vector3.Distance(transform.position, targetLocation) < npcStage.proximityThreshold);
        }
        actionComplete = true;
    }

    public IEnumerator FollowWalk()
    {
        while(!actionComplete)
        {   
            agent.destination = FindAnyObjectByType<Camera>().transform.position;
            agent.speed = walkSpeed;
            yield return _waitForSeconds0_1;
        }
    }

    public IEnumerator FollowRun()
    {
        while(!actionComplete)
        {   
            agent.destination = FindAnyObjectByType<Camera>().transform.position;
            agent.speed = walkSpeed;
            yield return _waitForSeconds0_1;
        }
    }
}
