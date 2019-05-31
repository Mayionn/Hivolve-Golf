using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Managers;
using static Enums;

public class Ball : MonoBehaviour
{
    public Player Player;
    public Vector3 StartingPosition;
    public Vector3 LastPosition;
    [HideInInspector] public Rigidbody RigBody;
    [HideInInspector] public SphereCollider SphereCollider;

    public void Init()
    {
        RigBody = this.GetComponent<Rigidbody>();
        SphereCollider = this.GetComponent<SphereCollider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Waypoint")
        {
            List<int> rp = other.GetComponent<Waypoint>().ReachedPlayers;
            if(!rp.Contains(Player.PlayerNum))
            {
                rp.Add(Player.PlayerNum);
                SetupWaypoint(other.GetComponent<Waypoint>());
            }
        }
        else if(other.tag == "Hole")
        {
            switch (GameManager.Instance._GameMode)
            {
                case GameMode.Menu:
                    switch (other.name)
                    {
                        case "Hole-Singleplayer":
                            {
                                UiManager.Instance.OpenInterface_MapSelector();
                            }
                            break;
                        case "Hole-Multiplayer":
                            {

                            }
                            break;
                        case "Hole-LocalGame" :
                            {
                                //Open LocalMultiplayer Interface
                                Player.EndedMap = false;
                                UiManager.Instance.CloseInterface_InGameHud();
                                UiManager.Instance.OpenInterface_LocalMultiplayer();
                            }
                            break;
                        default:
                            Debug.Log("Hole not defined!");
                            break;
                    }
                    break;
                case GameMode.Singleplayer:
                    {
                        //TODO: Test remove truancteTimer();
                        Player.TruncateTimer();
                        UiManager.Instance.OpenInterface_CompletedMap();
                    }
                    break;
                case GameMode.Multiplayer:
                    break;
                case GameMode.Localgame:
                    {
                        if (!Player.EndedMap) //Prevent double in hole
                        {
                            Player.EndedMap = true;
                            //this.gameObject.GetComponent<MeshRenderer>().enabled = false;
                            UiManager.Instance.Update_ScoreBoard_SaveScore(Player);
                            GameManager.Instance.NextPlayer();
                        }

                    }
                    break;
                default:
                    break;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "OOB")
        {
            if(GameManager.Instance._GameMode == GameMode.Singleplayer)
            {
                GoStartingPosition(true);
            }
            else
            {
                GoLastPosition(true);
            }
        }
    }

    public void GoLastPosition(bool grounded)
    {
        StopBall();
        if (grounded) SetBallGrounded(LastPosition);
        else transform.position = LastPosition;
    }
    public void GoStartingPosition(bool grounded)
    {
        StopBall();
        if (grounded) SetBallGrounded(StartingPosition);
        else transform.position = StartingPosition;
    }
    public void SaveLastPosition()
    {
        LastPosition = StartingPosition;
    }
    public void StopAtPosition(bool grounded, Vector3 position)
    {
        StopBall();
        if (grounded) SetBallGrounded(position);
        else transform.position = position;
    }
    
    public void StopBall()
    {
        RigBody.Sleep();
        RigBody.velocity = Vector3.zero;
        RigBody.Sleep();
    }
    private void SetBallGrounded(Vector3 pos)
    {
        Ray r = new Ray(pos, -Vector3.up * 10);
        RaycastHit[] hits = Physics.RaycastAll(r, 10);

        foreach (RaycastHit hit in hits)
        {
            if(hit.collider.tag == "Untagged")
            {
                transform.position = hit.point + (Vector3.up * SphereCollider.radius * transform.localScale.x);
                break;
            }
        }
    }
    
    private void SetupWaypoint(Waypoint wp)
    {
         //Move Ball
         StopAtPosition(true, wp.Position);
         LastPosition = wp.Position;
         //Leave to State_LaunchBall
         GameManager.Instance.CurrentState.LeaveState(GameManager.Instance.CurrentState.ConnectedStates[0]);
         //Update Waypoint - UI
         UiManager.Instance.UpdateMapInfoWaypoints();
    }
}
