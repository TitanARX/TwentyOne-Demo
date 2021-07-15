Shader "RetroLookPro/AnalogTVNoise"
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
		float2 texcoordStereo : TEXCOORD1;
	};

	#if STEREO_INSTANCING_ENABLED
		float _DepthSlice;
	#endif
	float _RenderViewportScaleFactor;
	float2 TransformStereoScreenSpaceTex(float2 uv, float w)
	{
		return uv * _RenderViewportScaleFactor;
	}

	// Vertex manipulation
	float2 TransformTriangleVertexToUV(float2 vertex)
	{
		float2 uv = (vertex + 1.0) * 0.5;
		return uv;
	}

	sampler2D _Pattern;
	sampler2D _MainTex;

	float _TimeX;
	half _Fade;
	half barHeight = 6.;
	half barOffset = 0.6;
	half barSpeed = 2.6;
	half barOverflow = 1.2;
	half edgeCutOff;
	half cut;
	half _OffsetNoiseX;
	half _OffsetNoiseY;
	half4 _MainTex_ST;
	half tileX = 0;
	half tileY = 0;
	half angle;
	float2 UnityStereoScreenSpaceUVAdjust(float2 uv, float4 scaleAndOffset)
	{
		return uv.xy * scaleAndOffset.xy + scaleAndOffset.zw;
	}
	VaryingsDefault VertDef(AttributesDefault v)
	{
		VaryingsDefault o;
		o.vertex = float4(v.vertex.xy, 0.0, 1.0);
		o.texcoord = TransformTriangleVertexToUV(v.vertex.xy);
		o.texcoord = o.texcoord * float2(1.0, -1.0) + float2(0.0, 1.0);
		float2 pivot = float2(0.5, 0.5);
		// Rotation Matrix
		float cosAngle = cos(angle);
		float sinAngle = sin(angle);
		float2x2 rot = float2x2(cosAngle, -sinAngle, sinAngle, cosAngle);
		// Rotation consedering pivot
		float2 uv = v.vertex.xy;
		float2 sfsf = mul(rot, uv);
		o.texcoordStereo = TransformStereoScreenSpaceTex(sfsf + o.texcoord + float2(_OffsetNoiseX - 0.2f, _OffsetNoiseY), 1.0);
		o.texcoordStereo *= float2(tileY, tileX);
		return o;
	}

	VaryingsDefault VertDefVertical(AttributesDefault v)
	{
		VaryingsDefault o;
		o.vertex = float4(v.vertex.xy, 0.0, 1.0);
		o.texcoord = TransformTriangleVertexToUV(v.vertex.xy);
		o.texcoord = o.texcoord * float2(1.0, -1.0) + float2(0.0, 1.0);
		float2 pivot = float2(1, 1);
		// Rotation Matrix
		float cosAngle = cos(angle);
		float sinAngle = sin(angle);
		float2x2 rot = float2x2(cosAngle, -sinAngle, sinAngle, cosAngle);
		// Rotation consedering pivot
		float2 uv = v.vertex.xy;
		float2 sfsf = mul(rot, uv);

		o.texcoordStereo = TransformStereoScreenSpaceTex(sfsf + o.texcoord + float2(_OffsetNoiseX, _OffsetNoiseY), 1.0);
		o.texcoordStereo *= float2(tileX, tileY);
		return o;
	}

	float4 Frag0(VaryingsDefault i) : SV_Target
	{
		float2 uvst = UnityStereoScreenSpaceUVAdjust(i.texcoord, _MainTex_ST);
		float3 col;
		float4 text = tex2D(_MainTex, i.texcoord);
		float4 pat = tex2D(_Pattern, i.texcoordStereo);
		col.rgb = text.xyz;
		float bar = floor(edgeCutOff + sin(uvst.xy.y * barHeight + _TimeX * barSpeed) * 50);
		float f = clamp(bar * 0.03,0,1);
		col = lerp(pat.rgb,col, f);
		col = lerp(text.rgb, col, smoothstep(col.r - cut,0,1) * _Fade);
		return float4(col,text.a);
	}

		float4 Frag(VaryingsDefault i) : SV_Target
	{
		float2 uvst = UnityStereoScreenSpaceUVAdjust(i.texcoord, _MainTex_ST);
		float3 col;
		float4 text = tex2D(_MainTex, i.texcoord);
		float4 pat = tex2D(_Pattern, i.texcoordStereo);
		col.rgb = text.xyz;
		float bar = floor(edgeCutOff + sin(uvst.xy.x * barHeight + _TimeX * barSpeed) * 50);
		float f = clamp(bar * 0.03,0,1);
		col = lerp(pat.rgb,col, f);
		col = lerp(text.rgb, col, smoothstep(col.r - cut,0,1) * _Fade);
		return float4(col,text.a);
	}

		ENDHLSL

		SubShader
	{
		Cull Off ZWrite Off ZTest Always
			Pass
		{
			HLSLPROGRAM

				#pragma vertex VertDef
				#pragma fragment Frag0

			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM

				#pragma vertex VertDefVertical
				#pragma fragment Frag

			ENDHLSL
		}
	}
}