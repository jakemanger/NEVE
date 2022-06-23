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
    public Vector3 origin = new Vector3(0f, 0f, 0f);
    public float startDistance = 100f;
    public float endDistance = 1f;
    public float duration = 5f;
    public float delayToApproach = 5f;
    public float delayToAppear = 0f;
    public Color stimulusColour = Color.white;
    public bool manualControl = false;
    public float mouseMoveSpeed = 2f;
    public bool directPath = true; // if false, the stimulus will follow the greater circle path according to how elevation and azimuth were changed.
    public bool hideAtEnd = false;


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
    float delayToAppearTimeElapsed = 0f;
    Color lastStimulusColour = Color.white;

    public Material fixedAngularSizeMaterial;
    public bool fixedAngularSize = false;
    public bool fixXAxis = true; // otherwise fix the Y axis
    public float minAngularAngle = -30f;
    public float maxAngularAngle = 30f;

    public GameObject stimulus;
    Renderer stimulusRenderer;
    Material originalMaterial;

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
        offsetFromCenter = startDistance;
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
            if (originalMaterial != null) {
                stimulusRenderer.material = originalMaterial;
            }
            stimulusRenderer.material.color = stimulusColour;
        } else {
            if (originalMaterial == null) {
                originalMaterial = stimulusRenderer.material;
            }
            SetupFixedAngularSizeMaterial();
        }

        if (directPath) {
            Vector3 startPositionCartesian = PolarToCartesian(startPolarPosition, startDistance);
            stimulus.transform.position = startPositionCartesian;
        } else {
            Vector3 pos = stimulus.transform.localPosition;
            stimulus.transform.localPosition = new Vector3(pos.x, pos.y, offsetFromCenter);
            transform.rotation = Quaternion.Euler(new Vector3(startPolarPosition.x, startPolarPosition.y, 0f));
        }

        stimulus.transform.localScale = startScale;

        if (stimulusType == 2) {
            SetupGratings();
        }

        print(delayToAppear);
        if (delayToAppear > 0) {
            stimulusRenderer.enabled = false;
        } else {
            stimulusRenderer.enabled = true;
        }
    }

    void Update()
    {
        if (manualControl) {
            ManualControl();
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            if (move || justFinishedMoving) {
                // if currently moving
                move = false;
                wantToMove = false;
            } else {
                wantToMove = true;
            }
            offsetFromCenter = startDistance;
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
            if (hideAtEnd) {
                stimulusRenderer.enabled = true;
            }
            if (!move && delayTimeElapsed >= delayToApproach) {
                move = true;
                wantToMove = false;
                delayTimeElapsed = 0f;
                offsetFromCenter = startDistance;
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

        if ((wantToMove || move) && delayToAppear > 0) {
            delayToAppearTimeElapsed += Time.deltaTime;
            if (delayToAppearTimeElapsed >= delayToAppear) {
                delayToAppearTimeElapsed = 0f;
                stimulusRenderer.enabled = true;
                delayToAppear = 0f;
            }
        }


        // logic to change offset and polar position of stimulus
        if (move) {
            base.stimulusState = StimulusState.Started;
            if (!currentlyReturning) {
                // print("Moving out");
                Move(startDistance, endDistance, startPolarPosition, endPolarPosition);
                if (startScale != endScale) {
                    // scale axes
                    stimulus.transform.localScale = Vector3.Lerp(startScale, endScale, timeElapsed / duration);
                }
            } else {
                // print("Coming back in");
                Move(endDistance, startDistance, endPolarPosition, startPolarPosition);
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
                    if (hideAtEnd) {
                        stimulusRenderer.enabled = false;
                    }
                }
            } 

            timeElapsed += Time.deltaTime;
        } 

        if (!directPath) {
            Vector3 pos = stimulus.transform.localPosition;
            stimulus.transform.localPosition = new Vector3(pos.x, pos.y, offsetFromCenter);
        }
        // add offset to the origin
        transform.position = origin;

        // always make the object face the origin
        stimulus.transform.LookAt(origin);
    }

    void ManualControl() {
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

    void Move(float startDistance, float endDistance, Vector2 startPolarCoordinate, Vector2 endPolarCoordinate) {
        if (directPath) {
            Vector3 startPositionCartesian = PolarToCartesian(startPolarCoordinate, startDistance);
            Vector3 endPositionCartesian = PolarToCartesian(endPolarCoordinate, endDistance);
            stimulus.transform.position = Vector3.Lerp(startPositionCartesian, endPositionCartesian, timeElapsed / duration);
        } else {
            // greater circle path
            offsetFromCenter = Mathf.Lerp(startDistance, endDistance, timeElapsed / duration);
            // move rotation
            transform.rotation = Quaternion.Slerp(Quaternion.Euler(new Vector3(startPolarCoordinate.x, startPolarCoordinate.y, 0f)),
                                                            Quaternion.Euler(new Vector3(endPolarCoordinate.x, endPolarCoordinate.y, 0f)),
                                                            timeElapsed / duration);
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

    Vector3 PolarToCartesian(Vector2 polar, float distance) {
        Vector3 origin = new Vector3(0, 0, distance);

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
        gratingSphereMesh.material = Instantiate<Material>(mat);
    }

    void SetupFixedAngularSizeMaterial () {
        Material mat = fixedAngularSizeMaterial;
        mat.SetColor("_Color", stimulusColour);
        mat.SetInt("_FixX", fixXAxis ? 1 : 0);
        mat.SetFloat("_MinAngle", minAngularAngle);
        mat.SetFloat("_MaxAngle", maxAngularAngle);
        Material[] materials = stimulusRenderer.materials;
        materials[0] = Instantiate<Material>(mat);
        stimulusRenderer.materials = materials;
    }
}
