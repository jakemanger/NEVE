using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SyncSquare : MonoBehaviour
{
    Image image;
    public float flickerDuration = 0.1f;
    public Color flickerColor = Color.red;

    float timeWaited = 0f;
    bool flicker = false;


    void Start() {
        image = GetComponent<Image>();
    }

    public void Reset() {
        image.enabled = false;
        image.color = flickerColor;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) {
            timeWaited = 0f;
            flicker = true;
        }

        if (flicker) {
            image.enabled = true;

            if (timeWaited >= flickerDuration) {
                flicker = false;
                image.enabled = false;
            }
            timeWaited += Time.deltaTime;
        }
    }
}
