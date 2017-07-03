using UnityEngine.UI;
using UnityEngine;

public class TimerCountDown : MonoBehaviour {
    private float matchTime = 60;
    public ResultManager result;

	void Update () {
        matchTime -= Time.deltaTime;
        this.gameObject.GetComponent<Text>().text = "Time: "+ matchTime.ToString("0");
        if (matchTime <= 0) {
            result.ShowResults();
        }
    }
}
