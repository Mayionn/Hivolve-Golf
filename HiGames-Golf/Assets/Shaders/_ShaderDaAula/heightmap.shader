Shader "mwanga/mat_Heightmap"
{
    Properties
    {
		_diff("Color", Color) = (1,1,1,1)
        _MainTex("Textura", 2D) = "white" {}
		_hmap("Height Map", 2D) = "white" {}
		_h("Displacement Height", float) = 0.1
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
				uniform sampler2D _hmap;
				uniform float _h;

				struct vertexIn
				{
					float4 pos : POSITION;
					float2 uv : TEXCOORD0;
					float3 n : NORMAL;
				};

				struct vertexOut
				{
					float4 pos : SV_POSITION;
					float2 uv : TEXCOORD0;
				};

				vertexOut vert(vertexIn i)
				{
					vertexOut o;
					float4 displacement = tex2Dlod(_hmap, 
											float4(i.uv.x, i.uv.y,0.0,0.0));
					i.pos = i.pos + float4(normalize(i.n),0.0)*displacement.x*_h;
					
					o.pos = UnityObjectToClipPos(i.pos);
					o.uv = i.uv;

					return o;
				}

				float4 frag(vertexOut i) : COLOR
				{
					float4 cor = tex2D(_MainTex, i.uv) * _diff;
					//float time = _Time.x * 50;
					//float4 cor = tex2D(_MainTex,float2(cos(time*0.5)*tan(time)*i.uv.x,cos(time)*i.uv.y));
					
					return cor;
				}
			ENDCG
        }
    }
}
