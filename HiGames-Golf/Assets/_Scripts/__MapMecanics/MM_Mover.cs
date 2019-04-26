using Assets.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class Destination
{
    public GameObject GO;
    [HideInInspector] public Vector3 Position;
    [HideInInspector] public int Index;
    public bool Stops;
    public float WaitTime;

    public void Init(int index)
    {
        Position = GO.transform.position;
        GO.gameObject.SetActive(false);
        Index = index;
    }
}

public class MM_Mover : MonoBehaviour
{
    public GameObject MovingPiece;
    public float Speed;
    public List<Destination> Destinations;
    private Destination targetDest;

    private readonly float MINDISTANCE = 1f;
    private float timer;

    public void Start()
    {
        for (int i = 0; i < Destinations.Count; i++)
        {
            Destinations[i].Init(i);
        }
        MovingPiece.transform.position = Destinations[0].Position;
        targetDest = Destinations[1];

        timer = 0;
    }
    
    private void Update()
    {
        Vector3 dir = Vector3.Normalize(targetDest.Position - MovingPiece.transform.position);

        if (Vector3.Distance(MovingPiece.transform.position, targetDest.Position) < MINDISTANCE)
        {
            if (targetDest.Stops)
            {
                timer += Time.deltaTime;
                if (timer > targetDest.WaitTime)
                {
                    timer = 0;
                    Setup_NextDestination();
                }
            }
            else Setup_NextDestination();
        }
        else MovingPiece.transform.position += (dir * Speed * Time.deltaTime);
    }

    private void Setup_NextDestination()
    {
        if(targetDest.Index + 1 < Destinations.Count)
        {
            targetDest = Destinations[targetDest.Index + 1];
        }
        else
        {
            targetDest = Destinations[0];
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

            collision.gameObject.GetComponent<Ball>().RigBody.AddForce(reflect * Speed);
            collision.gameObject.GetComponent<Ball>().RigBody.AddForce(norm * Speed / 2, ForceMode.Impulse);
            //fazer qualquer coisa
            Debug.Log("Manteiga");
        }       
    }
}
