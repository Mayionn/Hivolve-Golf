Shader "Golf/ToonShader_StrongOutline"
{
	Properties
	{
		_Color("Color", Color) = (0.5, 0.65, 1, 1)
		_MainTex("Main Texture", 2D) = "white" {}

		_OutlineColor("Outline Color", Color) = (0,0,0,0)
		_OutlineWidth("OutLine Width", Range(1.0, 10.0)) = 1
	}
	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
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
			};

			struct frag_in
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
				float4 color : COLOR;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Color;
			float4 _OutlineColor;
			float _OutlineWidth;

			geo_in vert(vert_in i)
			{
				geo_in o = (geo_in)0;
				o.pos = UnityObjectToClipPos(i.pos);
				o.normal = UnityObjectToWorldNormal(i.normal);
				o.uv = i.uv;
				return o;
			}

			[maxvertexcount(3)]
			void geo(triangle geo_in i[3], inout TriangleStream<frag_in> triStream)
			{
				/*for (int o = 0; o < 3; o++)
				{
					frag_in f;
					f.pos = i[o].pos;
					f.normal = i[o].normal;
					f.uv = i[o].uv;
					f.color = _Color;
					triStream.Append(f);
				}
				triStream.RestartStrip();*/

				for (int t = 2; t >= 0; t--)
				{
					frag_in f;
					f.pos = i[t].pos;
					f.pos += float4(i[t].normal.xyz, 1.0);
					f.normal = -i[t].normal;
					f.uv = i[t].uv;
					f.color = _OutlineColor;
					triStream.Append(f);
				}

				triStream.RestartStrip();
			}
			
			float4 frag(frag_in i) : COLOR
			{
				return tex2D(_MainTex, i.uv) * i.color;
			}
			ENDCG
		}
	}
}