using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphericalStimulusGenerator : MonoBehaviour
{
    public GameObject stimulus;
    public Vector2 stimulusPolarPosition = new Vector2(0f, 0f);
    public Vector3 stimulusCartesianPosition;
    public float offsetFromCenter = 10f;
    public float stimulusSize = 1f;

    public bool controlFromMouse = false;
    public float mouseMoveSpeed = 2f;
    
    public float unitPerSecond = 1;
    public float startOffset = 10f;
    public float endOffset = -10f;
    public float duration = 5f;
    bool move = false;
    float timeElapsed = 0f;

    void Update()
    {
        if (controlFromMouse) {
            stimulusPolarPosition.x += -Input.GetAxis("Mouse Y") * mouseMoveSpeed;
            stimulusPolarPosition.y += Input.GetAxis("Mouse X") * mouseMoveSpeed;
            stimulusSize += Input.mouseScrollDelta.y * mouseMoveSpeed;
            stimulusSize = Mathf.Clamp(stimulusSize, 0f, 1000f);
        }
        stimulusCartesianPosition = PolarToCartesian(stimulusPolarPosition);
        stimulus.transform.position = stimulusCartesianPosition;        
        stimulus.transform.localScale = new Vector3(stimulusSize, stimulusSize, stimulusSize);

        if (Input.GetKeyDown(KeyCode.Space)) {
            print("Start/stop movement");
            move = !move;
            offsetFromCenter = startOffset;
            timeElapsed = 0f;
        }

        if (move) {
            print("Stimulus is moving");
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
