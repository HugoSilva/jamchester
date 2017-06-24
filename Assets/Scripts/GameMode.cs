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

    public void UnregisterGameTickMethod(System.Action method)
    {
        mGameTickEvent -= method;
    }

    public void UnregisterPreGameTickMethod(System.Action method)
    {
        mPreGameTickEvent -= method;
    }

    public float GetTimeUntilNextTick()
    {
        return GameTickInterval - timer;
    }
    
    void Awake()
    {
        cInput.Init();
        cInput.SetKey("left1", Keys.Xbox1DPadLeft);
        cInput.SetKey("right1", Keys.Xbox1DPadRight);
        cInput.SetKey("up1", Keys.Xbox1DPadUp);
        cInput.SetKey("down1", Keys.Xbox1DPadDown);

        cInput.SetKey("left2", Keys.Xbox2DPadLeft);
        cInput.SetKey("right2", Keys.Xbox2DPadRight);
        cInput.SetKey("up2", Keys.Xbox2DPadUp);
        cInput.SetKey("down2", Keys.Xbox2DPadDown);
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