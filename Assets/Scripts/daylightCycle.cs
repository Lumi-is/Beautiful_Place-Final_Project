using UnityEngine;

public class daylightCycle : MonoBehaviour
{
    void Update()
    {
        if(GameStageManager.inGameTime != 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(GameStageManager.inGameTime,0,-20), Time.deltaTime*0.25f);
        }
    }
}
