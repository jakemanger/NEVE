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
    public Vector3 origin = Vector3.zero;
    public Vector2 rotationOffset = Vector2.zero;
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

    public bool ignoreKeyboard = false;

    public bool autoStart = false;

    // for mimicking expansion speed of another loom
    public bool mimicExpansionSpeed = false;
    public int mimicExpansionSpeedMethod = 0;
    public float referenceInitialDistance = 2f;
     public float referenceEndDistance = 2f;
    public float referenceSpeed = 1f;
    public float equalDistance = 1f;
    public float referenceDiameter = 1f;

    public float moveTime=1f;
    public Vector2 referenceStartPolarPosition = Vector2.zero;
    public Vector2 referenceEndPolarPosition = Vector2.zero;

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

    //AAA: Add new public variables for new input params

    public GameObject stimulus;
    Renderer stimulusRenderer;
    Material originalMaterial;

    public int stimulusType = 0; // 0 = icosphere, 1 = unity cube
    public GameObject[] stimuli; // 0 = icosphere, 1 = unity cube

    Outline outline;
    public float outlineWidth = 2f;
    public bool drawOutline = false;
    public int outlineType = 0;
    public Color outlineColor = Color.black;

    
    public MeshRenderer gratingSphereMesh;

    public bool opaqueObject = false;
    public Material target;
    public Material opaqueTarget;

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
            if (stimuli[i].activeSelf)
                stimuli[i].SetActive(false);
        }
        // select stimulus type
        stimulus = stimuli[stimulusType];

        stimulusRenderer = stimulus.GetComponent<Renderer>();
        if (opaqueObject) {
            stimulusRenderer.material = Instantiate<Material>(opaqueTarget);
        } else {
            stimulusRenderer.material = Instantiate<Material>(target);
        }
        stimulusRenderer.material.color = stimulusColour;

        SetupOutline();
        stimulus.SetActive(true);
        SetupFixedAngularSize();

        if (directPath) {
            Vector3 startPositionCartesian = PolarToCartesian(startPolarPosition, startDistance);
            stimulus.transform.localPosition = startPositionCartesian;
        } else {
            Vector3 pos = stimulus.transform.localPosition;
            stimulus.transform.localPosition = new Vector3(pos.x, pos.y, offsetFromCenter);
            transform.rotation = Quaternion.Euler(new Vector3(startPolarPosition.x, startPolarPosition.y, 0f));
        }

        stimulus.transform.localScale = startScale;

        if (stimulusType == 2) {
            SetupGratings();
        }

        if (delayToAppear > 0) {
            stimulusRenderer.enabled = false;
        } else {
            stimulusRenderer.enabled = true;
        }

    }

    void Update()
    {
        // inputs from user

        if (manualControl) {
            ManualControl();
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

        if ((Input.GetKeyDown(KeyCode.Space) && !ignoreKeyboard) || autoStart) {
            PrepareToMove();

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
            autoStart = false;
        }


        // movement calculations

        // wait delay period and then start approach
        if (wantToMove) {
            WaitToMove();
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
                if (startScale != endScale) {
                    if (mimicExpansionSpeed) {
                        throw new System.Exception("Changing scale doesn't make sense when mimicking the expansion speed of another stimulus.");
                    }
                    // scale axes
                    stimulus.transform.localScale = Vector3.Lerp(startScale, endScale, timeElapsed / duration);
                }
                Move(startDistance, endDistance, startPolarPosition, endPolarPosition);
            } else {
                // print("Coming back in");
                if (startScale != endScale) {
                    // scale axes
                    stimulus.transform.localScale = Vector3.Lerp(endScale, startScale, timeElapsed / duration);
                }
                Move(endDistance, startDistance, endPolarPosition, startPolarPosition);
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

        // always make the object face the origin (the eye)
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
        float progress = timeElapsed / duration;

        if (directPath) {
            // set start and end positions
            Vector3 startPositionCartesian = PolarToCartesian(startPolarCoordinate, startDistance);
            Vector3 endPositionCartesian = PolarToCartesian(endPolarCoordinate, endDistance);

            // modify progress or size (localScale) if mimicking expansion speed
            if (mimicExpansionSpeed) {
                if (mimicExpansionSpeedMethod == 1) {
                    // match expansionSpeed of directly approaching loom to another direct approaching looming stimuli
                    // by adjusting the current stimuli's distance to the observer over time
                    float diameter = stimulus.transform.localScale.x;
                    float tToCollision = referenceInitialDistance / referenceSpeed;
                    float distance = (
                        diameter / 2 / Mathf.Tan(
                                Mathf.Atan(referenceDiameter / (2 * referenceSpeed * (tToCollision - timeElapsed)))
                                + Mathf.Atan(diameter / (2 * equalDistance))
                                - Mathf.Atan(referenceDiameter / (2 * equalDistance)))
                    );
                    // moveTime - an extra delay to move, which is required in some mimic situations 
                    if (timeElapsed<moveTime) {
                        progress = (startDistance - startDistance) / (startDistance - endDistance);
                    }
                    if (timeElapsed>=moveTime){
                        progress = (startDistance - distance) / (startDistance - endDistance);
                    }
                    // print("Distance from Zahra's calc: " + distance);
                }
                if (mimicExpansionSpeedMethod == 2) {
                    // match a near miss stimulus with the expansion speed of a directly looming stimulus or vice versa
                    // by adjusting the current stimuli's size over time
                    Vector3 P = endPositionCartesian-startPositionCartesian;
                    Vector3 V = P / duration;
                    float diameter = startScale.x;
                    float stimulusDistance=Mathf.Sqrt(Mathf.Pow((V.x*timeElapsed+startPositionCartesian.x), 2) + Mathf.Pow((V.y*timeElapsed+startPositionCartesian.y), 2) + Mathf.Pow((V.z*timeElapsed+startPositionCartesian.z), 2));
                    Vector3 refStartPositionCartesian = PolarToCartesian(referenceStartPolarPosition, referenceInitialDistance);
                    Vector3 refEndPositionCartesian = PolarToCartesian(referenceEndPolarPosition, referenceEndDistance);

                    Vector3 pRef =  refEndPositionCartesian-refStartPositionCartesian;
                    Vector3 vRef = pRef / duration;
                    float refDistance=Mathf.Sqrt(Mathf.Pow((vRef.x*timeElapsed+refStartPositionCartesian.x), 2) + Mathf.Pow((vRef.y*timeElapsed+refStartPositionCartesian.y), 2) + Mathf.Pow((vRef.z*timeElapsed+refStartPositionCartesian.z), 2));

                    float newDiameter = 2 * stimulusDistance* Mathf.Tan(Mathf.Atan(referenceDiameter /(2*refDistance) ) - Mathf.Atan(referenceDiameter / (2*referenceInitialDistance)) + Mathf.Atan(diameter /(2*startDistance)));
                    
                    // assign new size to mimic the reference stimulus's expansion speed
                    stimulus.transform.localScale = new Vector3(newDiameter, newDiameter, newDiameter);
                    print(newDiameter);
                }

            }
            // assign the new position according to the new expansion speed
            stimulus.transform.localPosition = Vector3.Lerp(startPositionCartesian, endPositionCartesian, progress);
            // print("startDistance: " + startDistance + "; endDistance: " + endDistance + "; timeElapsed: " + timeElapsed);
            // print("Progress: " + progress + "; Distance from unity: " + Vector3.Distance(stimulus.transform.position, endPositionCartesian));
        } else {
            if (mimicExpansionSpeed) {
                throw new System.Exception("Mimic expansion speed can only be used when directPath == True");
            }
            // greater circle path
            offsetFromCenter = Mathf.Lerp(startDistance, endDistance, progress);
            // move rotation
            transform.rotation = Quaternion.Slerp(Quaternion.Euler(new Vector3(startPolarCoordinate.x, startPolarCoordinate.y, 0f)),
                                                            Quaternion.Euler(new Vector3(endPolarCoordinate.x, endPolarCoordinate.y, 0f)),
                                                            progress);
        }
    }

    Vector2 CartesianToPolar(Vector3 point) {
        Vector2 polar;
        // calculate longitude
        polar.y = Mathf.Atan2(point.x, point.z);
        // calculate Sqrt(Pow(x, 2), Pow(y, 2))
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

    void SetupOutline() {
        outline = stimulus.GetComponent<Outline>();

        if (opaqueObject) {
            outline.targetMaterial = Instantiate<Material>(opaqueTarget);
        } else {
            outline.targetMaterial = Instantiate<Material>(target);
        }
        outline.targetMaterialColor = stimulusColour;
        outline.outlineWidth = outlineWidth;
        outline.OutlineColor = outlineColor;
        if (outlineType == 1) { // if outline mode is 1, then use screen space, otherwise use worldspace
            outline.OutlineMode = Outline.Mode.OutlineVisible;
        } else {
            outline.OutlineMode = Outline.Mode.WorldSpace;
        }
        outline.enabled = drawOutline;
    }

    void SetupFixedAngularSize() {
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
    }

    public void PrepareToMove() {
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
    }

    void WaitToMove() {
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
}
