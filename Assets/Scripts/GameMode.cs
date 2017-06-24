using UnityEngine;

public class GameMode : MonoBehaviour
{
    public float GameTickInterval;
    public static GameMode Instance;
    float timer = 0.0f;
    event System.Action mGameTickEvent;

    public void RegisterGameTickMethod(System.Action method)
    {
        mGameTickEvent += method;
    }
    
    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= GameTickInterval) {
            timer = 0;
            if (mGameTickEvent != null) {
                mGameTickEvent();
            }
        }
    }
}