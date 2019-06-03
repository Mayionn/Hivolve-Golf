﻿Shader "tp2/alg1"
{
	Properties
	{
		_T ("a", 2D) = "white" {}
		_hoja("b", 2D) = "white" {}
		_Size("c", Range(1,96)) = 1
		_l("d", Vector) = (1,1,1,0)
		_p("g", Vector) = (1,1,1,0)
		_umbali("h", Range(0,5)) = 1
	}
	SubShader
	{

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
			};

			uniform sampler2D _T;
			uniform float _Size;
			uniform Texture2D _hoja;
			uniform SamplerState sampler_hoja;
			uniform float3 _l;
			uniform float3 _p;
			uniform float _umbali;

			geo_in vert (vert_in i)
			{
				geo_in o = (geo_in)0;
				o.pos = i.pos;
				o.normal = i.normal;
				o.uv = i.uv;
				return o;
			}

			[maxvertexcount(21)]
			void geo(triangle geo_in i[3], inout TriangleStream<frag_in> triStream)
			{

				float d1 = length(i[0].pos.xyz - i[1].pos.xyz);
				float d2 = length(i[0].pos.xyz - i[2].pos.xyz);
				float d3 = length(i[1].pos.xyz - i[2].pos.xyz);
				float3 n, pp;
				float2 uv;
				if (d1 > d2) 
				{
					if (d1 > d3) 
					{
						n = normalize((i[0].normal + i[1].normal) / 2);
						uv = (i[0].uv + i[1].uv) / 2;
						pp = (i[0].pos.xyz + i[1].pos.xyz) * 0.5;
					}
					else 
					{
						n = normalize((i[2].normal + i[1].normal) / 2);
						uv = (i[2].uv + i[1].uv) / 2;
						pp = (i[2].pos.xyz + i[1].pos.xyz) * 0.5;
					}
				}
				else 
				{
					if (d2 > d3) 
					{
						n = normalize((i[0].normal + i[2].normal) / 2);
						uv = (i[0].uv + i[2].uv) / 2;
						pp = (i[0].pos.xyz + i[2].pos.xyz) * 0.5;
					}
					else 
					{
						n = normalize((i[2].normal + i[1].normal) / 2);
						uv = (i[2].uv + i[1].uv) / 2;
						pp = (i[2].pos.xyz + i[1].pos.xyz) * 0.5;
					}
				}

				float disp;
				float dd = length(pp - _p);
				float ddc = length(pp - float3(0,0,0));
				if ( dd < _umbali)
				{
					disp = 0;
				}
				else
				{
					disp = _hoja.SampleLevel(sampler_hoja, uv, 0).r *
								abs(cos(_Time.x)) * max(min(abs(cos(ddc*_Time.x*10))*2, 5.5), 0.0);
				}

				float4 newp[4];
				newp[0] = float4(i[0].pos.xyz + n * disp, 1.0);
				newp[1] = float4(i[1].pos.xyz + n * disp, 1.0);
				newp[2] = float4(i[2].pos.xyz + n * disp, 1.0);

				frag_in o;
				float3 newN;
				for (uint it=0; it<3; it++) 
				{
					newN = cross(	normalize(newp[it] - i[it].pos),
									normalize(i[(it + 1) % 3].pos - i[it].pos)
								);
					o.pos = UnityObjectToClipPos(i[it].pos);
					o.uv = i[it].uv;
					o.normal = newN;
					triStream.Append(o);
					o.pos = UnityObjectToClipPos(i[(it + 1) % 3].pos);
					o.uv = i[(it + 1) % 3].uv;
					o.normal = newN;
					triStream.Append(o);
					o.pos = UnityObjectToClipPos(newp[it]);
					o.uv = i[it].uv;
					o.normal = newN;
					triStream.Append(o);
					triStream.RestartStrip();
					

					newN = cross(	normalize(newp[it] - newp[(it + 1) % 3]),	
									normalize(i[(it + 1) % 3].pos - newp[(it + 1) % 3])
								);
					o.pos = UnityObjectToClipPos(newp[it]);
					o.uv = i[it].uv;
					o.normal = newN;
					triStream.Append(o);
					o.pos = UnityObjectToClipPos(i[(it + 1) % 3].pos);
					o.uv = i[(it + 1) % 3].uv;
					o.normal = newN;
					triStream.Append(o);
					o.pos = UnityObjectToClipPos(newp[(it + 1) % 3]);
					o.uv = i[(it + 1) % 3].uv;
					o.normal = newN;
					triStream.Append(o);
					triStream.RestartStrip();
				}
				
				o.pos = UnityObjectToClipPos(newp[0]);
				o.uv = i[0].uv;
				o.normal = n;
				triStream.Append(o);

				o.pos = UnityObjectToClipPos(newp[1]);
				o.uv = i[1].uv;
				o.normal = n;
				triStream.Append(o);

				o.pos = UnityObjectToClipPos(newp[2]);
				o.uv = i[2].uv;
				o.normal = n;
				triStream.Append(o);

				triStream.RestartStrip();

			}
			
			float4 frag (frag_in i) : COLOR
			{
				float4 color = tex2D(_T, i.uv);
				float intensity = max(dot(i.normal, normalize(_l)), 0.0);
				return color * intensity;
			}

			ENDCG
		}
	}
}
