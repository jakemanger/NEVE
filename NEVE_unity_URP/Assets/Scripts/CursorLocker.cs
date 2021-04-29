using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorLocker : MonoBehaviour
{
    bool isLocked = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            if (isLocked) {
                Cursor.lockState = CursorLockMode.None;
            } else {
                Cursor.lockState = CursorLockMode.Locked;
            }
            isLocked = !isLocked;
        }

    }
}
