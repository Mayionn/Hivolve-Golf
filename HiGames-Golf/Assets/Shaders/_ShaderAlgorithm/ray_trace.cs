using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ray_trace : MonoBehaviour
{
    
	public ComputeShader shader;
	public Texture skybox;
	public Light light;
	public Camera cam;

	//setup spheres
	public int numSpheres;
	public float placementRadius;
	public float sphereRadius;

	struct sphere
	{
		public float radius;
		public Vector3 center;
		public Vector3 color;
	};
	ComputeBuffer sphereBuffer;
	RenderTexture target;


	void Start()
	{
		target = new RenderTexture(Screen.width, Screen.height, 0, 
			RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
		target.enableRandomWrite = true;
		target.Create();

		createSpheres();
	}

	void createSpheres()
	{
		List<sphere> sp = new List<sphere>();

		for(int i=0; i < numSpheres; i++)
		{
			sphere s = new sphere();

			s.radius = sphereRadius + Random.Range(1-sphereRadius, sphereRadius*0.5f);
			Vector2 randomPos = Random.insideUnitCircle * placementRadius;
			s.center = new Vector3(	randomPos.x, s.radius, randomPos.y);
			Color c = Random.ColorHSV();
			s.color = new Vector3(c.r, c.g, c.b);
			sp.Add(s);
		}
		sphereBuffer = new ComputeBuffer(sp.Count, sizeof(float)*7);
		sphereBuffer.SetData(sp);
	}

	void renderScene(RenderTexture dst)
	{
		//setup
		int index = shader.FindKernel("CSMain");
		shader.SetTexture(index, "skybox", skybox);
		shader.SetMatrix("_CameraToWorld", cam.cameraToWorldMatrix);
		shader.SetMatrix("_CameraInverseProjection", cam.projectionMatrix.inverse);

		shader.SetVector("light", new Vector4(light.transform.forward.x,
											light.transform.forward.y,
											light.transform.forward.z,
											light.intensity));
		shader.SetBuffer(index, "spheres", sphereBuffer);
		shader.SetTexture(index, "Result", target);

		//dispatch
		shader.Dispatch(index, Screen.width/8, Screen.height/8, 1);

		Graphics.Blit(target, dst);
	}

	void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		renderScene(dst);
	}
}
