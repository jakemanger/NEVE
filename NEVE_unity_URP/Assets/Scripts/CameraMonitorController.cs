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

    public void SetupCams(float distFromOrigin, float monitorHeightOffset, Vector2 monitorDimensions, Color horizonSkyColor) {
        Vector3 newPos = new Vector3(0, monitorHeightOffset + (monitorDimensions.y/2), distFromOrigin);
        SetupCam(frontCam, newPos, monitorDimensions, horizonSkyColor, frontDisplayNum);
        SetupCam(rightCam, newPos, monitorDimensions, horizonSkyColor, rightDisplayNum);
        SetupCam(backCam, newPos, monitorDimensions, horizonSkyColor, backDisplayNum);
        SetupCam(leftCam, newPos, monitorDimensions, horizonSkyColor, leftDisplayNum);
    }

    void SetupCam(AsymFrustum cam, Vector3 newPos, Vector2 monitorDimensions, Color horizonSkyColor, int targetDisplay) {
        if (cam != null) {
           cam.transform.GetChild(0).localPosition = newPos;
           cam.width = monitorDimensions.x;
           cam.height = monitorDimensions.y;
           Camera camera = cam.GetComponent<Camera>();
           camera.targetDisplay = targetDisplay;
           camera.backgroundColor = horizonSkyColor;
        }
    }
}
