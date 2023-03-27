Shader "RetroLookPro/UltimateVignette_RLPro"
{
	HLSLINCLUDE
#pragma fragmentoption ARB_precision_hint_fastest 
		sampler2D _MainTex;
	float4 _MainTex_ST;
	half4 _Params;
	half3 _InnerColor;
	half4 _Center;
#pragma shader_feature VIGNETTE_CIRCLE
#pragma shader_feature VIGNETTE_SQUARE
#pragma shader_feature VIGNETTE_ROUNDEDCORNERS
	half2 _Params1;

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
	float4 Frag(VaryingsDefault i) : SV_Target
	{
		half4 color = tex2D(_MainTex, i.texcoord);

		#if VIGNETTE_CIRCLE
		half d = distance(i.texcoord, _Center.xy);
		half multiplier = smoothstep(0.8, _Params.x * 0.799, d * (_Params.y + _Params.x));
		#elif VIGNETTE_ROUNDEDCORNERS
		half2 uv = -i.texcoord * i.texcoord + i.texcoord;
		half v = saturate(uv.x * uv.y * _Params1.x + _Params1.y);
		half multiplier = smoothstep(0.8, _Params.x * 0.799, v * (_Params.y + _Params.x));
		#else
		half multiplier = 1.0;
		#endif
		_InnerColor = -_InnerColor;
		color.rgb = (color.rgb - _InnerColor) * max((1.0 - _Params.z * (multiplier - 1.0) - _Params.w), 1.0) + _InnerColor;
		color.rgb *= multiplier;

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
				#pragma fragment Frag

			ENDHLSL
		}
	}
}