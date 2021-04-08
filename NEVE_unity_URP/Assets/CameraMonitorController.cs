using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A controller for positioning camera positions.
// Can support up to 6 monitors in a cube-like configuration.
public class CameraMonitorController : MonoBehaviour
{
    public AsymFrustum frontCam;
    public AsymFrustum rightCam;
    public AsymFrustum backCam;
    public AsymFrustum leftCam;
    public AsymFrustum upCam;
    public AsymFrustum downCam;

    public void SetupCams(float distFromOrigin, float monitorHeightOffset, Vector2 monitorDimensions, Color horizonSkyColor) {
        Vector3 newPos = new Vector3(0, monitorHeightOffset + (monitorDimensions.y/2), distFromOrigin);
        SetupCam(frontCam, newPos, monitorDimensions, horizonSkyColor);
        SetupCam(rightCam, newPos, monitorDimensions, horizonSkyColor);
        SetupCam(backCam, newPos, monitorDimensions, horizonSkyColor);
        SetupCam(leftCam, newPos, monitorDimensions, horizonSkyColor);
        SetupCam(upCam, newPos, monitorDimensions, horizonSkyColor);
        SetupCam(downCam, newPos, monitorDimensions, horizonSkyColor);
    }

    void SetupCam(AsymFrustum cam, Vector3 newPos, Vector2 monitorDimensions, Color horizonSkyColor) {
        if (cam != null) {
           cam.transform.GetChild(0).localPosition = newPos;
           cam.width = monitorDimensions.x;
           cam.height = monitorDimensions.y;
           cam.GetComponent<Camera>().backgroundColor = horizonSkyColor;
        }
    }
}
