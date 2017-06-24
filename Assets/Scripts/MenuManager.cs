using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuManager : MonoBehaviour {
	public void StartGame () {
        SceneManager.LoadScene("Game");
	}
}
