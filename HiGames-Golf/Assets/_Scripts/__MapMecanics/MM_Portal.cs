using Assets.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class Portal
{
    public GameObject GO;
    public GameObject Direction;
    [HideInInspector] public int Index;
    [HideInInspector] public Vector3 Position;
    [HideInInspector] public Vector3 OutDirection;
    [HideInInspector] public Vector3 DefaultDirection;

    public void Init(int index)
    {
        Index = index;
        Position = GO.transform.position;
        DefaultDirection = Vector3.Normalize(Direction.transform.position - GO.transform.position);
    }
}

public class MM_Portal : MonoBehaviour
{
    public Portal Portal1;
    public Portal Portal2;
    public bool isDefaultDirection;

    public void Start()
    {
        Portal1.Init(0);
        Portal2.Init(1);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(collision.contacts[0].thisCollider.gameObject == Portal1.GO)
            {
                Setup_TeleportTo(collision.gameObject.GetComponent<Ball>(), Portal2);
            }
            else
            {
                Setup_TeleportTo(collision.gameObject.GetComponent<Ball>(), Portal1);
            }
        }       
    }

    private void Setup_TeleportTo(Ball ball, Portal portal)
    {
        if (isDefaultDirection)
        {
            ball.transform.position = portal.Position + (Vector3.forward * (portal.GO.transform.localScale.x * 2f));
            ball.transform.position += Vector3.up * ball.SphereCollider.radius; //prevents ball from falling off the ground
            Vector3 vel = ball.RigBody.velocity;
            ball.RigBody.velocity = portal.DefaultDirection;
            ball.RigBody.AddForce(portal.DefaultDirection * vel.magnitude);
        }
        else
        {
            Debug.Log("Not Implemented Yet");
        }
    }
}
