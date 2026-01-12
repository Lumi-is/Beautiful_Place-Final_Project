using UnityEngine;

public class StaticConstants : MonoBehaviour
{

    public static Transform playerTransform;
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
}
