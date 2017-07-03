using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class Hamster {
    public GameObject model;
    public GameObject hat;
    public Sprite label;
    public string name;
    public int score;
    public AudioClip sound;
    public bool isBot;

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
    public AudioClip clickSound;
    public int player1Value = 0;
    public int player2Value = 1;
    public GameObject logoBig;
    public GameObject logoSmall;
    public bool waitingForBot = false;

    public GameObject P1Start;
    public GameObject P2Start;

    void Awake() {
        cInput.Init();
        SetupInputs();
        SetPlayers();

        P1Start = GameObject.Find("P1Start");
        P2Start = GameObject.Find("P2Start");
        P1Start.SetActive(false);
        P2Start.SetActive(false);
    }

    void SetupInputs() {
        cInput.SetKey("left1", Keys.Xbox1DPadLeft, Keys.A);
        cInput.SetKey("right1", Keys.Xbox1DPadRight, Keys.D);
        cInput.SetKey("up1", Keys.Xbox1DPadUp, Keys.W);
        cInput.SetKey("down1", Keys.Xbox1DPadDown, Keys.S);
        cInput.SetKey("select1", Keys.Xbox1A, Keys.LeftControl);
        cInput.SetKey("start1", Keys.Xbox1Start, Keys.Enter);

        cInput.SetKey("left2", Keys.Xbox2DPadLeft, Keys.LeftArrow);
        cInput.SetKey("right2", Keys.Xbox2DPadRight, Keys.RightArrow);
        cInput.SetKey("up2", Keys.Xbox2DPadUp, Keys.UpArrow);
        cInput.SetKey("down2", Keys.Xbox2DPadDown, Keys.DownArrow);
        cInput.SetKey("select2", Keys.Xbox2A, Keys.RightControl);
        cInput.SetKey("start2", Keys.Xbox2Start, Keys.KeypadEnter);
    }

    void SetPlayers() {
        Transform label = selectPlayer1.transform.Find("Name");
        Transform model = selectPlayer1.transform.Find("ModelContainer");
        Transform selectedLabel = selectedPlayer1.transform.Find("Name");
        Transform selectedModel = selectedPlayer1.transform.Find("ModelContainer");

        foreach (Transform child in model) {
            GameObject.Destroy(child.gameObject);
        }

        foreach (Transform child in selectedModel) {
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

        foreach (Transform child in selectedModel2) {
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
        if(waitingForBot) {
            return;
        }
        if (cInput.GetButtonDown("start1")) {
            ActivatePlayer1();
        }

        if (cInput.GetButtonDown("start2")) {
            ActivatePlayer2();
        }

        if (start) {
            startText.SetActive(false);
            characterSelect.SetActive(true);
            if (cInput.GetButtonDown("select1")) {
                SelectPlayer1();
            }
            if (cInput.GetButtonDown("select2")) {
                SelectPlayer2();
            }
            if (cInput.GetButtonDown("left1")) {
                ChangePlayer(1, -1);
                playSFX(this.clickSound);
            }
            if (cInput.GetButtonDown("right1")) {
                ChangePlayer(1, 1);
                playSFX(this.clickSound);
            }
            if (cInput.GetButtonDown("left2")) {
                ChangePlayer(2, -1);
                playSFX(this.clickSound);
            }
            if (cInput.GetButtonDown("right2")) {
                ChangePlayer(2, 1);
                playSFX(this.clickSound);
            }
        }
        if (player1Selected && !selectPlayer2.activeInHierarchy) {
            StartCoroutine(BotPause(SetBot2));
            waitingForBot = true;
        }
        if (player2Selected && !selectPlayer1.activeInHierarchy) {
            StartCoroutine(BotPause(SetBot1));
            waitingForBot = true;
        }
        if (player1Selected && player2Selected && selectPlayer1.activeInHierarchy && selectPlayer2.activeInHierarchy) {
            StartGame();
        }
    }

    public delegate void SetBotDelegate();

    IEnumerator BotPause(SetBotDelegate callback) {
        yield return new WaitForSeconds(2);
        callback();
    }

    void SetBot2() {
        do {
            player2Value = Random.Range(0, models.Length - 1);
        } while (player1Value == player2Value);
        SetPlayers();
        SelectPlayer2(true);
        StartGame();
    }

    void SetBot1() {
        do {
            player1Value = Random.Range(0, models.Length - 1);
        } while (player1Value == player2Value);
        SetPlayers();
        SelectPlayer1(true);
        StartGame();
    }

    void ActivatePlayer1() {
        if(!start) {
            start = true;
            playSFX(startSound);
            logoBig.SetActive(false);
            logoSmall.SetActive(true);
        }
        selectPlayer1.SetActive(true);
        if(P1Start.gameObject.activeInHierarchy) {
            P1Start.gameObject.SetActive(false);
        }
        if(!selectPlayer2.activeInHierarchy) {
            P2Start.gameObject.SetActive(true);
        }
    }

    void ActivatePlayer2() {
        if (!start) {
            start = true;
            playSFX(startSound);
            logoBig.SetActive(false);
            logoSmall.SetActive(true);
        }
        selectPlayer2.SetActive(true);
        if (P2Start.gameObject.activeInHierarchy) {
            P2Start.gameObject.SetActive(false);
        }
        if (!selectPlayer1.activeInHierarchy) {
            P1Start.gameObject.SetActive(true);
        }
    }

    void SelectPlayer1(bool isBot = false) {
        player1Selected = true;
        selectPlayer1.SetActive(false);
        selectedPlayer1.SetActive(true);
        playSFX(this.models[player1Value].sound);
        this.models[player1Value].isBot = isBot;

        PlayerInfo info = GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>();
        info.SetPlayer1(this.models[player1Value]);
    }

    void SelectPlayer2(bool isBot = false) {
        player2Selected = true;
        selectPlayer2.SetActive(false);
        selectedPlayer2.SetActive(true);
        playSFX(this.models[player2Value].sound);
        this.models[player2Value].isBot = isBot;

        PlayerInfo info = GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>();
        info.SetPlayer2(this.models[player2Value]);
    }

    void playSFX(AudioClip clip) {
        if(player.isPlaying) {
            player.Stop();
        }
        player.clip = clip;
        player.Play();
    }

    IEnumerator StartPause(bool condition) {
        yield return new WaitForSeconds(2);
        if(condition) {
            SceneManager.LoadScene("Game");
        }
        else {
            ReadySound();
        }
    }

    public void StartGame() {
        StartCoroutine(StartPause(false));
    }

    public void ReadySound() {
        playSFX(readySound);
        StartCoroutine(StartPause(true));
    }
}
