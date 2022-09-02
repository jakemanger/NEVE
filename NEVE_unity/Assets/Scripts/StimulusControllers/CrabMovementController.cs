using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CrabMovementController : GenericStimulusController
{
    public Vector3 pos = new Vector3(0, 0, 50f);
    public float eyeHeight = 2f;

    float speed = 0.1f;

    bool canPlay = false;
    public Color burrowColour = Color.grey;

    Animation anim;
    public Renderer  burrowRenderer;

    public override void Reset()
    {
        transform.parent.localPosition = pos;
        burrowRenderer.material.color = burrowColour;

        base.Reset();
    }

    void Start()
    {
        anim = gameObject.GetComponent<Animation>();
    }

    void Update() {
        if (anim.isPlaying) {
            return;
        }
        print("Starting crab animation");
        // set random rotation and speed
        transform.parent.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);
        transform.parent.parent.parent.position = new Vector3(transform.parent.parent.parent.position.x, -eyeHeight + 0.48f, transform.parent.parent.parent.position.z);
        anim["CrabWalkOutOfBurrow"].speed = Random.Range(0.75f, 1.25f);
        anim.Play("CrabWalkOutOfBurrow");
    }
}
