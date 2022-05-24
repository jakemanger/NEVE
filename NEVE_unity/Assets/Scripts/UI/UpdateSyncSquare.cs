using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateSyncSquare : MonoBehaviour
{
    public Image image;
    int index = 0;

    // Update is called once per frame
    void Update()
    {
        if (index == 0) {
            image.color = Color.black;
            index = 1;
        } else {
            image.color = Color.white;
            index = 0;
        }
    }
}
