using System;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NPCStage", menuName = "ScriptableObjects/Create New NPCStage")]
public class NPCStage : ScriptableObject
{
    public string npcTag;

    [Tooltip("Types: Loop between positions (LoopPos), Walk to position (WalkToPos), Run to position (RunToPos), Follow player by walking (FollowWalk), Follow player by running (FollowRun), Stay (Stay)")]
    public List<string> BehaviourTypes;
    public List<Vector3> npcTargetLocations;
    public float proximityThreshold = 3;
}
