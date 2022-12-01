using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SyncSquare : MonoBehaviour
{
    public float flickerDuration = 0.1f;
    public Color flickerColor = Color.red;
    public Color textColor = Color.white;

    float timeWaited = 0f;
    bool doFlicker = false;

    public bool displayStimulusCode = false;
    public float stimulusCode = 9999f;
    public float animalCode = 0f;

    public GameObject additionalInfo;
    public Image flicker;
    public Text experimentId;
    public Text animalId;
    public Text timeText;


    public void Reset() {
        if (displayStimulusCode) {
            flicker.enabled = true;
            experimentId.enabled = true;
            animalId.enabled = true;
            additionalInfo.SetActive(true);
        } else {
            flicker.enabled = false;
            experimentId.enabled = false;
            animalId.enabled = false;
            additionalInfo.SetActive(false);
        }
        flicker.color = flickerColor;
        experimentId.text = stimulusCode.ToString();
        animalId.text = animalCode.ToString();
        experimentId.color = textColor;
        animalId.color = textColor;
        timeText.color = textColor;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) {
            timeWaited = 0f;
            doFlicker = true;
        }

        if (doFlicker) {
            if (displayStimulusCode) {
                // flicker off or on
                flicker.enabled = !flicker.enabled;
                experimentId.enabled = !experimentId.enabled;
                doFlicker = false;
            } else {
                // flicker on and off
                flicker.enabled = true;
                experimentId.enabled = true;

                if (timeWaited >= flickerDuration) {
                    doFlicker = false;
                    flicker.enabled = false;
                    experimentId.enabled = false;
                }
                timeWaited += Time.deltaTime;

            }
        }
    }
}
