using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class TimerCountDown : MonoBehaviour {
    private float matchTime = 60;

	void Update () {
        matchTime -= Time.deltaTime;
        this.gameObject.GetComponent<Text>().text = "Time: "+ matchTime.ToString("0");
        if (matchTime <= 0) {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
