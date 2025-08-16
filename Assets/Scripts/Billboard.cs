using UnityEngine;

public class Billboard : MonoBehaviour
{

    private Transform player;

    void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
    }
    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player);
    }
}
