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

    public int crabType = 0;
    public float crabSize = 1.9f;
    public Color sphereColour = Color.white;

    public GameObject[] crabTypes;

    public override void Reset()
    {
        transform.parent.localPosition = pos;
        burrowRenderer.material.color = burrowColour;

        if (crabType == 0)
        {
            crabTypes[0].SetActive(true);
            crabTypes[1].SetActive(false);
            crabTypes[0].transform.localScale = new Vector3(crabSize, crabSize, crabSize);
        }
        else
        {
            crabTypes[0].SetActive(false);
            crabTypes[1].SetActive(true);
            crabTypes[1].transform.localScale = new Vector3(crabSize, crabSize, crabSize);
            crabTypes[1].transform.GetChild(0).GetComponent<Renderer>().material.color = sphereColour;
        }

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
        transform.parent.parent.parent.position = new Vector3(
            transform.parent.parent.parent.position.x,
            -eyeHeight,
            transform.parent.parent.parent.position.z
        );
        anim["CrabWalkOutOfBurrow"].speed = Random.Range(0.75f, 1.25f);
        anim.Play("CrabWalkOutOfBurrow");
    }
}
