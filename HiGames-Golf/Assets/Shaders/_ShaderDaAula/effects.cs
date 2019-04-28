using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effects : MonoBehaviour
{
    private Material mat;
    public int amount = 2;

    void Awake()
    {
        mat = new Material(Shader.Find("mwanga/effects"));
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        RenderTexture tmp = RenderTexture.GetTemporary(src.width, 
                                                        src.height);
        for (int i=0; i< amount; i++)
        {
            Graphics.Blit(src, tmp, mat);
            Graphics.Blit(tmp, src, mat);
        }
        Graphics.Blit(src, dest);
    }
    
}
