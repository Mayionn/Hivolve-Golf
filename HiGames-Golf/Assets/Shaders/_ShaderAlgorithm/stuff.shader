// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "procedural/stuff"
{
    Properties
    {
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct vertex_in
            {
            	float4 pos : POSITION;
            	float2 uv : TEXCOORD0;
            };

            struct frag_in
            {
            	float4 pos : SV_POSITION;
            	float2 uv : TEXCOORD0;
            };

            frag_in vert(vertex_in i)
            {
            	frag_in o;
            	o.pos = UnityObjectToClipPos(i.pos);
            	o.uv = i.uv;
            	return o;
            }

            //funções úteis

            float desenha(float2 uv, float p)
            {
            	float espessura = 0.03;
            	return (smoothstep(p-espessura,p,uv.y)-smoothstep(p,p+espessura,uv.y));
            }

            float2x2 roda2d(float a)
            {
            	//cos(a) -sin(a)
            	//sin(a) cos(a)
            	return float2x2(cos(a),-sin(a),sin(a),cos(a));
            }

            //geradores pseudo aleatórios
            float rngesus(float2 uv)
            { 
            	return frac(tan(dot(uv, float2(13.5392,77.3412))) * 
            				53982.12345);
            }

            float rngesus2(float2 uv)
            { 
            	return frac(cos(dot(uv, float2(13.5392,77.3412))) * 
            				582.12345);
            }

            float random2D(float2 uv)
            {
            	uv = float2(dot(uv, float2(135.2, 345.3)), dot(uv, float2(294.7,145.6)));
            	return (2.0*frac(sin(uv)*45678.90123)-1.0);
            }

            float noise1(float2 uv)
            { 
            	float2 i = floor(uv);
            	float2 f = frac(uv);

            	float2 comb = f*f*(3.0-2.0*f);

            	float2 a = i+float2(0,0);
            	float2 b = i+float2(1,0);
            	float2 c = i+float2(0,1);
            	float2 d = i+float2(1,1);

            	return lerp(lerp(dot(random2D(a),f-float2(0,0)), dot(random2D(b),f-float2(1,0)),uv.x),
            				lerp(dot(random2D(c),f-float2(0,1)), dot(random2D(d),f-float2(1,1)),uv.x),
            				uv.y);
            }

            float noise2(float2 uv)
            { 
            	float2 i = floor(uv);
            	float2 f = frac(uv);

            	float2 comb = f*f*(3.0-2.0*f);

            	float2 a = i+float2(0,0);
            	float2 b = i+float2(1,0);
            	float2 c = i+float2(0,1);
            	float2 d = i+float2(1,1);

            	return lerp(lerp(random2D(a),random2D(b),uv.x),
            				lerp(random2D(c),random2D(d),uv.x),
            				uv.y);
            }

            float noise3(float2 uv)
            { 
            	float2 i = floor(uv);
            	float2 f = frac(uv);

            	float2 comb = f*f*(3.0-2.0*f);

            	float2 a = i+float2(0,0);
            	float2 b = i+float2(1,0);
            	float2 c = i+float2(0,1);
            	float2 d = i+float2(1,1);

            	return lerp(lerp(rngesus(a),rngesus(b),uv.x),
            				lerp(rngesus(c),rngesus(d),uv.x),
            				uv.y);
            }

            float lines(float2 uv, float b)
            {
            	float scale = 10;
            	uv *= scale;
            	return smoothstep(0, 0.5*b+0.5, (sin(uv.x*3.1415)+b*2.0)*0.5);
            }

            //
            float random(float2 uv)
            {
            	return frac( sin(dot(uv.xy, float2(12.9898,78.233))) * 43758.5453123);
            }

            float2x2 rotate2d(float a)
            {
            	return float2x2(cos(a), -sin(a), sin(a), cos(a));
            }

            float noise(float2 uv)
            {
            	float2 i = floor(uv);
            	float2 f = frac(uv);
            	float2 u = f*f*(3.0-2.0*f);

            	float a = random(i);
            	float b = random(i + float2(1,0));
            	float c = random(i + float2(0,1));
            	float d = random(i + float2(1,1));

            	//return (lerp(a,b,u.x) + (c-a)*u.y*(1.0-u.x) + (d-b)*u.x*u.y);
            	return lerp(lerp(a,b,u.x),lerp(c,d,u.x), u.y);
            }

            float4 frag(frag_in i) : COLOR
            {
            	//float3 c = float3(0,0,0);
            	//float2 sc = i.uv; // screen coordinates

            	//linhas
            	//float eq = smoothstep(0.2,0.8,sc.x); // equação linha
            	//float eq = step(0.8,sc.x); // equação linha
            	//float p = desenha(sc,eq);

            	//objectos
            	//quadrado
            	//float2 b1 = step(float2(0.1,0.1),sc);
            	//float p = b1.x * b1.y;
				//
            	//float2 b2 = step(float2(0.1,0.1),1-sc);
            	//p *= b2.x * b2.y;
				//
            	////circulo
            	//float raio = 0.1;
            	//float2 dist = sc-float2(0.5,0.5);
            	//p = 1-smoothstep(raio-(raio*0.01),raio+(raio*0.01),dot(dist,dist));
				//
            	////forma da cena
            	//sc = sc*5; // divisão do espaço
            	//float sc2 = sc*2;
            	//float2 sc_i = floor(sc);
            	//float2 sc_f = frac(sc); // tiling
            	//float2 pos = float2(0.5,0.5)-sc_f;
            	//pos = mul(roda2d(cos(_Time.y)*3.1415),pos);
            	//float r = length(pos)*2.0;
            	//float a = atan2(pos.y, pos.x);
				//
            	//p = cos(a*10);
            	//p = 1-smoothstep(p, p+0.02,r);
            	////
            	//float p2 = 0.0; //usar sc2
            	//float2 sc_f2 = frac(sc2);
            	//float2 dist2 = sc_f2 - float2(0.5,0.5);
            	//float raio2 = 0.02;
            	//p2 = 1-smoothstep(raio2-(raio2*0.01),raio2+(raio2*0.01),dot(dist2,dist2));
				//
            	////c = float3(p+p2,p+p2,p+p2);
				//
				//
            	//float2 ipos = floor(sc*5);
            	//float2 fpos = frac(sc*5);
            	//float rand = rngesus2(ipos);
				//
            	//float2 tile;
            	//float ii = frac((rand-0.5)*2.0);
            	//if(ii > 0.75)
            	//{
            	//	tile = float2(1,1) - fpos;
            	//} else if(ii > 0.5)
            	//{
            	//	tile = float2(1-fpos.x,fpos.y);
            	//} else if(ii > 0.25)
            	//{
            	//	tile = 1.0 - float2(1-fpos.x,fpos.y);
            	//}
            	//p = desenha(tile,tile.x);
            	//c = float3(p,p,p);

            	//sc - scree coordinates

            	//float n = noise2(sc*100);

            	//float2 pos = float2(sc.y * 12.0, sc.x * 3.0);
            	//float p = pos.x;
				//
            	//pos = mul(roda2d(noise3(pos)),pos);
            	////pos = mul(roda2d(cos(_Time.y)*3.1415),pos);
            	//p = lines(pos, 0.5);
				//
            	//c = float3(p,p,p);
				//
            	//return float4(c, 1);

            	float2 uv = i.uv;
            	//uv.y *= uv.y/uv.x;
            	//float2 pos = mul(uv.yx,float2(12.0,3.0));
            	float2 pos = float2(uv.y*10, uv.x*3);
            	//float n = noise(pos);

            	float pat = pos.x;

            	pos = mul(rotate2d(noise(pos)),pos);

            	pat = lines(pos, 0.5);

            	return float4(pat,pat,pat,1.0);
            	//return float4(uv,0,1);
            }

            ENDCG
        }
    }
}
