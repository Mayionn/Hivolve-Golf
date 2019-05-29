using Assets.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearPlayerView : MonoBehaviour
{
    public Camera MainCamera;
    private RaycastHit oldHit;

    void FixedUpdate()
    {
        XRay();
    }

    private void XRay()
    {
        Vector3 playerPosition = GameManager.Instance.CurrentPlayer.SelectedBall.transform.position;
        Vector3 cameraPosition = MainCamera.transform.position;
        Vector3 fwd = playerPosition - cameraPosition;
        float characterDistance = Vector3.Distance(cameraPosition, playerPosition);

        Debug.DrawRay(cameraPosition, fwd, Color.red);
        Vector3 forward = playerPosition - cameraPosition;
        forward.y = 0;
        Debug.DrawRay(playerPosition, forward, Color.red);

        if (Physics.Raycast(playerPosition, forward , out RaycastHit hit, characterDistance))
        {
            if (oldHit.transform)
            {
                // Add transparence
                Color colorA = oldHit.transform.gameObject.GetComponent<MeshRenderer>().material.color;
                colorA.a = 1f;
                oldHit.transform.gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", colorA);
            }

            // Add transparence
            if(hit.transform.GetComponent<MeshRenderer>())
            {
                Color colorB = hit.transform.gameObject.GetComponent<MeshRenderer>().material.color;
                colorB.a = 0.5f;
                hit.transform.gameObject.GetComponent<MeshRenderer>().material.SetColor("Color", colorB);
            }

            // Save hit
            oldHit = hit;
        }
    }
}
