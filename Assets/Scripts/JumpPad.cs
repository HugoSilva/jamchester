using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour {

    public float jumpPower;

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            PlayCollectSFX();
            ApplyJump(other.gameObject);
        }
    }

    void PlayCollectSFX() {
        this.gameObject.GetComponent<AudioSource>().Play();
    }

    void ApplyJump(GameObject Player) {
        Player.GetComponent<Rigidbody>().AddForce(transform.up * jumpPower, ForceMode.Impulse);
    }
}
