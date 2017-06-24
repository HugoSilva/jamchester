using UnityEngine;

public class GameMode : MonoBehaviour
{
    public float GameTickInterval;
    public static GameMode Instance;
    float timer = 0.0f;
    event System.Action mGameTickEvent;
    event System.Action mPreGameTickEvent;

    public void RegisterGameTickMethod(System.Action method)
    {
        mGameTickEvent += method;
    }

    public void RegisterPreGameTickMethod(System.Action method)
    {
        mPreGameTickEvent += method;
    }
    
    void Awake()
    {
        Instance = this;
    }

    void LateUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= GameTickInterval) {
            timer = 0;
            if (mPreGameTickEvent != null) {
                mPreGameTickEvent();
            }
            if (mGameTickEvent != null) {
                mGameTickEvent();
            }
        }
    }
}