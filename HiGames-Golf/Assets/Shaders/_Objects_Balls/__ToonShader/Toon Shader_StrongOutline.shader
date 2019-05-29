Shader "Golf/ToonShader_StrongOutline"
{
	Properties
	{
		_Color("Color", Color) = (0.5, 0.65, 1, 1)
		_AmbientColor("Ambient Color", Color) = (0.4,0.4,0.4,1)
		_SpecularColor("Specular Color", Color) = (0.9, 0.9, 0.9, 1)
		_Glossiness("Glossiness", Float) = 32
		[HDR]
		_RimColor("Rim Color", Color) = (1,1,1,1)
		_RimAmount("Rim Amount", Range(0,1)) = 0.716
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
			#pragma geometry geo

			#include "UnityCG.cginc"
			#include "Lighting.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float4 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct geo_in
			{
				float4 pos : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
				float3 viewDir: TEXCOORD1;
			};
			
			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 worldNormal : NORMAL;
				float3 viewDir : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			geo_in vert(appdata i)
			{
				geo_in o = (geo_in)0;
				o.pos = i.vertex;
				o.normal = i.normal;
				o.uv = i.uv;
				o.viewDir = WorldSpaceViewDir(i.vertex);

				return o;
			}

			/*[maxvertexcount(6)]
			void geo(triangle geo_in i[3], inout TriangleStream<v2f> triStream)
			{
				for (int o = 0; o < 3; o++)
				{
					v2f f;
					f.pos = i[o].pos;
					f.worldNormal = i[o].normal;
					f.viewDir = i[o].viewDir;
					f.uv = i[o].uv;
					triStream.Append(f);
				}

				triStream.RestartStrip();

				for (int t = 2; t >= 0; t--)
				{
					v2f f;
					f.pos = i[t].pos;
					f.pos += float4(i[t].normal, 0.0);
					f.worldNormal = -i[t].normal;
					f.viewDir = i[t].viewDir;
					f.uv = i[t].uv;
					triStream.Append(f);
				}

				triStream.RestartStrip();
			}*/

			float _Glossiness;
			float _RimAmount;
			float4 _Color;
			float4 _AmbientColor;
			float4 _SpecularColor;
			float4 _RimColor;

			float4 frag(geo_in i) : SV_Target
			{
				//float3 normal = normalize(i.normal);
				//float NdotL = dot(_WorldSpaceLightPos0, normal);
				//float lightIntensity = smoothstep(0, 0.01, NdotL);
				//float4 light = lightIntensity * _LightColor0;

				//float3 viewDir = normalize(i.viewDir);
				//float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);
				//float NdotH = dot(normal, halfVector);
				//float specularIntensity = pow(NdotH * lightIntensity, _Glossiness * _Glossiness);

				////Harden Specular lines
				//float specularIntensitySmooth = smoothstep(0.005, 0.01, specularIntensity);
				//float4 specular = specularIntensitySmooth * _SpecularColor;

				
				float4 sample = tex2D(_MainTex, i.uv);
			
				return _Color;// *sample* (_AmbientColor* light);
			}
			ENDCG
		}
	}
}