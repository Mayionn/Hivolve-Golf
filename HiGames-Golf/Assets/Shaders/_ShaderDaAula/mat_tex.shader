Shader "mwanga/mat_tex"
{
    Properties
    {
        _MainTex("Textura", 2D) = "white" {}
	}
		SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			uniform sampler2D _MainTex;

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
				o.uv = i.uv;

				return o;
			}

			float4 frag(vertexOut i) : COLOR
			{
				float time = _Time.x * 50;
				/*float4 cor = tex2D(_MainTex,
								float2(
									cos(time*0.5)*tan(time)*i.uv.x, 
									cos(time)*i.uv.y
								)
							);
				*/
				float4 cor = float4(cos(time)*i.uv.x,(sin(time)+1)*0.5,1,1);
				/*if (time > 0) 
				{
					cor = float4(0,1,0,1);
				}
				else 
				{
					cor = float4(0, 0, 1, 1);
				}
				*/
				return cor;
			}
			ENDCG
        }
    }
}
