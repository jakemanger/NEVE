using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;
using System.Reflection;


// a class to control the writing of information to file
// about the simulation each frame
public class FrameWriter : MonoBehaviour
{
    public bool recordEachFrame = true;

    // only used if recordEachFrame is false
    public float recordingFrequency = 1f; // in seconds

    string logsDir = "trial_logs/";

    public Transform stimTrans;

    public Image syncSquareImg;

    public string experimentId = "test_";
    public string outputFilePath;
    StreamWriter _sw;

    bool startNewFile = false;
    bool startedNewFile = false;

    // to make it easy to be sure screen information matches
    // exactly with the csv, we set the SyncSquare info here

    // Use SphericalStimulusGenerator or SquareStimulusGenerator
    // in the inspector
    GenericStimulusController[] stimulusControllers;
    public Image stimulusStateImage;
    public Text timeText;

    int stimControllerLength;

    List<Transform> transformsToRecord = new List<Transform>();

    public void Reset() {
        startedNewFile = false;
        startNewFile = true;

        // find all stimulus controllers GenericStimulusController
        stimulusControllers = GameObject.FindObjectsOfType<GenericStimulusController>();
        stimControllerLength = stimulusControllers.Length;

        transformsToRecord = new List<Transform>();
  
        // record the transforms of active stimulus controller objects
        for (int i = 0; i < stimControllerLength; i++) {
            Transform stimControllerTrans = stimulusControllers[i].transform;
            for (int j = 0; j < stimControllerTrans.childCount; j++)
            {
                if(stimControllerTrans.GetChild(j).gameObject.activeSelf == true)
                {
                    transformsToRecord.Add(stimControllerTrans.GetChild(j));
                }
            }
        }

        // find the SocketMovementController
        SocketMovementController socketMoveController = GameObject.FindObjectOfType<SocketMovementController>();
        if (socketMoveController != null)
        {
            transformsToRecord.Add(socketMoveController.transform);
        }

        SaveStimulusManagerValues();
    }

    // Update is called once per frame
    void Update()
    {
        if (startedNewFile && recordEachFrame) {
            setSyncSquareValues();
            WriteData();
        }

        if (startNewFile) {
            outputFilePath = logsDir + System.DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + experimentId + ".csv";
            if (outputFilePath == null || outputFilePath == "") {
                Debug.LogError("FrameWriter outputFilePath was not specified.");
            }

            // if the logs directory doesn't already exist, create it
            if (!Directory.Exists(logsDir)) {
                Directory.CreateDirectory(logsDir);
            }

            _sw = System.IO.File.AppendText(outputFilePath);
            // write first line (the headers)
            if (!recordEachFrame) {
                InvokeRepeating("WriteData", 0, 1/recordingFrequency);
            }

            string headers = "unityTime, datetime, stimulusOn";

            for (int i = 0; i < transformsToRecord.Count; i++) {
                string name = transformsToRecord[i].name;
                if (transformsToRecord[i].parent != null)
                    name = transformsToRecord[i].parent.name + "." + transformsToRecord[i].name;

                headers += ", " + name  + "_x, " + name + "_y, " + name + "_z, " + name + "_scale_x, " + name + "_scale_y, " + name + "_scale_z";
            }

            for (int i = 0; i < stimulusControllers.Length; i++) {
                headers += ", " + stimulusControllers[i].name + "_stimulusState";
            }

            _sw.WriteLine(headers);

            startNewFile = false;
            startedNewFile = true;
        }
    }

    public void WriteData()
    {
        string data = Time.time + ", " + System.DateTime.Now + ", " + syncSquareImg.enabled;
        for (int i = 0; i < transformsToRecord.Count; i++) {
            Vector3 position = transformsToRecord[i].position;
            Vector3 localScale = transformsToRecord[i].localScale;
            data += ", " + position.x + ", " + position.y + ", " + position.z + ", " + localScale.x + ", " + localScale.y + ", " + localScale.z;
        }

        for (int i = 0; i < stimulusControllers.Length; i++) {
            data += ", " + stimulusControllers[i].stimulusState;
        }

        _sw.WriteLine(data);
    }

    void OnDestroy()
    {
        if (_sw != null) {
            _sw.Close();
        }
    }

    void setSyncSquareValues() {
        Color stimStateColor = Color.black;

        StimulusState stimState = StimulusState.Waiting;

        if (stimControllerLength > 0) {
            stimState = stimulusControllers[0].stimulusState;
        } else {
            stimState = StimulusState.Waiting;
        }
        
        if (stimState == StimulusState.Waiting) {
            stimStateColor = Color.black;
        } else if (stimState == StimulusState.Started) {
            stimStateColor = Color.white;
        } else if (stimState == StimulusState.Ended) {
            stimStateColor = Color.grey;
        }

        stimulusStateImage.color = stimStateColor;

        timeText.text = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");
    }

    void SaveStimulusManagerValues()
    {
        string path = logsDir + System.DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + experimentId + "_params.txt";
        StreamWriter sw = System.IO.File.AppendText(path);

        GenericStimulusManager stimulusManager = GameObject.FindObjectOfType<GenericStimulusManager>();

        if (stimulusManager == null) {
            Debug.LogError("FrameWriter could not find GenericStimulusManager.");
            return;
        }

        // const BindingFlags flags = BindingFlags.Public;
        Type myObject_type = stimulusManager.GetType();
        FieldInfo[] fields = myObject_type.GetFields(
            BindingFlags.Instance | 
            BindingFlags.Static |
            BindingFlags.NonPublic |
            BindingFlags.Public
        );
        // print(fields);
        foreach(FieldInfo field in fields) {
            if (field != null) {
                    string text = field.Name + ": " + field.GetValue(stimulusManager);
                    sw.WriteLine(text);
                    // print(text);
                }
        }
        sw.Close();
    }
}
