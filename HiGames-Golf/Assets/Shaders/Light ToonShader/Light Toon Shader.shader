﻿Shader "Golf/LightToonShader"
{
	Properties
	{
		_Color("Color", Color) = (0.5, 0.65, 1, 1)
		_AmbientColor("Ambient Color", Color) = (0.5,0.5,0.5,1)
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

			struct vertex
			{
				float4 vertex : POSITION;
				float4 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};
			struct fragment
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 worldNormal : NORMAL;
				LIGHTING_COORDS(0, 1)
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			fragment vert(vertex v)
			{
				fragment o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				TRANSFER_VERTEX_TO_FRAGMENT(o)
				return o;
			}

			float4 _Color;
			float4 _AmbientColor;

			float4 frag(fragment i) : COLOR
			{
				float3 normal = normalize(i.worldNormal);
				float NdotL = dot(_WorldSpaceLightPos0, normal);

				//Shadow Related
				float shadow = LIGHT_ATTENUATION(i);
				float lightIntensity = smoothstep(0, 0.01, NdotL * shadow);
				float4 light = lightIntensity * _LightColor0;

				return tex2D(_MainTex, i.uv) * _Color * (_AmbientColor + light) ;
			}
			ENDCG
		}
		UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
	}
	Fallback "VertexLit"
}