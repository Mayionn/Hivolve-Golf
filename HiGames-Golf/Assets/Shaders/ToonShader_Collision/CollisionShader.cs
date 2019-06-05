using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CollisionShader : MonoBehaviour
{
    public Vector4[] ContactPoints;
    public Material shaderMaterial;

    private void OnCollisionStay(Collision collision)
    {
        ContactPoints = new Vector4[collision.contacts.Length];

        for (int i = 0; i < collision.contacts.Length; i++)
        {
            ContactPoints[i] = (Vector4)collision.GetContact(i).point;
        }

        shaderMaterial.SetInt("_ContactPointsSize", ContactPoints.Length);
        shaderMaterial.SetVectorArray("_ContactPoints", ContactPoints);
    }

    private void OnCollisionExit(Collision collision)
    {
        if(ContactPoints.Length == 1)
        {
            ContactPoints = new Vector4[1];
            ContactPoints[0] = new Vector4(0, 0, 0, 0);
            shaderMaterial.SetInt("_ContactPointsSize", ContactPoints.Length);
            shaderMaterial.SetVectorArray("_ContactPoints", ContactPoints);
        }
    }
}
