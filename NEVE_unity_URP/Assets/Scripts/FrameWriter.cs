using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

// a class to control the writing of information to file
// about the simulation each frame
public class FrameWriter : MonoBehaviour
{
    public bool recordEachFrame = true;

    // only used if recordEachFrame is false
    public float recordingFrequency = 1f; // in seconds

    public Transform stimTrans;
    public Renderer stimRenderer;

    public string experimentId = "test_";
    public string outputFilePath;
    StreamWriter _sw;

    bool startNewFile = false;
    bool startedNewFile = false;

    public void Reset() {
        startedNewFile = false;
        startNewFile = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (startedNewFile && recordEachFrame) 
            WriteData();

        if (startNewFile) {
            outputFilePath = experimentId + "_" + System.DateTime.UtcNow.ToString("HHmmssddMMMMyyyy") + ".csv";

            if (outputFilePath == null || outputFilePath == "") {
                Debug.LogError("FrameWriter outputFilePath was not specified.");
            }

            _sw = System.IO.File.AppendText(outputFilePath);
            if (!recordEachFrame) {
                InvokeRepeating("WriteData", 0, 1/recordingFrequency);
            }
            startNewFile = false;
            startedNewFile = true;
        }
    }

    public void WriteData()
    {
        // write the time data and
        // x, y and z coordinates of the stimulus
        _sw.WriteLine(
            "t {0}, x {1}, y {2}, z {3}, scale_x {4}, scale_y {5}, scale_z {6}, stimulusOn {7}",
            Time.time,
            stimTrans.position.x,
            stimTrans.position.y,
            stimTrans.position.z,
            stimTrans.localScale.x,
            stimTrans.localScale.y,
            stimTrans.localScale.z,
            stimRenderer.enabled
        );
    }
}
