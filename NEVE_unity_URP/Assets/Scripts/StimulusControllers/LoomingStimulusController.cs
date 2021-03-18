using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoomingStimulusController : MonoBehaviour {
    public Transform sphere;
    public float unitPerSecond = 1;
    bool move = false;
    Vector3 startPos;

    void Start() {
        // use local position, so it is relative to looming stimulus controller
        startPos = sphere.localPosition;
        print("Press Space to start stimulus");
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            print("Start movement");
            move = true;
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            print("Resetting stimulus");
            sphere.localPosition = startPos;
            move = false;
        }

        if (move) {
            print("Stimulus is moving");
            sphere.Translate(0, 0, Time.deltaTime * unitPerSecond);
        }
    }
}
