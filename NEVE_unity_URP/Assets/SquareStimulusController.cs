using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquareStimulusController : MonoBehaviour
{
    // configurable stimulus parameters
    public float width = 100f;
    public float height = 100f;
    
    public Vector2 startPos = new Vector2(0f, 0f);
    public Vector2 endPos = new Vector2(10f, 0f);

    public float duration = 5f;
    public float timeElapsed = 0f;
    public float delayToApproach = 5f;
    public Color stimulusColour = Color.white;
    public float numReps = 1f;

    // private variables
    bool currentlyReturning = false;
    float numRepsDone = 0;
    float delayTimeElapsed = 0f;
    bool wantToMove = false;
    bool move = false;
    bool justFinishedMoving = false;

    public StimulusState stimulusState = StimulusState.Waiting;

    // objects to manipulate
    RectTransform rectTransform;

    void Start() {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Reset() {
        currentlyReturning = false;
        numRepsDone = 0;
        delayTimeElapsed = 0f;
        wantToMove = false;
        move = false;
        justFinishedMoving = false;
        print("reset");
        rectTransform.position = startPos;
        rectTransform.sizeDelta = new Vector2(width, height);
        GetComponent<Image>().color = stimulusColour;
        stimulusState = StimulusState.Waiting;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (move || justFinishedMoving) {
                // if currently moving
                move = false;
                wantToMove = false;
            } else {
                wantToMove = true;
            }
            timeElapsed = 0f;
            numRepsDone = 0;
            delayTimeElapsed = 0f;
            currentlyReturning = false;
            justFinishedMoving = false;
        }

        // wait delay period and then start approach
        if (wantToMove) {
            // print("wanting to move");
            delayTimeElapsed += Time.deltaTime;
            if (!move && delayTimeElapsed >= delayToApproach) {
                move = true;
                wantToMove = false;
                delayTimeElapsed = 0f;
                currentlyReturning = false;
                timeElapsed = 0f;
                numRepsDone = 0;
            }
        }

        if (move) {
            stimulusState = StimulusState.Started;
            if (!currentlyReturning) {
                rectTransform.position = Vector2.Lerp(startPos, endPos, timeElapsed / duration);
            } else {
                rectTransform.position = Vector2.Lerp(endPos, startPos, timeElapsed / duration);
            }

            if (timeElapsed / duration >= 1f) {
                if ((numRepsDone) < numReps - 0.5f) {
                    currentlyReturning = !currentlyReturning;
                    timeElapsed = 0f;
                    numRepsDone += 0.5f;
                    // print("Completed a rep");
                } else {
                    // print("finished");
                    delayTimeElapsed = 0f;
                    timeElapsed = 0f;
                    numRepsDone = 0f;
                    move = false;
                    wantToMove = false;
                    justFinishedMoving = true;
                    stimulusState = StimulusState.Ended;
                }
            } 

            timeElapsed += Time.deltaTime;
        } 
    }
}
