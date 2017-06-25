using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class Hamster {
    public GameObject model;
    public GameObject hat;
    public Sprite label;
    public AudioClip sound;

    public Hamster(GameObject model, GameObject hat, Sprite label, AudioClip sound) {
        this.model = model;
        this.hat = hat;
        this.label = label;
        this.sound = sound;
    }
}


public class MenuManager : MonoBehaviour {
    bool start = false;
    bool player1Selected = false;
    public Hamster[] models;
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
    public int player1Value = 0;
    public int player2Value = 1;

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

        SetPlayers();
    }

    void SetPlayers() {
        Transform label = selectPlayer1.transform.Find("Name");
        Transform model = selectPlayer1.transform.Find("ModelContainer");
        Transform selectedLabel = selectedPlayer1.transform.Find("Name");
        Transform selectedModel = selectedPlayer1.transform.Find("ModelContainer");

        foreach (Transform child in model) {
            GameObject.Destroy(child.gameObject);
        }

        label.gameObject.GetComponent<SpriteRenderer>().sprite = this.models[player1Value].label;
        GameObject drop = Instantiate(this.models[player1Value].model, Vector3.zero, Quaternion.identity, model.gameObject.transform);

        selectedLabel.gameObject.GetComponent<SpriteRenderer>().sprite = this.models[player1Value].label;
        GameObject sel1 = Instantiate(this.models[player1Value].model, Vector3.zero, Quaternion.identity, selectedModel.gameObject.transform);
        sel1.transform.localPosition = Vector3.zero;
        sel1.transform.localRotation = new Quaternion(0, 60, 0, 45);

        drop.transform.localPosition = Vector3.zero;
        drop.transform.localRotation = new Quaternion(0, 180, 0, 0);
        drop.transform.localScale = new Vector3(10, 10, 10);

        Transform label2 = selectPlayer2.transform.Find("Name");
        Transform model2 = selectPlayer2.transform.Find("ModelContainer");
        Transform selectedLabel2 = selectedPlayer2.transform.Find("Name");
        Transform selectedModel2 = selectedPlayer2.transform.Find("ModelContainer");

        foreach (Transform child in model2) {
            GameObject.Destroy(child.gameObject);
        }
        label2.gameObject.GetComponent<SpriteRenderer>().sprite = this.models[player2Value].label;
        GameObject drop2 = Instantiate(this.models[player2Value].model, Vector3.zero, Quaternion.identity, model2.gameObject.transform);


        selectedLabel2.gameObject.GetComponent<SpriteRenderer>().sprite = this.models[player2Value].label;
        GameObject sel2 = Instantiate(this.models[player2Value].model, Vector3.zero, Quaternion.identity, selectedModel2.gameObject.transform);
        sel2.transform.localPosition = Vector3.zero;
        sel2.transform.localRotation = new Quaternion(0, -60, 0, 45);

        drop2.transform.localPosition = Vector3.zero;
        drop2.transform.localRotation = new Quaternion(0, 180, 0, 0);
        drop2.transform.localScale = new Vector3(10, 10, 10);
    }

    void ChangePlayer(int player, int value) {
        if(player == 1) {
            player1Value += value;

            if (player1Value > models.Length-1) {
                player1Value = 0;
            }
            if (player1Value < 0) {
                player1Value = models.Length - 1;
            }
        }
        if (player == 2) {
            player2Value += value;

            if (player2Value > models.Length - 1) {
                player2Value = 0;
            }
            if (player2Value < 0) {
                player2Value = models.Length - 1;
            }

        }
        SetPlayers();
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
            if (cInput.GetButtonDown("left1")) {
                ChangePlayer(1, -1);
            }
            if (cInput.GetButtonDown("right1")) {
                ChangePlayer(1, 1);
            }
            if (cInput.GetButtonDown("left2")) {
                ChangePlayer(2, -1);
            }
            if (cInput.GetButtonDown("right2")) {
                ChangePlayer(2, 1);
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
