using UnityEngine;
using System.Collections;

public class ActivateAllDisplays : MonoBehaviour {
    public int nDisplaysWanted = 4;
    void Start () {
        Debug.Log ("displays connected: " + Display.displays.Length);
    
        // for (int i = 1; i < Display.displays.Length; i++)
        int nDisplays = Mathf.Min(nDisplaysWanted, Display.displays.Length);
        Debug.Log ("displays connected: " + Display.displays.Length);
        for (int i = 1; i < nDisplays; i++) {
            // Display.displays[0] is the primary, default display and is always ON, so start at index 1.
            // Check if additional displays are available and activate each.
            Display.displays[i].Activate();
        }
    }
}
