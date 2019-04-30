using Assets.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class Destination_Pusher
{
    public GameObject GO;
    [HideInInspector] public Vector3 Position;

    public void Init()
    {
        Position = GO.transform.position;
        GO.gameObject.SetActive(false);
    }
}

public class MM_Pusher : MonoBehaviour
{
    public GameObject MovingPiece;
    public float PushSpeed;
    public float WaitTimeBegin;
    public float WaitTimeEnd;
    public Destination_Pusher Begin;
    public Destination_Pusher End;

    private readonly float MINDISTANCE = 1f;
    private float timerBegin;
    private float timerEnd;
    private bool isPushing;
    private Vector3 dir;

    public void Start()
    {
        Begin.Init();
        End.Init();

        MovingPiece.transform.position = Begin.Position;

        timerBegin = 0;
        timerEnd = 0;
        isPushing = false;
        dir = Vector3.Normalize(End.Position - Begin.Position);
    }

    private void Update()
    {
        timerBegin += Time.deltaTime;
        if (timerBegin > WaitTimeBegin)
        {
            if (isPushing)
            {
                if (Vector3.Distance(MovingPiece.transform.position, End.Position) < MINDISTANCE)
                {
                    isPushing = false;
                }
                else
                {
                    MovingPiece.transform.position += (dir * PushSpeed * Time.deltaTime);
                    isPushing = true;
                }
            }
            else
            {
                timerEnd += Time.deltaTime;
                if(timerEnd > WaitTimeEnd)
                {
                    if (Vector3.Distance(MovingPiece.transform.position, Begin.Position) < MINDISTANCE)
                    {
                        isPushing = true;
                        timerBegin = 0;
                        timerEnd = 0;
                    }
                    else MovingPiece.transform.position += (-dir * PushSpeed * Time.deltaTime);
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            ContactPoint[] c = new ContactPoint[collision.contactCount];
            collision.GetContacts(c);
            Vector3 dir = Vector3.Normalize(c[0].point - collision.gameObject.transform.position); 
            Vector3 norm = Vector3.Normalize(c[0].normal);
            Vector3 reflect = Vector3.Normalize(Vector3.Reflect(dir, norm));

            collision.gameObject.GetComponent<Ball>().RigBody.AddForce(reflect * PushSpeed);
            collision.gameObject.GetComponent<Ball>().RigBody.AddForce(norm * (PushSpeed / 2), ForceMode.Impulse);
            //fazer qualquer coisa
            Debug.Log("Manteiga");
        }       
    }
}
