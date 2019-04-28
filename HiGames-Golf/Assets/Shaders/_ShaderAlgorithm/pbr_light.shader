// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "PBR/pbr_light"
{
    Properties
    {
    	_DifColor("Cor Difusa", Color) = (1,1,1,1)
    	_SpecColor("Cor Especular", Color) = (1,1,1,1)
    	_Smoothness("Smoothness", Range(0,1)) = 0
    	_Metallic("Metallic", Range(0,1)) = 0
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM

            	#pragma vertex vert
            	#pragma fragment frag

            	#include "UnityCG.cginc"

            	uniform float4 _DifColor, _SpecColor;
            	uniform float _Smoothness, _Metallic;

            	struct vertex_in
            	{
            		float4 pos : POSITION;
            		float3 normal : NORMAL;
            		float4 tangent : TANGENT;
            	};

            	struct vertex_out
            	{
            		float4 pos : SV_POSITION;
            		float3 normal : NORMAL;
            		float3 tangent : TANGENT;
            		float3 bitangent : TEXCOORD0;
            		float4 worldPos : TEXCOORD1;
            	};

            	//ndf
            	float ggx_ndf(float roughness, float NdotH)
            	{
            		float rr = roughness*roughness;
            		float nn = NdotH*NdotH;
            		float tanr = (1-nn) / (nn);
            		return ( (1.0/3.14) * sqrt(roughness / (nn * (rr+tanr))) );
            	}
            	//gs
            	float walter_gsf(float NdotL, float NdotV, float roughness)
            	{
            		float a = roughness*roughness;
            		float nl = NdotL*NdotL;
            		float nv = NdotV*NdotV;
            		float sl = 2.0 / (1.0 + sqrt(1.0 + a * (1.0-nl)/(nl)));
            		float sv = 2.0 / (1.0 + sqrt(1.0 + a * (1.0-nv)/(nv)));
            		return (sv*sl);
            	}

            	//ff
            	float mix_f(float i, float j, float x)
            	{
            		return j*x + i*(1.0 - x);
            	}

            	float schlick_f(float i)
            	{
            		float x = clamp(1.0-i, 0.0, 1.0);
            		return x*x*x*x*x;
            	}

            	float schlick_ff(float ior, float LdotH)
            	{
            		float f = pow(ior-1,2)/pow(ior+1,2);
            		return f + (1-f)*schlick_f(LdotH);
            	}

            	float f0(float NdotL, float NdotV, float LdotH, float roughness)
            	{
            		float light = schlick_f(NdotL);
            		float view = schlick_f(NdotV);
            		float diff = 0.5 + 2.0*LdotH*LdotH*roughness;
            		return mix_f(1, diff, light) * mix_f(1, diff, view);
            	}

            	vertex_out vert(vertex_in i)
            	{
            		vertex_out o;
            		o.pos = UnityObjectToClipPos(i.pos); //local para ecrã
            		o.normal = normalize(mul(unity_WorldToObject, float4(i.normal, 0)).xyz); //normalizar a normal
            		o.tangent = normalize( mul(float4(i.tangent.xyz, 0), UNITY_MATRIX_M) );
            		o.bitangent = normalize(cross(o.normal,o.tangent));
            		o.worldPos = mul(UNITY_MATRIX_M, i.pos); //local para mundo
            		return o;
            	}

            	float4 frag(vertex_out i) : COLOR
            	{
            		float4 result;

            		float3 N = normalize(i.normal);
            		float3 L = normalize(_WorldSpaceLightPos0.xyz);
            		float3 V = normalize(_WorldSpaceCameraPos.xyz - i.worldPos.xyz);
            		float3 R = reflect(-V, N);
            		float3 H = normalize(V+L);
            		float3 Halt = reflect(-L, N);

            		float4 cDifusa = _DifColor;
            		float4 cSpecular = _SpecColor;

            		float NdotL = max(dot(N, L), 0.0);
            		float NdotH = max(dot(N, H), 0.0);
            		float NdotV = max(dot(N, V), 0.0);
            		float VdotH = max(dot(V, H), 0.0);
            		float LdotH = max(dot(L, H), 0.0);
            		float LdotV = max(dot(L, V), 0.0);
            		float RdotV = max(dot(R, V), 0.0);

            		//distributions
            		float distr_dif = 1;
            		float distr_spec = 1;

            		float smoothness = _Smoothness;
            		float metallic = _Metallic;
            		float roughness = 1.0 - (smoothness*smoothness);
            		roughness *= roughness;
            		float ior = 1;

            		cSpecular = lerp(cSpecular, cDifusa, metallic);
            		//Normal Distribution Function
            		float ndf = ggx_ndf(roughness, NdotH);
            		//Geometry Shadowing
            		float gs = walter_gsf(NdotL, NdotV, roughness);
            		//Fresnel Function
            		float ffd = f0(NdotL, NdotV, LdotH, roughness);
            		float ffs = schlick_ff(ior, LdotH);

            		//calc distributions
            		distr_dif *= max(0.01, (1.0f - metallic)) * ffd;
            		distr_spec *= ((ndf * gs * ffs)/(4.0 * NdotL * NdotV));

            		cDifusa *= distr_dif;
            		cSpecular *= distr_spec;

            		result = float4((cDifusa.rgb + cSpecular.rgb)*NdotL, 1);

            		return result;
            	}

            ENDCG
        }
    }
}
