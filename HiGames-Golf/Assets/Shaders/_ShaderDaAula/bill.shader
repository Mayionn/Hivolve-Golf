Shader "geo/bill"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_sizeX("sizeX", float) = 1.0
		_sizeY("sizeY", float) = 1.0
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma geometry geo
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform float _sizeY, _sizeX;

			struct vert_in 
			{
				float4 pos : POSITION;
				float3 normal : NORMAL;
			};

			struct geo_in 
			{
				float4 pos : POSITION;
				float3 normal : NORMAL;
			};

			struct frag_in 
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			geo_in vert(vert_in i) 
			{
				geo_in o = (geo_in)0;
				o.pos = mul(UNITY_MATRIX_M, i.pos);
				o.normal = i.normal;
				return o;
			}

			[maxvertexcount(4)]
			void geo(point geo_in i[1], 
					inout TriangleStream<frag_in> triStream)
			{
				float3 up = float3(0,1,0);
				float3 look = normalize(-_WorldSpaceCameraPos.xyz + i[0].pos.xyz);
				float3 _right = cross(up, look);

				float4 pos_bill[4]; 
				float2 uv_bill[4];
				pos_bill[0] = float4(i[0].pos + up*_sizeY - _right*_sizeX, 1.0);
				pos_bill[1] = float4(i[0].pos + up*_sizeY + _right*_sizeX, 1.0);
				pos_bill[2] = float4(i[0].pos - up*_sizeY - _right*_sizeX, 1.0);
				pos_bill[3] = float4(i[0].pos - up*_sizeY + _right*_sizeX, 1.0);
				pos_bill[0] = mul(UNITY_MATRIX_VP, pos_bill[0]);
				pos_bill[1] = mul(UNITY_MATRIX_VP, pos_bill[1]);
				pos_bill[2] = mul(UNITY_MATRIX_VP, pos_bill[2]);
				pos_bill[3] = mul(UNITY_MATRIX_VP, pos_bill[3]);
				uv_bill[0] = float2(0, 1);
				uv_bill[1] = float2(1, 1);
				uv_bill[2] = float2(0, 0);
				uv_bill[3] = float2(1, 0);
				//
				frag_in o;
				o.pos = pos_bill[0];
				o.uv = uv_bill[0];
				triStream.Append(o);
				o.pos = pos_bill[1];
				o.uv = uv_bill[1];
				triStream.Append(o);
				o.pos = pos_bill[2];
				o.uv = uv_bill[2];
				triStream.Append(o);
				o.pos = pos_bill[3];
				o.uv = uv_bill[3];
				triStream.Append(o);

				triStream.RestartStrip();
			}

			float4 frag(frag_in i) : COLOR 
			{
				return tex2D(_MainTex, i.uv);
			}

            ENDCG
        }
    }
}
