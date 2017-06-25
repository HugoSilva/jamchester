using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

struct Hamsters {
    GameObject model;
    GameObject hat;
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
    public AudioSource player;
    public AudioClip startSound;
    public AudioClip selectSound;
    public AudioClip readySound;

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
        if((cInput.GetButtonDown("start1") || cInput.GetButtonDown("start2")) && !start) {
            start = true;
            playSFX(startSound);
        }
        if(start) {
            startText.SetActive(false);
            characterSelect.SetActive(true);
            if (cInput.GetButtonDown("select1")) {
                player1Selected = true;
                selectPlayer1.SetActive(false);
                selectedPlayer1.SetActive(true);
                playSFX(selectSound);
            }
            if (cInput.GetButtonDown("select2")) {
                player2Selected = true;
                selectPlayer2.SetActive(false);
                selectedPlayer2.SetActive(true);
                playSFX(selectSound);
            }
        }
        if (player1Selected && player2Selected) {
            StartGame();
        }
    }

    void playSFX(AudioClip clip) {
        if(player.isPlaying) {
            player.Stop();
        }
        player.clip = clip;
        player.Play();
    }

    IEnumerator Example(bool condition) {
        yield return new WaitForSeconds(2);
        if(condition) {
            SceneManager.LoadScene("LukeTestScene");
        }
        else {
            ReadySound();
        }
    }

    public void StartGame() {
        StartCoroutine(Example(false));
    }

    public void ReadySound() {
        playSFX(readySound);
        StartCoroutine(Example(true));
    }
}
