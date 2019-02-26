using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
        [HideInInspector] public GameManager GameManager;
        [HideInInspector] public List<State> ConnectedStates;
        public abstract void StartState();
        public abstract void OnState();
        public abstract void CheckState();
        public abstract void LeaveState(State state);
}
