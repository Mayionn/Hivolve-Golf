using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class environment_stuff : MonoBehaviour
{
    Camera cam;
    RenderTexture rt;
    int cubemapSize = 256;
    Material mat;

    void Start()
    {
        mat = new Material(Shader.Find("mwanga/environment"));
    }

    void Update()
    {
        //if (!cam)
       //{
            GameObject c = new GameObject("cubemap_cam", typeof(Camera));
            c.hideFlags = HideFlags.HideAndDontSave;
            c.transform.position = transform.position;
            c.transform.rotation = Quaternion.identity;
            cam = c.GetComponent<Camera>();
            cam.farClipPlane = 100;
            cam.enabled = false;
        //}
        //if (!rt)
        //{
            rt = new RenderTexture(cubemapSize, cubemapSize, 16);
            rt.dimension = UnityEngine.Rendering.TextureDimension.Cube;
            rt.hideFlags = HideFlags.HideAndDontSave;
            //mat.SetTexture("_Cube", rt);
        //}

        cam.transform.position = transform.position;
        cam.RenderToCubemap(rt, 63);
        mat.SetTexture("_Cube", rt);
    }
}
