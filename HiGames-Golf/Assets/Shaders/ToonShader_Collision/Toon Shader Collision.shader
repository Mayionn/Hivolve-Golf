Shader "Golf/ToonShader_Collision"
{
	Properties
	{
		//Ball Color
		_Color("Color", Color) = (0.5, 0.65, 1, 1)
		_SpecularColor("Specular Color", Color) = (0.9, 0.9, 0.9, 1)
		_Glossiness("Glossiness", Float) = 32
		//Collision Effect Properties
		_ContactColor("ContactColor", Color) = (1,1,1,1)
		_ImpactSize("ImpactSize", float) = 0.5
		//Texture
		_MainTex("Main Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags
		{
			"LightMode" = "ForwardBase"
			"PassFlags" = "OnlyDirectional"
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase /*Shadow's Related*/

			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			//Disable for more FPS
			#include "AutoLight.cginc"

			struct vert_in
			{
				float4 vertex : POSITION;
				float4 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};
			struct fragment
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
				float4 color : COLOR;
				float3 viewDir : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			//Ball color
			float4 _Color;
			float _Glossiness;
			float4 _SpecularColor;
			//ContactPoints
			float4 _ContactColor;
			float _ImpactSize;
			int _ContactPointsSize;
			float4 _ContactPoints[50];

			fragment vert(vert_in v)
			{
				fragment o;

				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.viewDir = WorldSpaceViewDir(v.vertex);

				//COLLISION EFFECT
				o.color = _Color;
				float4 vertexPos = mul(UNITY_MATRIX_M, v.vertex);
				//float4 vertexPos = mul(unity_WorldToObject, float4(v.vertex.xyz, 0));

				for (int i = 0; i < _ContactPointsSize; i++)
				{
					//float4 position = mul(UNITY_MATRIX_M, _ContactPoints[i]);
					//float4 position = mul(unity_WorldToObject, _ContactPoints[i]);

					//o.color += max(0, _ImpactSize - distance(position.xyz, vertexPos.xyz)) * _ContactColor;
					o.color += smoothstep(_ImpactSize - 0.01, 
											_ImpactSize + 0.01, 
											max(0, 1 - distance(_ContactPoints[i].xyz, vertexPos.xyz))) * _ContactColor;
				}

				return o;
			}

			float4 frag(fragment f) : COLOR
			{

				float3 normal = normalize(f.normal);
				float3 viewDir = normalize(f.viewDir);
				float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);
				float NdotH = dot(normal, halfVector);
				float specularIntensity = pow(NdotH, _Glossiness * _Glossiness);

				//Harden Specular lines
				float specularIntensitySmooth = smoothstep(0.005, 0.01, specularIntensity);
				float4 specular = specularIntensitySmooth * _SpecularColor;

				return tex2D(_MainTex, f.uv) * f.color + (specular);
			}
			ENDCG
		}
		UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
	}
}