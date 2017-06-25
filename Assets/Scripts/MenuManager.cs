using UnityEngine.SceneManagement;
using UnityEngine;

struct Hamsters {
    GameObject model;
    Sprite label;
    AudioSource sound;
}

public class MenuManager : MonoBehaviour {
    bool start = false;
    bool player1Selected = false;
    public GameObject selectPlayer1;
    public GameObject selectPlayer2;
    public GameObject selectedPlayer1;
    public GameObject selectedPlayer2;
    bool player2Selected = false;
    public GameObject startText;
    public GameObject characterSelect;
    public AudioSource startSound;
    public AudioSource selectSound;

    void Awake() {
        cInput.Init();
        cInput.SetKey("left1", Keys.Xbox1DPadLeft);
        cInput.SetKey("right1", Keys.Xbox1DPadRight);
        cInput.SetKey("select1", Keys.Xbox1A);
        cInput.SetKey("start1", Keys.Xbox1Start);
        //TEST ONLY
        //cInput.SetKey("left1", Keys.A);
        //cInput.SetKey("right1", Keys.D);
        //cInput.SetKey("select1", Keys.E);
        //cInput.SetKey("start1", Keys.Q);

        cInput.SetKey("left2", Keys.Xbox2DPadLeft);
        cInput.SetKey("right2", Keys.Xbox2DPadRight);
        cInput.SetKey("select2", Keys.Xbox2A);
        cInput.SetKey("start2", Keys.Xbox2Start);
    }

    void Update() {
        if(cInput.GetButtonDown("start1") && !start) {
            start = true;
            PlayButtonSFX();
        }
        if(start) {
            startText.SetActive(false);
            characterSelect.SetActive(true);
            if (cInput.GetButtonDown("select1")) {
                player1Selected = true;
                selectPlayer1.SetActive(false);
                selectedPlayer1.SetActive(true);
                PlaySelectedSFX();
            }
            if (cInput.GetButtonDown("select2")) {
                player2Selected = true;
                selectPlayer2.SetActive(false);
                selectedPlayer2.SetActive(true);
                PlaySelectedSFX();
            }
        }
        if (player1Selected && player2Selected) {
            StartGame();
        }
    }

    void PlayButtonSFX() {
        startSound.Play();
    }

    void PlaySelectedSFX() {
        selectSound.Play();
    }

    public void StartGame() {
        SceneManager.LoadScene("LukeTestScene");
    }
}
