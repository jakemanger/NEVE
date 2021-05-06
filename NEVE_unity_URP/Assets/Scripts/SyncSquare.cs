using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SyncSquare : MonoBehaviour
{
    Image image;
    Text text;
    public float flickerDuration = 0.1f;
    public Color flickerColor = Color.red;

    float timeWaited = 0f;
    bool flicker = false;

    public bool displayStimulusCode = false;
    public float stimulusCode = 9999f;


    void Start() {
        image = GetComponent<Image>();
        text = transform.GetChild(0).GetComponent<Text>();
    }

    public void Reset() {
        if (displayStimulusCode) {
            image.enabled = true;
            text.enabled = true;
        } else {
            image.enabled = false;
            text.enabled = false;
        }
        image.color = flickerColor;
        text.text = stimulusCode.ToString();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) {
            timeWaited = 0f;
            flicker = true;
        }

        if (flicker) {
            if (displayStimulusCode) {
                // flicker off or on
                image.enabled = !image.enabled;
                text.enabled = !text.enabled;
                flicker = false;
            } else {
                // flicker on and off
                image.enabled = true;
                text.enabled = true;

                if (timeWaited >= flickerDuration) {
                    flicker = false;
                    image.enabled = false;
                    text.enabled = false;
                }
                timeWaited += Time.deltaTime;

            }
        }
    }
}
