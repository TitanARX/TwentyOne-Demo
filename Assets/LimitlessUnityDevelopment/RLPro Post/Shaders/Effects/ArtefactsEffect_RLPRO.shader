Shader "RetroLookPro/ArtefactsEffect"
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
	sampler2D _LastTex;
	sampler2D _FeedbackTex;
	float feedbackAmount = 0.0;
	float feedbackFade = 0.0;
	float feedbackThresh = 5.0;
	half3 feedbackColor = half3(1.0, 0.5, 0.0);
	half3 bm_screen(half3 a, half3 b) { return 1.0 - (1.0 - a) * (1.0 - b); }
	float feedbackAmp = 1.0;

	float4 Frag(VaryingsDefault i) : SV_Target
	{
		float2 p = i.texcoord.xy;
		float one_x = 1.0 / _ScreenParams.x;

		half3 fc = tex2D(_MainTex, i.texcoord).rgb;
		half3 fl = tex2D(_LastTex, i.texcoord).rgb;
		float diff = abs(fl.x - fc.x + fl.y - fc.y + fl.z - fc.z) / 3.0;
		if (diff < feedbackThresh) diff = 0.0;

		half3 fbn = fc * diff * feedbackAmount;

		half3 fbb = half3(0.0, 0.0, 0.0);

		fbb = (
				tex2D(_FeedbackTex, i.texcoord).rgb +
				tex2D(_FeedbackTex, i.texcoord + float2(one_x, 0.0)).rgb +
				tex2D(_FeedbackTex, i.texcoord - float2(one_x, 0.0)).rgb
			  ) / 3.0;
		fbb *= feedbackFade;
		fbn = bm_screen(fbn, fbb);
		return half4(fbn * feedbackColor, tex2D(_MainTex, i.texcoord).a * diff);
	}
	
	float4 Frag1(VaryingsDefault i) : SV_Target
	{
		float2 p = i.texcoord;
		half4 col = tex2D(_MainTex, i.texcoord);
		half3 fbb = tex2D(_FeedbackTex, i.texcoord).rgb;
		col.rgb = bm_screen(col.rgb, fbb * feedbackAmp);
		return col;
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
			Pass
		{
			HLSLPROGRAM

				#pragma vertex VertDefault
				#pragma fragment Frag1

			ENDHLSL
		}
	}
}