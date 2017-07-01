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

    public GameObject player1;
    public GameObject player2;

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
        PlayerInfo info = GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>();

        if (character.MyID == 1) {
            ScoreP2++;
            ScoreP2Text.text = "" + ScoreP2;
            info.AddPointPlayer2();
        } else {
            ScoreP1++;
            ScoreP1Text.text = "" + ScoreP1;
            info.AddPointPlayer1();
        }
    }
    
    void Awake()
    {
        cInput.Init();
        cInput.SetKey("left1", Keys.Xbox1DPadLeft, Keys.A);
        cInput.SetKey("right1", Keys.Xbox1DPadRight, Keys.D);
        cInput.SetKey("up1", Keys.Xbox1DPadUp, Keys.W);
        cInput.SetKey("down1", Keys.Xbox1DPadDown, Keys.S);
        cInput.SetKey("attack1", Keys.Xbox1A, Keys.LeftControl);

        cInput.SetKey("left2", Keys.Xbox2DPadLeft, Keys.LeftArrow);
        cInput.SetKey("right2", Keys.Xbox2DPadRight, Keys.RightArrow);
        cInput.SetKey("up2", Keys.Xbox2DPadUp, Keys.UpArrow);
        cInput.SetKey("down2", Keys.Xbox2DPadDown, Keys.DownArrow);
        cInput.SetKey("attack2", Keys.Xbox2A, Keys.RightControl);
        Instance = this;

        DefinePlayers();
        timer = GameTimerOffset;
    }


    void DefinePlayers() {
        PlayerInfo info = GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>();
        Hamster player1 = info.GetPlayer1();
        Hamster player2 = info.GetPlayer2();

        GameObject p1 = Instantiate(player1.model, Vector3.zero, Quaternion.identity, this.player1.transform);
        p1.transform.localPosition = new Vector3(0, p1.transform.localPosition.y, 0);
        p1.transform.localRotation = Quaternion.Euler(0, -90, 0);
        p1.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

        GameObject p2 = Instantiate(player2.model, Vector3.zero, Quaternion.identity, this.player2.transform);
        p2.transform.localPosition = new Vector3(0, p2.transform.localPosition.y, 0);
        p2.transform.localRotation = Quaternion.Euler(0, -90, 0);
        p2.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
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