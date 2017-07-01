using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour {

    public static PlayerInfo playerInfo;
    private Hamster player1;
    private Hamster player2;

    void Awake () {
        if (playerInfo == null) {
            DontDestroyOnLoad(gameObject);
            playerInfo = this;
        }
        else if(playerInfo != this) {
            Destroy(gameObject);
        }
    }

    public void SetPlayer1(Hamster player) {
        this.player1 = player;
    }

    public Hamster GetPlayer1() {
        return this.player1;
    }

    public void SetPlayer2(Hamster player) {
        this.player2 = player;
    }

    public Hamster GetPlayer2() {
        return this.player2;
    }

    public void AddPointPlayer1() {
        this.player1.score++;
    }

    public void AddPointPlayer2() {
        this.player2.score++;
    }

    public void ResetPlayers() {
        this.player1 = null;
        this.player2 = null;
    }
}
