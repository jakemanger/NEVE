using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors; 

public class RollerAgent : Agent {
    Rigidbody rBody;

    public Transform Target;
    public float forceMultiplier = 10;

    public bool manualInput = false;

    void Start() {
       rBody = GetComponent<Rigidbody>(); 
    }

    public override void OnEpisodeBegin() {
        // used for initialising and resetting the environment
        if (this.transform.localPosition.y < 0) {
            // If the Agent fell, zero its momentum
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3(0, 0.5f, 0);
        }

        // Move the target to a new spot
        Target.localPosition = new Vector3(Random.value * 8 - 4,
                                           0.5f,
                                           Random.value * 8 - 4);

    }

    public override void CollectObservations(VectorSensor sensor) {
        // creates a feature vector to send to the neural network
        // Target and Agent positions
        sensor.AddObservation(Target.localPosition);
        sensor.AddObservation(this.transform.localPosition);

        // Agent velocity
        sensor.AddObservation(rBody.velocity.x);
        sensor.AddObservation(rBody.velocity.z);
    }

    public override void OnActionReceived(float[] vectorAction) {
        // receives actions, does action, assigns rewards
        // and checks if episode is done (fell off platform)

        // Actions, size = 2
        // vectorAction[0] = force applied along the x-axis
        // vectorAction[1] = force applied along the z-axis

        // do action
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[0];
        controlSignal.z = vectorAction[1];
        rBody.AddForce(controlSignal * forceMultiplier);

        // calculate reward
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);

        // Reached target
        if (distanceToTarget < 1.42f)
        {
            SetReward(1.0f);
            EndEpisode();
        }

        // Fell off platform
        else if (this.transform.localPosition.y < 0)
        {
            EndEpisode();
        }
    }

    public override void Heuristic(float[] actionsOut) {
        // extend the heuristic method to manually change
        // input axes for testing
        if (manualInput) {
            actionsOut[0] = Input.GetAxis("Horizontal");
            actionsOut[1] = Input.GetAxis("Vertical");
        }
    }
}
