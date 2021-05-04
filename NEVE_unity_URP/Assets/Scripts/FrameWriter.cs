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
        if (stimTrans != null) {
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
        } else if (stimTrans1 != null) {
            _sw.WriteLine(
                "t {0}, x1 {1}, y1 {2}, z1 {3}, scale_x1 {4}, scale_y1 {5}, scale_z1 {6}, x2 {7}, y2 {8}, z2 {9}, scale_x2 {10}, scale_y2 {11}, scale_z2 {12}, stimulusOn {13}",
                Time.time,
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
                "t {0}, stimulusOn {1}",
                Time.time,
                syncSquareImg.enabled
            );
        }
    }
}
