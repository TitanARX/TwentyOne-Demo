Shader "RetroLookPro/NegativeFilterRetroLook"
{
	HLSLINCLUDE

	struct AttributesDefault
	{
		float3 vertex : POSITION;
	};
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
	uniform float Luminosity;
	uniform float Vignette;
	uniform float Negative;

	float3 linearLight(float3 s, float3 d)
	{
		return 2.0 * s + d - 1.0 * Luminosity;
	}

	float4 Frag(VaryingsDefault i) : SV_Target
	{
		float2 uv = i.texcoord;
		float3 col = tex2D(_MainTex, uv).rgb;
		col = lerp(col,1 - col,Negative);
		col *= pow(abs(16.0 * uv.x * (1.0 - uv.x) * uv.y * (1.0 - uv.y)), 0.4) * 1 + Vignette;
		col = dot(float3(0.2126, 0.7152, 0.0722), col);
		col = linearLight(float3(0.5,0.5,0.5),col);
		return float4(col, tex2D(_MainTex,uv).a);
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