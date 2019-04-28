// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "hahaha/mat1"
{
	Properties
	{
		_muahaha("Cor da cena", Color) = (1,1,1,1)
	}
	SubShader
	{
	Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			uniform float4 _muahaha;

			struct vertexIn 
			{
				float4 pos : POSITION;
			};

			struct vertexOut
			{
				float4 pos : SV_POSITION;
			};

			vertexOut vert(vertexIn i)
			{
				vertexOut o;

				o.pos = UnityObjectToClipPos(i.pos);

				return o;
			}

			float4 frag(vertexOut pos) : COLOR
			{
				return _muahaha;
			}
			ENDCG
        }
    }
}
