using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum StimulusState {Waiting, Started, Ended}

public abstract class GenericStimulusController : MonoBehaviour
{
    public StimulusState stimulusState = StimulusState.Waiting;

    public virtual void Reset() {
        stimulusState = StimulusState.Waiting;
    } 
}