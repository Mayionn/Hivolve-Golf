using Assets.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMechanic_Pusher : MonoBehaviour
{
    public MapMechanic_Pusher GO;
    public GameObject End;
    public GameObject Start;
    public float Speed;

    public bool Stops;
    public float WaitTime;

    private bool isStartTheDestination;
    private Vector3 destination;
    private float maxDistance;
    private readonly float MINDISTANCE = 0.1f;

    public void Init()
    {
        isStartTheDestination = false;
        destination = End.transform.position;
        maxDistance = Vector3.Distance(End.transform.position, Start.transform.position);
        //movingPiece = Instantiate(Prefab);
        //movingPiece.transform.parent = Start; 
        //movingPiece.transform.position = Start.position;
        End.gameObject.SetActive(false);
        Start.gameObject.SetActive(false);

        GameManager.Instance.ActUpdate += Update_MiddlePosition;
    }
    
    private void Update_MiddlePosition()
    {
        Vector3 dir = Vector3.Normalize(destination - transform.position);
        transform.position += (dir * Speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, destination) < MINDISTANCE)
        {
            if (!isStartTheDestination)
            {
                destination = Start.transform.position;
                isStartTheDestination = true;
            }
            else
            {
                destination = End.transform.position;
                isStartTheDestination = false;
            }
        }


        ////Prevent platform going out of place---------------------------------------------------------//
        //if(Vector3.Distance(transform.position, destination) > maxDistance + MINDISTANCE * 5)
        //{
        //    transform.position = destination;
        //}
    }
}
