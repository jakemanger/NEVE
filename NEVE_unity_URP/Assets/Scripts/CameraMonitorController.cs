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

    public void SetupCams(float distFromOrigin, float monitorHeightOffset, Vector2 monitorDimensions, bool setColor, Color horizonSkyColor) {
        Vector3 newPos = new Vector3(0, monitorHeightOffset + (monitorDimensions.y/2), distFromOrigin);
        SetupCam(frontCam, newPos, monitorDimensions, frontDisplayNum, setColor, horizonSkyColor);
        SetupCam(rightCam, newPos, monitorDimensions, rightDisplayNum, setColor, horizonSkyColor);
        SetupCam(backCam, newPos, monitorDimensions, backDisplayNum, setColor, horizonSkyColor);
        SetupCam(leftCam, newPos, monitorDimensions, leftDisplayNum, setColor, horizonSkyColor);
    }

    void SetupCam(AsymFrustum cam, Vector3 newPos, Vector2 monitorDimensions, int targetDisplay, bool setColor, Color horizonSkyColor)  {
        if (cam != null) {
           cam.transform.GetChild(0).localPosition = newPos;
           cam.width = monitorDimensions.x;
           cam.height = monitorDimensions.y;
           Camera camera = cam.GetComponent<Camera>();
           camera.targetDisplay = targetDisplay;
           if (setColor) {
               camera.backgroundColor = horizonSkyColor;
           }
        }
    }
}
