// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "mwanga/environment"
{
	Properties
	{
		_Cube("Reflection Map", Cube) = ""  {}
		_refract("refraction index", float) = 1.5
	}
	SubShader
	{
	Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			uniform samplerCUBE _Cube;
			uniform float _refract;

			struct vertexIn 
			{
				float4 pos : POSITION;
				float3 normal : NORMAL;
			};

			struct vertexOut
			{
				float4 pos : SV_POSITION;
				float3 normal : NORMAL;
				float3 look : TEXCOORD0;
			};

			vertexOut vert(vertexIn i)
			{
				vertexOut o;

				o.look = normalize(mul(UNITY_MATRIX_M, i.pos).xyz - 
							_WorldSpaceCameraPos.xyz);
				o.normal = normalize(mul(float4(i.normal,0.0), 
										unity_WorldToObject).xyz
									);
				o.pos = UnityObjectToClipPos(i.pos);

				return o;
			}

			float4 frag(vertexOut i) : COLOR
			{
				//float3 r = reflect(	normalize(i.look), 
				//					normalize(i.normal));
				float refract_index = _refract;
				float3 r = refract(normalize(i.look),
									normalize(i.normal),
									refract_index);
				return texCUBE(_Cube, r);
			}
			ENDCG
        }
    }
}
