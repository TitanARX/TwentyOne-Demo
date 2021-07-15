Shader "RetroLookPro/PictureCorrection"
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
	float signalAdjustY = 0.0;
	float signalAdjustI = 0.0;
	float signalAdjustQ = 0.0;

	float signalShiftY = 0.0;
	float signalShiftI = 0.0;
	float signalShiftQ = 0.0;
	float gammaCorection = 1.0;
	half3 rgb2yiq(half3 c) {
		return half3(
			(0.2989 * c.x + 0.5959 * c.y + 0.2115 * c.z),
			(0.5870 * c.x - 0.2744 * c.y - 0.5229 * c.z),
			(0.1140 * c.x - 0.3216 * c.y + 0.3114 * c.z)
			);
	};

	half3 yiq2rgb(half3 c) {
		return half3(
			(1.0 * c.x + 1.0 * c.y + 1.0 * c.z),
			(0.956 * c.x - 0.2720 * c.y - 1.1060 * c.z),
			(0.6210 * c.x - 0.6474 * c.y + 1.7046 * c.z)
			);
	};

	half3 t2d(float2 p) {
		half3 col = tex2D(_MainTex, p).rgb;
		return rgb2yiq(col);
	}

	float4 Frag(VaryingsDefault i) : SV_Target
	{
		half3 signal = half3(0.0,0.0,0.0);
		float2 p = i.texcoord.xy;
		signal = t2d(p);
					signal.x += signalAdjustY;
				   signal.y += signalAdjustI;
				   signal.z += signalAdjustQ;
				   signal.x *= signalShiftY;
				   signal.y *= signalShiftI;
				   signal.z *= signalShiftQ;

		float3 rgb = yiq2rgb(signal);
					if (gammaCorection != 1.0) rgb = pow(abs(rgb), gammaCorection);

		return half4(rgb, tex2D(_MainTex,i.texcoord).a);
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