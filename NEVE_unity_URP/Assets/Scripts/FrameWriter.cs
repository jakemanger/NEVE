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

    void OnEnable() {
        if (outputFilePath == null || outputFilePath == "")
           Debug.LogError("FrameWriter outputFilePath was not specified.");

        outputFilePath = experimentId + "_" + System.DateTime.UtcNow.ToString("HHmmssddMMMMyyyy") + ".csv";

        _sw = System.IO.File.AppendText(outputFilePath);
        if (!recordEachFrame)
            InvokeRepeating("WriteData", 0, 1/recordingFrequency);
    }

    // Update is called once per frame
    void Update()
    {
        if (recordEachFrame) 
            WriteData();
    }

    public void WriteData()
    {
        // write the time data and
        // x, y and z coordinates of the stimulus
        _sw.WriteLine(
            "t {0}, x {1}, y {2}, z {3}, stimulusOn {4}",
            Time.time,
            stimTrans.position.x,
            stimTrans.position.y,
            stimTrans.position.z,
            stimRenderer.enabled
        );
    }
}
