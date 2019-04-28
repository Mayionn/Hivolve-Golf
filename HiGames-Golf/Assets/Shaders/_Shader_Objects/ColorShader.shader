Shader "Golf_Objects/ColorShader"
{
    Properties
    {
		_diff("Color", Color) = (1,1,1,1)
        _MainTex("Textura", 2D) = "white" {}
		_Scale("Scale", float) = 0.1
	}
		SubShader
		{
			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				uniform float4 _diff;
				uniform sampler2D _MainTex;
				uniform float _Scale;

				struct vertexIn
				{
					float4 pos : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct vertexOut
				{
					float4 pos : SV_POSITION;
					float2 uv : TEXCOORD0;
				};

				vertexOut vert(vertexIn i)
				{
					vertexOut o;

					o.pos = UnityObjectToClipPos(i.pos);
					o.uv = i.uv * _Scale;

					return o;
				}

				float4 frag(vertexOut i) : COLOR
				{
					return tex2D(_MainTex, i.uv)* _diff;
				}
			ENDCG
        }
    }
}
