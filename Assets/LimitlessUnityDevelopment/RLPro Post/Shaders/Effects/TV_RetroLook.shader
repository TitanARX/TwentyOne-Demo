﻿Shader "RetroLookPro/TV_RetroLook"
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
	float maskDark = 0.5;
	float maskLight = 1.5;
	float hardScan = -8.0;
	float hardPix = -3.0;
	float2 warp = float2(1.0 / 32.0, 1.0 / 24.0);
	float2 res;
	float resScale;
	float scale;
	float fade;
	float3 Fetch(float2 pos, float2 off)
	{
		pos = floor(pos * res + off) / res;
		return tex2Dlod(_MainTex, float4(pos.xy, 0, -16.0)).rgb;
	}

	float2 Dist(float2 pos) { pos = pos * res; return -((pos - floor(pos)) - float2(0.5, 0.5)); }
	float Gaus(float pos, float scale) { return exp2(scale * pos * pos); }
	float3 Horz3(float2 pos, float off)
	{
		float3 b = Fetch(pos, float2(-1.0, off));
		float3 c = Fetch(pos, float2(0.0, off));
		float3 d = Fetch(pos, float2(1.0, off));
		float dst = Dist(pos).x;
		float scale = hardPix;
		float wb = Gaus(dst - 1.0, scale);
		float wc = Gaus(dst + 0.0, scale);
		float wd = Gaus(dst + 1.0, scale);
		return (b * wb + c * wc + d * wd) / (wb + wc + wd);
	}
	float3 Horz5(float2 pos, float off)
	{
		float3 a = Fetch(pos, float2(-2.0, off));
		float3 b = Fetch(pos, float2(-1.0, off));
		float3 c = Fetch(pos, float2(0.0, off));
		float3 d = Fetch(pos, float2(1.0, off));
		float3 e = Fetch(pos, float2(2.0, off));
		float dst = Dist(pos).x;
		float scale = hardPix;
		float wa = Gaus(dst - 2.0, scale);
		float wb = Gaus(dst - 1.0, scale);
		float wc = Gaus(dst + 0.0, scale);
		float wd = Gaus(dst + 1.0, scale);
		float we = Gaus(dst + 2.0, scale);
		return (a * wa + b * wb + c * wc + d * wd + e * we) / (wa + wb + wc + wd + we);
	}
	float Scan(float2 pos, float off)
	{
		float dst = Dist(pos).y;
		return Gaus(dst + off, hardScan);
	}

	float3 Tri(float2 pos)
	{
		float3 a = Horz3(pos, -1.0);
		float3 b = Horz5(pos, 0.0);
		float3 c = Horz3(pos, 1.0);
		float wa = Scan(pos, -1.0);
		float wb = Scan(pos, 0.0);
		float wc = Scan(pos, 1.0);
		return a * wa + b * wb + c * wc;
	}

	float2 Warp(float2 pos)
	{
		float2 h = pos - float2(0.5, 0.5);
		float r2 = dot(h, h);
		float f = 1.0 + r2 * (warp.x + warp.y * sqrt(r2));
		return f * scale * h + 0.5;
	}
	float2 Warp1(float2 pos)
	{
		pos = pos * 2.0 - 1.0;
		pos *= float2(1.0 + (pos.y * pos.y) * warp.x, 1.0 + (pos.x * pos.x) * warp.y);
		return pos * scale + 0.5;
	}

	float3 Mask(float2 pos)
	{
		pos.x += pos.y * 3.0;
		float3 mask = float3(maskDark, maskDark, maskDark);
		pos.x = frac(pos.x / 6.0);
		if (pos.x < 0.333)mask.r = maskLight;
		else if (pos.x < 0.666)mask.g = maskLight;
		else mask.b = maskLight;
		return mask;
	}

	float4 Frag0(VaryingsDefault i) : SV_Target
	{
		float4 col = tex2D(_MainTex,i.texcoord);
		res = _ScreenParams.xy / resScale;
		float2 fragCoord = i.texcoord.xy * _ScreenParams.xy;
		float4 fragColor = 0;
		float2 pos = Warp1(fragCoord.xy / _ScreenParams.xy);
		fragColor.rgb = Tri(pos) * Mask(fragCoord);
		fragColor.a = tex2D(_MainTex,pos).a;
		return lerp(col,fragColor,fade);
	}

		float4 Frag(VaryingsDefault i) : SV_Target
	{
		float4 col = tex2D(_MainTex,i.texcoord);
		res = _ScreenParams.xy / resScale;
		float2 fragCoord = i.texcoord.xy * _ScreenParams.xy;
		float4 fragColor = 0;
		float2 pos = Warp(fragCoord.xy / _ScreenParams.xy);
		fragColor.rgb = Tri(pos) * Mask(fragCoord);
		fragColor.a = tex2D(_MainTex,pos).a;
		return lerp(col,fragColor,fade);
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