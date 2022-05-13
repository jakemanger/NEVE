using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SphericalStimulusGenerator : GenericStimulusController
{

    [Header("Specific spherical stimulus parameters")]
    // configurable parameters
    public float stimulusSize = 1f;
    Vector2 stimulusPolarPosition = new Vector2(0f, 0f);
    public Vector2 startPolarPosition = new Vector2(0f, 0f);
    public Vector2 endPolarPosition = new Vector2(0f, 0f);
    public Vector3 startScale = new Vector3(1f, 1f, 1f);
    public Vector3 endScale = new Vector3(1f, 1f, 1f);
    public Vector3 targetLocationOffset = new Vector3(0f, 0f, 0f);
    public float startOffset = 100f;
    public float endOffset = 1f;
    public float duration = 5f;
    public float delayToApproach = 5f;
    public Color stimulusColour = Color.white;
    public bool manualControl = false;
    public float mouseMoveSpeed = 2f;
    // for gratings
    public float gratingNum = 100f;
    public int gratingIsSquare = 0;
    public float gratingMaxIntensity = 0.1f;
    public float gratingMinIntensity = 0f;

    public float flickerDuration = 0.1f; // time sphere renderer is off in seconds
    public bool canFlicker = false;
    bool flicker = false;
    float timeSinceFlickerStart = 0f;
    bool currentlyReturning = false;
    public float numReps = 1;
    float numRepsDone = 0f;
    bool wantToMove = false;
    bool justFinishedMoving = false;

    float offsetFromCenter;
    bool move = false;
    float timeElapsed = 0f;
    Vector3 stimulusCartesianPosition;
    float delayTimeElapsed = 0f;
    Color lastStimulusColour = Color.white;

    public Material fixedAngularSizeMaterial;
    public bool fixedAngularSize = false;
    public bool fixXAxis = true; // otherwise fix the Y axis
    public float minAngularAngle = -30f;
    public float maxAngularAngle = 30f;

    public GameObject stimulus;
    Renderer stimulusRenderer;

    public int stimulusType = 0; // 0 = icosphere, 1 = unity cube
    public GameObject[] stimuli; // 0 = icosphere, 1 = unity cube

    Outline outline;
    public float outlineWidth = 2f;
    public bool drawOutline = false;
    public Color outlineColor = Color.black;
    
    public MeshRenderer gratingSphereMesh;

    public override void Reset() {

        base.Reset();

        // reset variables
        offsetFromCenter = startOffset;
        currentlyReturning = false;
        numRepsDone = 0;
        delayTimeElapsed = 0f;
        wantToMove = false;
        move = false;
        justFinishedMoving = false;

        // disable all stimuli 
        for (int i = 0; i < stimuli.Length; i++)
        {
            stimuli[i].SetActive(false);
        }
        // select stimulus type
        stimulus = stimuli[stimulusType];
        outline = stimulus.GetComponent<Outline>();
        outline.enabled = drawOutline;
        outline.outlineWidth = outlineWidth;
        outline.OutlineColor = outlineColor;
        stimulus.SetActive(true); // enable selected stimuli

        stimulusRenderer = stimulus.GetComponent<Renderer>();
        if (!fixedAngularSize) {
            stimulusRenderer.material.color = stimulusColour;
        } else {
            SetupFixedAngularSizeMaterial();
        }

        Vector3 pos = stimulus.transform.localPosition;
        stimulus.transform.localPosition = new Vector3(pos.x, pos.y, offsetFromCenter);
        transform.rotation = Quaternion.Euler(new Vector3(startPolarPosition.x, startPolarPosition.y, 0f));

        stimulus.transform.localScale = startScale;

        if (stimulusType == 2) {
            SetupGratings();
        }
    }

    void Update()
    {
        if (manualControl) {
            if (!move && !wantToMove && !justFinishedMoving) {
                Vector3 rot = transform.eulerAngles;
                transform.rotation = Quaternion.Euler(new Vector3(
                        rot.x += -Input.GetAxis("Mouse Y") * mouseMoveSpeed,
                        rot.y += Input.GetAxis("Mouse X") * mouseMoveSpeed,
                        0f
                    )
                );
            }
            stimulusSize += Input.mouseScrollDelta.y * mouseMoveSpeed;
            stimulusSize = Mathf.Clamp(stimulusSize, 0f, 1000f);

            if (startScale == endScale) {
                stimulus.transform.localScale = new Vector3(stimulusSize, stimulusSize, stimulusSize);
            }

            if (Input.GetKeyDown(KeyCode.Alpha0)) {
                transform.rotation = Quaternion.identity;
                stimulus.transform.localScale = startScale;
            }
        } 

        if (Input.GetKeyDown(KeyCode.Space)) {
            if (move || justFinishedMoving) {
                // if currently moving
                move = false;
                wantToMove = false;
            } else {
                wantToMove = true;
            }
            offsetFromCenter = startOffset;
            timeElapsed = 0f;
            numRepsDone = 0;
            delayTimeElapsed = 0f;
            currentlyReturning = false;
            justFinishedMoving = false;

            if (manualControl) {
                if (startPolarPosition == endPolarPosition) {
                    // looming stimulus
                    Vector3 angles = transform.eulerAngles;
                    startPolarPosition = new Vector2(angles.x, angles.y);
                    endPolarPosition = new Vector2(angles.x, angles.y);
                } else {
                    // translating stimulus
                    Vector3 angles = transform.eulerAngles;
                    startPolarPosition = new Vector2(angles.x, angles.y);
                }
            } 
        }

        // wait delay period and then start approach
        if (wantToMove) {
            // print("wanting to move");
            delayTimeElapsed += Time.deltaTime;
            if (!move && delayTimeElapsed >= delayToApproach) {
                move = true;
                wantToMove = false;
                delayTimeElapsed = 0f;
                offsetFromCenter = startOffset;
                currentlyReturning = false;
                timeElapsed = 0f;
                numRepsDone = 0;
            }
        }

        if (canFlicker && Input.GetKeyDown(KeyCode.F)) {
            flicker = true;
            timeSinceFlickerStart = 0f;
        }
        if (flicker) {
            stimulusRenderer.enabled = false;
            timeSinceFlickerStart += Time.deltaTime;
            if (timeSinceFlickerStart >= flickerDuration) {
                stimulusRenderer.enabled = true;
                flicker = false;
            }
        }



        // logic to change offset and polar position of stimulus
        if (move) {
            base.stimulusState = StimulusState.Started;
            if (!currentlyReturning) {
                // print("Moving out");
                offsetFromCenter = Mathf.Lerp(startOffset, endOffset, timeElapsed / duration);
                // move rotation
                transform.rotation = Quaternion.Slerp(Quaternion.Euler(new Vector3(startPolarPosition.x, startPolarPosition.y, 0f)),
                                                                Quaternion.Euler(new Vector3(endPolarPosition.x, endPolarPosition.y, 0f)),
                                                                timeElapsed / duration);
                if (startScale != endScale) {
                    // scale axes
                    stimulus.transform.localScale = Vector3.Lerp(startScale, endScale, timeElapsed / duration);
                }
            } else {
                // print("Coming back in");
                offsetFromCenter = Mathf.Lerp(endOffset, startOffset, timeElapsed / duration);
                // move rotation
                transform.rotation = Quaternion.Slerp(Quaternion.Euler(new Vector3(endPolarPosition.x, endPolarPosition.y, 0f)),
                                                                Quaternion.Euler(new Vector3(startPolarPosition.x, startPolarPosition.y, 0f)),
                                                                timeElapsed / duration);
                if (startScale != endScale) {
                    // scale axes
                    stimulus.transform.localScale = Vector3.Lerp(endScale, startScale, timeElapsed / duration);
                }
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
                    base.stimulusState = StimulusState.Ended;
                }
            } 

            timeElapsed += Time.deltaTime;
        } 
        Vector3 pos = stimulus.transform.localPosition;
        stimulus.transform.localPosition = new Vector3(pos.x, pos.y, offsetFromCenter);

        // add offset to cartesian position in case you want a near miss stimuli
        transform.position = targetLocationOffset;
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

    void SetupGratings() {
        Material mat = gratingSphereMesh.material;
        mat.SetFloat("_Density", gratingNum);
        mat.SetInt("_Square", gratingIsSquare);
        mat.SetFloat("_Minimum", gratingMinIntensity);
        mat.SetFloat("_Maximum", gratingMaxIntensity);
        gratingSphereMesh.material = mat;
    }

    void SetupFixedAngularSizeMaterial () {
        Material mat = fixedAngularSizeMaterial;
        mat.SetColor("_Color", stimulusColour);
        mat.SetInt("_FixX", fixXAxis ? 1 : 0);
        mat.SetFloat("_MinAngle", minAngularAngle);
        mat.SetFloat("_MaxAngle", maxAngularAngle);
        Material[] materials = stimulusRenderer.materials;
        materials[0] = mat;
        stimulusRenderer.materials = materials;
    }
}
