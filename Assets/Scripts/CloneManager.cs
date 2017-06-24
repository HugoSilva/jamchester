using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneManager : MonoBehaviour {

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            if(other.GetComponent<BasicCharacterController>().bUserInput) {
                other.GetComponent<BasicCharacterController>().CreateClone();
                Debug.Log("Created a new clone");
            }
        }
    }
}
