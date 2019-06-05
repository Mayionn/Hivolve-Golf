Shader "Golf_Objects/VoidBall"
{
	Properties
	{
		_MainTex("Textura", 2D) = "white" {}
		_Scale("Scale", Range(0,3)) = 1
		_PulsatingSpeed("Pulsating Speed", Range(5.0, 20.0)) = 10
		_PulsatingRange("Pulsating Range", Range(0.0, 3.0)) = 1
	}
		SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			//Texture
			uniform sampler2D _MainTex;
			uniform float _Scale;
			//Effect
			uniform float _PulsatingSpeed;
			uniform float _PulsatingRange;

			struct vertexIn
			{
				float4 pos : POSITION;
				float3 normal : NORMAL;
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
				o.pos = UnityObjectToClipPos(i.pos);
				o.uv = i.uv * _Scale;

				//Effect
				float time = _Time.x * _PulsatingSpeed * _PulsatingSpeed;
				float wave = cos(time) + sin(time) + 2.5;
				float intensity = _PulsatingRange / wave;
				o.color = float4(1,1,1,1) * intensity;

				//float3 ldir = normalize(_WorldSpaceLightPos0.xyz);
				//float3 n = normalize(
				//				mul(float4(i.normal,0),unity_WorldToObject).xyz
				//			);

				//float3 r = reflect(ldir, n);
				//float time = _Time.x * _PulsatingSpeed;
				//float wave = cos(time) + sin(time) + 2.5;
				//float idiff = max(1, dot(n,ldir)) * wave;
				//float ispec = 0;
				//if (idiff > 0)
				//{
				//	ispec = pow(max(0, dot(r, look)), _sh);
				//}


				return o;
			}

			float4 frag(vertexOut i) : COLOR
			{
				return tex2D(_MainTex, i.uv) * i.color;
			}
			ENDCG
		}
		UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
	}
}
