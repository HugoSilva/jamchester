using UnityEngine;
using UnityEngine.UI;

public class GameMode : MonoBehaviour
{
    public float GameTickInterval;
    public float GameTimerOffset;
    public int CloneDelayTicks;
    public int ScoreP1 = 0;
    public int ScoreP2 = 0;
    public Text ScoreP1Text;
    public Text ScoreP2Text;

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

    public void OnPlayerDie(BasicCharacterController character)
    {
        if (character.MyID == 1) {
            ScoreP2++;
            ScoreP2Text.text = "" + ScoreP2;
        } else {
            ScoreP1++;
            ScoreP1Text.text = "" + ScoreP1;
        }
    }
    
    void Awake()
    {
        cInput.Init();
        cInput.SetKey("left1", Keys.Xbox1DPadLeft, "A");
        cInput.SetKey("right1", Keys.Xbox1DPadRight, "D");
        cInput.SetKey("up1", Keys.Xbox1DPadUp, "W");
        cInput.SetKey("down1", Keys.Xbox1DPadDown, "S");
        cInput.SetKey("attack1", Keys.Xbox1A);

        cInput.SetKey("left2", Keys.Xbox2DPadLeft);
        cInput.SetKey("right2", Keys.Xbox2DPadRight);
        cInput.SetKey("up2", Keys.Xbox2DPadUp);
        cInput.SetKey("down2", Keys.Xbox2DPadDown);
        cInput.SetKey("attack2", Keys.Xbox2A);
        Instance = this;

        timer = GameTimerOffset;
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