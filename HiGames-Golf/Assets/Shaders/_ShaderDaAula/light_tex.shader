// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "mwanga/light_tex"
{
    Properties
    {
		_dayTex("Textura do Dia", 2D) = "white" {}
		_nightTex("Night Texture", 2D) = "white" {}
    }
    SubShader
    {

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

			uniform sampler2D _dayTex;
			uniform sampler2D _nightTex;

			struct vertexIn
			{
				float4 pos : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct vertexOut
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			vertexOut vert(vertexIn i)
			{
				vertexOut o;
				o.normal = normalize(mul(float4(i.normal,0), 
										unity_WorldToObject).xyz);
				o.uv = i.uv;
				o.pos = UnityObjectToClipPos(i.pos);

				return o;
			}

			float4 frag(vertexOut i) : COLOR
			{
				float3 ldir = normalize(_WorldSpaceLightPos0.xyz);
				float3 n = normalize(i.normal);
				float4 cor, corDia, corNoite;
				corDia = tex2D(_dayTex, i.uv);
				corNoite = tex2D(_nightTex, i.uv);
				float intensity = max(dot(n, ldir), 0.0);
				
				cor = lerp(corNoite,corDia,intensity);

				return cor;
			}

            
            ENDCG
        }
    }
}
