using UnityEngine;

[RequireComponent(typeof(Interactor))]
[RequireComponent(typeof(DialogueHandler))]
[RequireComponent(typeof(GameStageManager))]
public class PlayerObject : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
