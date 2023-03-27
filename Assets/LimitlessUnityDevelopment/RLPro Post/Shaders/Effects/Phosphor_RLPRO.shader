Shader "RetroLookPro/Phosphor_RLPro"
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
	sampler2D _Tex;
	float speed = 10.00;
	half amount = 5;
	half fade;
	float T;

	float fract(float x) {
		return  x - floor(x);
	}
	float2 fract(float2 x) {
		return  x - floor(x);
	}

	float random(float2 noise)
	{
		return fract(sin(dot(noise.xy, float2(0.0001, 98.233))) * 925895933.14159265359);
	}

	float random_color(float noise)
	{
		return frac(sin(noise));
	}
	float4 Frag0(VaryingsDefault i) : SV_Target
	{
		float4 col = tex2D(_MainTex,i.texcoord );
		float4 result = col + tex2D(_Tex, i.texcoord);
		return lerp(col,result,fade);
	}

		half4 Frag(VaryingsDefault i) : SV_Target
	{
		half2 uv = fract(i.texcoord.xy / 12 * ((T.x * speed)));
		half4 color = float4(random(uv.xy), random(uv.xy), random(uv.xy), random(uv.xy));

		color.r *= random_color(sin(T.x * speed));
		color.g *= random_color(cos(T.x * speed));
		color.b *= random_color(tan(T.x * speed));
		if (color.a < 33- amount) {
			color = float4(0,0,0,0);
		}

		return color;

	}

		ENDHLSL

		SubShader
	{
		Cull Off ZWrite Off ZTest Always
			Pass
		{
			HLSLPROGRAM

				#pragma vertex VertDefault
				#pragma fragment Frag0

			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM

				#pragma vertex VertDefault
				#pragma fragment Frag

			ENDHLSL
		}
	}
}