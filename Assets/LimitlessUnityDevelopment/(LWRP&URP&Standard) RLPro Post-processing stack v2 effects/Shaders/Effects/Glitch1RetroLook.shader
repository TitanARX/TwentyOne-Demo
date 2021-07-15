Shader "RetroLookPro/Glitch1RetroLook"
{
	HLSLINCLUDE
		struct AttributesDefault
	{
		float3 vertex : POSITION;
	};
	float4 _ScreenParams;
	struct VaryingsDefault
	{
		float4 vertex : SV_POSITION;
		float2 texcoord : TEXCOORD0;
	};

	float2 TransformTriangleVertexToUV(float2 vertex)
	{
		float2 uv = (vertex + 1.0) * 0.5;
		return uv;
	}
	VaryingsDefault VertDefault(AttributesDefault v)
	{
		VaryingsDefault o;
		o.vertex = float4(v.vertex.xy, 0.0, 1.0);
		o.texcoord = TransformTriangleVertexToUV(v.vertex.xy);
		o.texcoord = o.texcoord * float2(1.0, -1.0) + float2(0.0, 1.0);
		return o;
	}
	uniform sampler2D _MainTex;
	half alphaTex;
	sampler2D _AlphaMapTex;
	uniform float T;
	uniform float Speed;
	uniform float Strength;
	uniform float Fade;
	float x = 127.1;
	float angleY = 311.7;
	float y = 43758.5453123;
	uniform float Stretch = 0.02;

	half mR = 0.08;
	half mG = 0.07;
	half mB = 0.0;

	float hash(float2 d)
	{
		float m = dot(d, float2(x, angleY));
		return -1.0 + 2.0 * frac(sin(m) * y);
	}
	float noise(float2 d)
	{
		float2 i = floor(d);
		float2 f = frac(d);
		float2 u = f * f * (3.0 - 2.0 * f);
		return lerp(lerp(hash(i + float2(0.0, 0.0)), hash(i + float2(1.0, 0.0)), u.x), lerp(hash(i + float2(0.0, 1.0)), hash(i + float2(1.0, 1.0)), u.x), u.y);
	}
	float noise1(float2 d)
	{
		float2 s = float2 (1.6, 1.2);
		float f = 0.0;
		for (int i = 1; i < 3; i++) { float mul = 1.0 / pow(2.0, float(i)); f += mul * noise(d); d = s * d; }
		return f;
	}

	float4 Frag(VaryingsDefault i) : SV_Target
	{
		float4 result = float4(0,0,0,0);
		float2 uv = i.texcoord;
		float glitch = pow(abs(cos(T * Speed * 0.5) * 1.2 + 1.0), 1.2);
		glitch = saturate(glitch);
		float2 hp = float2(0.0, uv.y);
		float nh = noise1(hp * 7.0 + T * Speed * 10.0) * (noise(hp + T * Speed * 0.3) * 0.8);
		nh += noise1(hp * 100.0 + T * Speed * 10.0) * Stretch;
		float rnd = 0.0;
		if (glitch > 0.0) { rnd = hash(uv); if (glitch < 1.0) { rnd *= glitch; } }
		nh *= glitch + rnd;
		half shiftR = 0.08 * mR;
		half shiftG = 0.07 * mG;
		half shiftB = mB;
		float cuttOff = 1;
		if (alphaTex > 0)
			cuttOff = tex2D(_AlphaMapTex, i.texcoord).a;
		float4 r = tex2D(_MainTex, uv + float2(nh, shiftR) * nh * Strength* cuttOff);
		float4 g = tex2D(_MainTex, uv + float2(nh - shiftG, 0.0) * nh * Strength * cuttOff );
		float4 b = tex2D(_MainTex, uv + float2(nh, shiftB) * nh * Strength * cuttOff );
		float4 kkk = tex2D(_MainTex, i.texcoord);
		float alpha = r.a + g.a + b.a;
		alpha += kkk.a;
		float4 col = float4(r.r, g.g, b.b, alpha);
		result = lerp(kkk,col,Fade);
		return result;
	}

		ENDHLSL

		SubShader
	{
		Cull Off ZWrite Off ZTest Always

			Pass
		{
			HLSLPROGRAM

				#pragma vertex VertDefault
				#pragma fragment Frag

			ENDHLSL
		}
	}
}