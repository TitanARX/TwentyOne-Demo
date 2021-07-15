Shader "RetroLookPro/PulsatingVignette"
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
	float vignetteAmount = 1.0;
	float vignetteSpeed = 1.0;
	float _Time = 0.0;
	float vignette(float2 uv, float t)
	{
		float vigAmt = 2.5 + 0.1 * sin(t + 5.0 * cos(t * 5.0));
		float c = (1.0 - vigAmt * (uv.y - 0.5) * (uv.y - 0.5)) * (1.0 - vigAmt * (uv.x - 0.5) * (uv.x - 0.5));
		c = pow(abs(c), vignetteAmount);
		return c;
	}

	float4 Frag(VaryingsDefault i) : SV_Target
	{
		float2 p = i.texcoord.xy;
	    float4 rgb = tex2D(_MainTex, p);
		rgb.rgb *= vignette(p, _Time * vignetteSpeed);
		return half4(rgb);
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