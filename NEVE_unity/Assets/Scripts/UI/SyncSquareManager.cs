using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SyncSquareManager : MonoBehaviour
{
    public SyncSquare syncSquare;
    public int displayNum;
    public Canvas firstCanvas;
    public SyncSquare secondSyncSquare;
    public bool displaySecondSyncSquare = false;
    public int secondDisplayNum;
    public Canvas secondCanvas;
    public Image flicker;

    public float flickerDuration;
    public Color flickerColor;
    public bool displayStimulusCode = false;
    public float stimulusCode = 9999f;
    public float animalCode = 0f;
    public string timeText = "";
    public Color stimStateColor;

    public Vector2 syncSquarePos = new Vector2(-29.84f, 18.17102f);
    public float syncSquareScalar = 1f;


    void Start() {
        firstCanvas = syncSquare.transform.parent.GetComponent<Canvas>();
        secondCanvas = secondSyncSquare.transform.parent.GetComponent<Canvas>();
        flicker = syncSquare.flicker;
    }

    public void Reset() {
        firstCanvas.targetDisplay = displayNum;
        
        SetSyncSquareVariables(syncSquare);

        if (displaySecondSyncSquare) {
            secondCanvas.enabled = true;
            secondCanvas.targetDisplay = secondDisplayNum;
            SetSyncSquareVariables(secondSyncSquare);
        } else {
            secondCanvas.enabled = false;
        }
        
    }

    void SetSyncSquareVariables(SyncSquare square) {
        square.flickerDuration = flickerDuration;
        square.flickerColor = flickerColor;
        square.displayStimulusCode = displayStimulusCode;
        square.stimulusCode = stimulusCode;
        square.animalCode = animalCode;
        square.experimentId.text = stimulusCode.ToString();
        square.animalId.text = animalCode.ToString();
        RectTransform syncSquareRect = square.GetComponent<RectTransform>();
        syncSquareRect.anchoredPosition = syncSquarePos;
        square.transform.parent.GetComponent<CanvasScaler>().scaleFactor = syncSquareScalar;
    }

    void LateUpdate() {
        syncSquare.timeText.text = timeText;
        secondSyncSquare.timeText.text = timeText;
        syncSquare.stimStateImg.color = stimStateColor;
        secondSyncSquare.stimStateImg.color = stimStateColor;
    }
}
