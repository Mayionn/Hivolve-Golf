using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeterministicPhysics : MonoBehaviour
{
    public float step;

    //---Deterministic Physics
    void FixedUpdate()
    {
        Physics.autoSimulation = false;
        Physics.Simulate(step);
    }
}
