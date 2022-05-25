using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A controller for positioning camera positions.
// Can support up to 6 monitors in a cube-like configuration.
public class CameraMonitorController : MonoBehaviour
{
    public int frontDisplayNum = 0;
    public int rightDisplayNum = 1;
    public int backDisplayNum = 2;
    public int leftDisplayNum = 3;

    public AsymFrustum frontCam;
    public AsymFrustum rightCam;
    public AsymFrustum backCam;
    public AsymFrustum leftCam;
    public AsymFrustum upCam;
    public AsymFrustum downCam;

    public void SetupCams(float distFromOrigin, float monitorHeightOffset, Vector2 monitorDimensions, bool setColor, Color[] bgColor) {
        Vector3 newPos = new Vector3(0, monitorHeightOffset + (monitorDimensions.y/2), distFromOrigin);

        SetupCam(frontCam, newPos, monitorDimensions, frontDisplayNum, setColor, bgColor[0]);
        SetupCam(rightCam, newPos, monitorDimensions, rightDisplayNum, setColor, bgColor[1]);
        SetupCam(backCam, newPos, monitorDimensions, backDisplayNum, setColor, bgColor[2]);
        SetupCam(leftCam, newPos, monitorDimensions, leftDisplayNum, setColor, bgColor[3]);
    }

    void SetupCam(AsymFrustum cam, Vector3 newPos, Vector2 monitorDimensions, int targetDisplay, bool setColor, Color bgColor)  {
        if (cam != null) {
           cam.transform.GetChild(0).localPosition = newPos;
           cam.width = monitorDimensions.x;
           cam.height = monitorDimensions.y;
           Camera camera = cam.GetComponent<Camera>();
           camera.targetDisplay = targetDisplay;
           if (setColor) {
               camera.backgroundColor = bgColor;
           }
        }
    }
}
