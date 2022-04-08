using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;

// a class to control the writing of information to file
// about the simulation each frame
public class FrameWriter : MonoBehaviour
{
    public bool recordEachFrame = true;

    // only used if recordEachFrame is false
    public float recordingFrequency = 1f; // in seconds

    string logsDir = "trial_logs/";

    public Transform stimTrans;

    public Transform stimTrans1;
    public Transform stimTrans2;

    public Renderer stimRenderer;
    public Image syncSquareImg;

    public string experimentId = "test_";
    public string outputFilePath;
    StreamWriter _sw;

    bool startNewFile = false;
    bool startedNewFile = false;

    // to make it easy to be sure screen information matches
    // exactly with the csv, we set the SyncSquare info here
    public SphericalStimulusGenerator sphericalStimulusGenerator;
    public SquareStimulusController squareStimulusController;
    public Image stimulusStateImage;
    public Text timeText;

    public void Reset() {
        startedNewFile = false;
        startNewFile = true;
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

            if (stimTrans != null) {
                _sw.WriteLine(
                    "t, x , y, z , scale_x, scale_y, scale_z, stimulusOn"
                );
            } else if (stimTrans1 != null) {
                _sw.WriteLine(
                    "t, x1, y1, z1, scale_x1, scale_y1, scale_z1, x2, y2, z2, scale_x2, scale_y2, scale_z2, stimulusOn"
                );

            } else {
                _sw.WriteLine(
                    "t, stimulusOn"
                );
            }

            startNewFile = false;
            startedNewFile = true;
        }
    }

    public void WriteData()
    {
        // write the time data and
        // x, y and z coordinates of the stimulus
        if (stimTrans != null) {
            _sw.WriteLine(
                "{0}, {1:o}, {2}, {3}, {4}, {5}, {6}, {7}, {8}",
                Time.time,
                System.DateTime.Now,
                stimTrans.position.x,
                stimTrans.position.y,
                stimTrans.position.z,
                stimTrans.localScale.x,
                stimTrans.localScale.y,
                stimTrans.localScale.z,
                syncSquareImg.enabled 
            );
        } else if (stimTrans1 != null) {
            _sw.WriteLine(
                "{0}, {1:o}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}",
                Time.time,
                System.DateTime.Now,
                stimTrans1.position.x,
                stimTrans1.position.y,
                stimTrans1.position.z,
                stimTrans1.localScale.x,
                stimTrans1.localScale.y,
                stimTrans1.localScale.z,
                stimTrans2.position.x,
                stimTrans2.position.y,
                stimTrans2.position.z,
                stimTrans2.localScale.x,
                stimTrans2.localScale.y,
                stimTrans2.localScale.z,
                syncSquareImg.enabled
            );
        } else {
            _sw.WriteLine(
                "{0}, {1:o}, {2}",
                Time.time,
                System.DateTime.Now,
                syncSquareImg.enabled
            );
        }
    }

    void setSyncSquareValues() {
        Color stimStateColor = Color.black;

        StimulusState stimState = StimulusState.Waiting;

        if (sphericalStimulusGenerator != null) {
            stimState = sphericalStimulusGenerator.stimulusState;
        } else if (squareStimulusController != null) {
            stimState = squareStimulusController.stimulusState;
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
}
