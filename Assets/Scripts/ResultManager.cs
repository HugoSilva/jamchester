using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class ResultManager : MonoBehaviour {
    
    public GameObject ScoreCanvas;
    public GameObject ResultCanvas;
    public GameObject Player1;
    public GameObject Player2;

    public void ShowResults() {
        Player1.SetActive(false);
        Player2.SetActive(false);
        ScoreCanvas.SetActive(false);
        ResultCanvas.SetActive(true);

        PlayerInfo info = GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>();
        Hamster player1 = info.GetPlayer1();
        Hamster player2 = info.GetPlayer2();

        Text p1Name = GameObject.Find("P1 Name").GetComponent<Text>();
        p1Name.text = player1.name;
        Text p2Name = GameObject.Find("P2 Name").GetComponent<Text>();
        p2Name.text = player2.name;

        Text p1Score = GameObject.Find("P1 Score").GetComponent<Text>();
        p1Score.text = player1.score.ToString();
        Text p2Score = GameObject.Find("P2 Score").GetComponent<Text>();
        p2Score.text = player2.score.ToString();

        Text p1Winner = GameObject.Find("P1 Winner").GetComponent<Text>();
        Text p2Winner = GameObject.Find("P2 Winner").GetComponent<Text>();

        if (player1.score > player2.score) {
            p1Winner.text = "Winner";
            p2Winner.text = "Loser";
        } else {
            p1Winner.text = "Loser";
            p2Winner.text = "Winner";
        }
        StartCoroutine(TimeToMenu());
    }

    IEnumerator TimeToMenu() {
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene("MainMenu");
    }
}
