Shader "Golf_Objects/ColorShader"
{
	Properties
	{
		_diff("Cena difusa", Color) = (1,1,1,1)
		_spec("Cena especular", Color) = (1,1,1,1)
		_sh("Cena shiny", Range(0,32)) = 1
		_ShadowIntensity("Shadow Intensity", Range(0,1)) = 0

		_MainTex("Textura", 2D) = "white" {}
		_Scale("Scale", Range(0,3)) = 1 
	}
		SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			//Lighting
			uniform float4 _diff;
			uniform float4 _spec;
			uniform float _sh;
			uniform float _ShadowIntensity;
			//Texture
			uniform sampler2D _MainTex;
			uniform float _Scale;

			struct vertexIn
			{
				float4 pos : POSITION;
				float3 n : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct vertexOut
			{
				float4 pos : SV_POSITION;
				float4 color : COLOR;
				float2 uv : TEXCOORD0;
			};

			vertexOut vert(vertexIn i)
			{
				vertexOut o;

				//componente especular
				float3 cam = _WorldSpaceCameraPos.xyz;
				float3 pm = mul(UNITY_MATRIX_M, i.pos);
				float3 look = normalize(pm - cam);

				//componente difusa
				float3 ldir = normalize(_WorldSpaceLightPos0.xyz);
				float3 n = normalize(
								mul(float4(i.n,0),unity_WorldToObject).xyz
							);

				float3 r = reflect(ldir, n);


				float idiff = max(_ShadowIntensity,dot(n,ldir));
				float ispec = 0;
				if (idiff > 0)
				{
					ispec = pow(max(0, dot(r, look)), _sh);
				}
				o.pos = UnityObjectToClipPos(i.pos);
				o.uv = i.uv * _Scale;
				o.color = _diff * idiff + _spec * ispec;

				return o;
			}

			float4 frag(vertexOut pos) : COLOR
			{
				return tex2D(_MainTex, pos.uv) * pos.color;
			}
			ENDCG
		}
		UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
	}
}
