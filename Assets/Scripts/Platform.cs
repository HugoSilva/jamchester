using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    public Transform platform;
    public Transform begin;
    public Transform end;
    private bool startDirection;
    public float speed = 1.0F;
    private float startTime;
    private float journeyLength;

    void Awake () {
        platform.position = begin.position;
        startDirection = true;
        startTime = Time.time;
        journeyLength = Vector3.Distance(begin.position, end.position);
    }

	void Update () {

        Vector3 current = platform.position;
        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = distCovered / journeyLength;
        if (startDirection) {
            platform.position = Vector3.Lerp(begin.position, end.position, fracJourney);
            if(Vector3.Distance(platform.position, end.position) == 0) {
                startDirection = false;
                startTime = Time.time;
            }
        }
        else {
            platform.position = Vector3.Lerp(end.position, begin.position, fracJourney);
            if (Vector3.Distance(platform.position, begin.position) == 0) {
                startDirection = true;
                startTime = Time.time;
            }
        }
    }
}