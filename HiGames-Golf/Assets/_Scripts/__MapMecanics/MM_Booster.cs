using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MM_Booster : MonoBehaviour
{
    public Transform pos1;
    public Transform pos2;
    public float BoostForce;

    private Vector3 _direction;

    void Start()
    {
        _direction = Vector3.Normalize(pos2.position - pos1.position);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<Ball>().RigBody.AddForce(_direction * BoostForce, ForceMode.VelocityChange);
        }
    }
}
