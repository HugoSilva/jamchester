using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCharacter : MonoBehaviour {

	void Update () {
        this.transform.Rotate(Vector3.up * (30f * Time.deltaTime));

    }
}
