Shader "Golf/ToonShader_GeometryOutline"
{
	Properties
	{
		_Color("Color", Color) = (0.5, 0.65, 1, 1)
		_MainTex("Main Texture", 2D) = "white" {}

		_AmbientColor("Ambient Color", Color) = (0.4,0.4,0.4,1)
		_SpecularColor("Specular Color", Color) = (0.9, 0.9, 0.9, 1)
		_Glossiness("Glossiness", Float) = 32
		
		_OutlineColor("Outline Color", Color) = (0,0,0,0)
		_OutlineWidth("Outline Width", Range(0.0, 0.2)) = 0.1
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
			#pragma multi_compile_fwdbase

			#include "UnityCG.cginc"
			#include "Lighting.cginc"

			struct vert_in
			{
				float4 pos : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct geo_in
			{
				float4 pos : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
				float3 viewDir : TEXCOORD1;
			};

			struct frag_in
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
				float4 color : COLOR;
				float3 viewDir : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Color;
			float4 _OutlineColor;
			float _OutlineWidth;

			geo_in vert(vert_in i)
			{
				geo_in o = (geo_in)0;
				o.pos = mul(UNITY_MATRIX_M, i.pos);
				o.normal = mul(UNITY_MATRIX_M, i.normal);
				o.uv = TRANSFORM_TEX(i.uv, _MainTex);
				o.viewDir = WorldSpaceViewDir(i.pos);
				return o;
			}

			[maxvertexcount(6)]
			void geo(triangle geo_in i[3], inout TriangleStream<frag_in> triStream)
			{
				for (int o = 0; o < 3; o++)
				{
					frag_in f;
					f.pos = mul(UNITY_MATRIX_VP,i[o].pos);
					f.normal = i[o].normal;
					f.uv = i[o].uv;
					f.color = _Color;
					f.viewDir = i[o].viewDir;
					triStream.Append(f);
				}
				triStream.RestartStrip();

				for (int t = 2; t >= 0; t--)
				{
					frag_in f;
					f.pos = i[t].pos + float4(normalize(i[t].normal.xyz) * _OutlineWidth, 0.0);
					f.pos = mul(UNITY_MATRIX_VP, f.pos);
					f.normal = -i[t].normal;
					f.uv = i[t].uv;
					f.color = _OutlineColor;
					f.viewDir = i[t].viewDir;
					triStream.Append(f);
				}
				triStream.RestartStrip();
			}
			
			float4 _AmbientColor;
			float4 _SpecularColor;
			float _Glossiness;

			float4 frag(frag_in i) : COLOR
			{
				float3 normal = normalize(i.normal);
				float NdotL = dot(_WorldSpaceLightPos0, normal);

				float lightIntensity = smoothstep(0, 0.01, NdotL);
				float4 light = lightIntensity * _LightColor0;

				float3 viewDir = normalize(i.viewDir);
				float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);
				float NdotH = dot(normal, halfVector);
				float specularIntensity = pow(NdotH * lightIntensity, _Glossiness * _Glossiness);

				//Harden Specular lines
				float specularIntensitySmooth = smoothstep(0.005, 0.01, specularIntensity);
				float4 specular = specularIntensitySmooth * _SpecularColor;

				//Shadow Related


				return tex2D(_MainTex, i.uv) * i.color * (_AmbientColor + light + specular);
			}
			ENDCG
		}
		UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
	}
}