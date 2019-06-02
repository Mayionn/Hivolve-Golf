Shader "Golf/ToonShader_Collision"
{
	Properties
	{
		//Ball Color
		_Color("Color", Color) = (0.5, 0.65, 1, 1)
		//Collision Effect Properties
		_ContactColor("ContactColor", Color) = (1,1,1,1)
		_ImpactSize("ImpactSize", float) = 0.5
		//Texture
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

			struct appdata
			{
				float4 vertex : POSITION;
				float4 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};
			struct fragment
			{
				float4 color : COLOR;
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			//Ball color
			float4 _Color;
			float _ShadowIntensity;
			//ContactPoints
			float4 _ContactColor;
			float _ImpactSize;
			int _ContactPointsSize;
			float4 _ContactPoints[50];

			fragment vert(appdata v)
			{
				fragment o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				
				//ContactPoints
				o.color = _Color;
				float4 objPos = mul(unity_WorldToObject, float4(o.pos.xyz, 1));
				for (int i = 0; i < _ContactPointsSize; i++)
				{
					//Add Ball Color so it doesnt turn black
					o.color += max(0 ,_ImpactSize - distance(_ContactPoints[i], objPos)) * _ContactColor;
				}

				return o;
			}

			float4 frag(fragment i) : COLOR
			{
				return tex2D(_MainTex, i.uv) * i.color;
			}
			ENDCG
		}
		//UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
	}
}