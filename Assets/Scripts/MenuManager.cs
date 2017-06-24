using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuManager : MonoBehaviour {
    bool start = false;
    bool playersSelected = false;
    public GameObject startText;
    public GameObject characterSelect;
    public AudioSource startSound;
    public AudioSource selectSound;

    void Update() {
        if(Input.GetButtonDown("Start") && !start) {
            start = true;
            PlayButtonSFX();
        }
        if(start) {
            startText.SetActive(false);
            characterSelect.SetActive(true);
            if (Input.GetButtonDown("Fire1")) {
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
