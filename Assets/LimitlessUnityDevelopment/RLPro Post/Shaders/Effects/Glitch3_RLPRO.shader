Shader "RetroLookPro/Glitch3"
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
	sampler2D _MainTex;
	half alphaTex;
	sampler2D _AlphaMapTex;
	float _Time;
	float speed;
	float density;
	float maxDisplace;

	inline float rand(float2 seed)
	{
		return frac(sin(dot(seed * floor(_Time * speed), float2(127.1, 311.7))) * 43758.5453123);
	}

	inline float rand(float seed)
	{
		return rand(float2(seed, 1.0));
	}

	float4 Frag(VaryingsDefault i) : SV_Target
	{
		float2 rblock = rand(floor(i.texcoord * density));
		float displaceNoise = pow(rblock.x, 8.0) * pow(rblock.x, 3.0) - pow(rand(7.2341), 17.0) * maxDisplace;
		float cuttOff = 1;
		if (alphaTex > 0)
			cuttOff = tex2D(_AlphaMapTex, i.texcoord).a;
		float4 r = tex2D(_MainTex, i.texcoord);
		float4 g = tex2D(_MainTex, i.texcoord + half2(displaceNoise * 0.05 * rand(7.0), 0.0) * cuttOff);
		float4 b = tex2D(_MainTex, i.texcoord - half2(displaceNoise * 0.05 * rand(13.0), 0.0) * cuttOff);

		return half4(r.r, g.g, b.b, r.a + g.a + b.a);
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