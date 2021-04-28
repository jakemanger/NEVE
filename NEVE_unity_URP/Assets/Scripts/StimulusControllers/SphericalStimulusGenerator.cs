using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphericalStimulusGenerator : MonoBehaviour
{
    // configurable parameters
    public float stimulusSize = 1f;
    public Vector2 stimulusPolarPosition = new Vector2(0f, 0f);
    public Vector3 targetLocationOffset = new Vector3(0f, 0f, 0f);
    public float startOffset = 100f;
    public float endOffset = 1f;
    public float duration = 5f;
    public float delayToApproach = 5f;
    public Color stimulusColour = Color.white;
    public bool manualControl = false;
    public float mouseMoveSpeed = 2f;

    float offsetFromCenter;
    bool move = false;
    float timeElapsed = 0f;
    Vector3 stimulusCartesianPosition;
    float delayTimeElapsed = 0f;
    Color lastStimulusColour = Color.white;

    public GameObject stimulus;
    Renderer stimulusRenderer;


    void Start() {
        stimulusRenderer = stimulus.GetComponent<Renderer>();
    }

    public void Setup() {
        offsetFromCenter = startOffset;
        delayTimeElapsed = 0f;
        move = false;
        print("Stimulus Reset");
    }

    void Update()
    {
        if (manualControl) {
            stimulusPolarPosition.x += -Input.GetAxis("Mouse Y") * mouseMoveSpeed;
            stimulusPolarPosition.y += Input.GetAxis("Mouse X") * mouseMoveSpeed;
            stimulusSize += Input.mouseScrollDelta.y * mouseMoveSpeed;
            stimulusSize = Mathf.Clamp(stimulusSize, 0f, 1000f);

            if (Input.GetKeyDown(KeyCode.Space)) {
                move = !move;
                offsetFromCenter = startOffset;
                timeElapsed = 0f;
            }
        } else {
            // wait delay period and then start approach
            delayTimeElapsed += Time.deltaTime;
            if (!move && delayTimeElapsed >= delayToApproach) {
                move = true;
                offsetFromCenter = startOffset;
                timeElapsed = 0f;
            }
        }

        // convert new polar position to cartesian
        stimulusCartesianPosition = PolarToCartesian(stimulusPolarPosition);
        // add offset to cartesian position in case you want a near miss stimuli
        stimulus.transform.position = targetLocationOffset + stimulusCartesianPosition;

        stimulus.transform.localScale = new Vector3(stimulusSize, stimulusSize, stimulusSize);

        if (lastStimulusColour != stimulusColour) {
            stimulusRenderer.material.color = stimulusColour;
            lastStimulusColour = stimulusColour;
        }

        // logic to change offset from target location of looming stimulus
        if (move) {
            offsetFromCenter = Mathf.Lerp(startOffset, endOffset, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
        }
    }

    Vector2 CartesianToPolar(Vector3 point) {
        Vector2 polar;
        // calculate longitude
        polar.y = Mathf.Atan2(point.x, point.z);
        // calculate sqrt(pow(x, 2), pow(y, 2))
        float xzLen = new Vector2(point.x, point.z).magnitude;
        polar.x = Mathf.Atan2(-point.y, xzLen);
        // convert to deg
        polar *= Mathf.Rad2Deg;

        return polar;
    }

    Vector3 PolarToCartesian(Vector2 polar) {
        Vector3 origin = new Vector3(0, 0, offsetFromCenter);

        // build a quaternion using euler angles for lat and lon
        Quaternion rotation = Quaternion.Euler(polar.x, polar.y, 0);
        // transform reference vector by the rotation
        Vector3 point = rotation * origin;

        return point;
    }
}
