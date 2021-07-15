Shader "RetroLookPro/BottomNoiseEffect"
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
	half tileX = 0;
	half tileY = 0;
	half _OffsetNoiseX;
	half _OffsetNoiseY;
	sampler2D _MainTex;
	float4 _MainTex_TexelSize;
	sampler2D _SecondaryTex;

	half _OffsetPosY;
	half _NoiseBottomHeight;
	half _NoiseBottomIntensity;

	VaryingsDefault VertDef(AttributesDefault v)
	{
		VaryingsDefault o;
		o.vertex = float4(v.vertex.xy, 0.0, 1.0);
		o.texcoord = TransformTriangleVertexToUV(v.vertex.xy);

		o.texcoord = o.texcoord * float2(1.0, -1.0) + float2(0.0, 1.0);
		return o;
	}

	float4 Frag(VaryingsDefault i) : SV_Target
	{
		half2 uv = i.texcoord;
		half4 color = tex2D(_MainTex, uv);
		float condition = saturate(floor(_NoiseBottomHeight / uv.y));
		float4 noise_bottom = tex2D(_SecondaryTex, i.texcoord - 0.5) * condition * _NoiseBottomIntensity;
		color = lerp(color, noise_bottom, -noise_bottom * ((uv.y / (_NoiseBottomHeight)) - 1.0));
		float exp = 1.0;
		return float4(pow(color.xyz, float3(exp, exp, exp)), color.a);
	}

		ENDHLSL

		SubShader
	{
		Cull Off ZWrite Off ZTest Always

			Pass
		{
			HLSLPROGRAM

				#pragma vertex VertDef
				#pragma fragment Frag

			ENDHLSL
		}
	}
}