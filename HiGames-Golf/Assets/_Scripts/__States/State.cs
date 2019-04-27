using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Managers;

public abstract class State : MonoBehaviour
{
    [HideInInspector] public Ball Ball;
    [HideInInspector] public GameManager GameManager;
    [HideInInspector] public State[] ConnectedStates;
    public abstract void StartState();
    public abstract void OnState();
    public abstract void CheckState();
    public abstract void LeaveState(State state);
}