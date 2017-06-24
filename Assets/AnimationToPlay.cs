using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationToPlay : MonoBehaviour {

    public AnimationClip anim;

	void Awake () {
        this.GetComponent<Animation>().Play(anim.name);
	}
}
