using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuManager : MonoBehaviour {
    bool start = false;
    bool playersSelected = false;
    public GameObject startText;
    public GameObject characterSelect;
    public AudioSource startSound;
    public AudioSource selectSound;

    void Awake() {
        cInput.Init();
        cInput.SetKey("left1", Keys.Xbox1DPadLeft);
        cInput.SetKey("right1", Keys.Xbox1DPadRight);
        cInput.SetKey("select1", Keys.Xbox1A);
        //cInput.SetKey("start1", Keys.Xbox1Start);
        cInput.SetKey("start1", Keys.M);

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
                playersSelected = true;
            }
        }
        if (playersSelected) {
            PlayButtonSFX();
            StartGame();
        }
    }

    void PlayButtonSFX() {
        startSound.Play();
    }

    public void StartGame() {
        SceneManager.LoadScene("Game");
    }
}
