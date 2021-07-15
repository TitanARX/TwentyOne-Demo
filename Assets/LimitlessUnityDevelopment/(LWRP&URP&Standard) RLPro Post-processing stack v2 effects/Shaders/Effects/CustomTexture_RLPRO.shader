Shader "RetroLookPro/CustomTexture"
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
	sampler2D _CustomTex;
	half fade;
	half alpha;
	float4 Frag(VaryingsDefault i) : SV_Target
	{
		float4 col = tex2D(_MainTex,i.texcoord);
		float4 col2 = tex2D(_CustomTex,i.texcoord);

		if (alpha < 1)
			col2.a = fade;
		return lerp(col, col2, fade);

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