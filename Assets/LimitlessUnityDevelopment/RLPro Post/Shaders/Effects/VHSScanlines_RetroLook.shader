Shader "RetroLookPro/VHSScanlines_RLPro"
{
	HLSLINCLUDE
	float4 _ScanLinesColor;
	float _ScanLines;
	sampler2D _MainTex;
	float speed;
	float fade;
	float _OffsetDistortion;
	float sferical;
	float barrel;
	float scale;
	float _OffsetColor;
	float2 _OffsetColorAngle;
	float _Time;
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
	float2 FisheyeDistortion(float2 coord, float spherical, float barrel, float scale)
	{
		float2 h = coord.xy - float2(0.5, 0.5);
		float r2 = dot(h, h);
		float f = 1.0 + r2 * (spherical + barrel * sqrt(r2));
		return f * scale * h + 0.5;
	}

	float4 FragH(VaryingsDefault i) : SV_Target
	{
		float2 coord = FisheyeDistortion(i.texcoord, sferical, barrel, scale);
		half4 color = tex2D(_MainTex, i.texcoord);
		float lineSize = _ScreenParams.y * 0.005;
		float displacement = ((_Time / 4 * 1000) * speed) % _ScreenParams.y;
		float ps;
		ps = displacement + (coord.y * _ScreenParams.y / i.vertex.w);
		float sc = i.texcoord.y;
		float4 result;
		result = ((uint)(ps / floor(_ScanLines * lineSize)) % 2 == 0) ? color : _ScanLinesColor;
		result += color * sc;
		return lerp(color,result,fade);
	}

		float4 FragHD(VaryingsDefault i) : SV_Target
	{
		float2 coord = FisheyeDistortion(i.texcoord, sferical, barrel, scale);
		half4 color = tex2D(_MainTex, i.texcoord);
		float lineSize = _ScreenParams.y * 0.005;
		float displacement = ((_Time / 4 * 1000) * speed) % _ScreenParams.y;
		float ps;
		i.texcoord.y = frac(i.texcoord.y + cos((coord.x + _Time / 4) * 100) / _OffsetDistortion);
		ps = displacement + (i.texcoord.y * _ScreenParams.y / i.vertex.w);
		float sc = i.texcoord.y;
		float4 result;
		result = ((uint)(ps / floor(_ScanLines * lineSize)) % 2 == 0) ? color : _ScanLinesColor;
		result += color * sc;
		return lerp(color,result,fade);
	}

		float4 FragV(VaryingsDefault i) : SV_Target
	{
		float2 coord = FisheyeDistortion(i.texcoord, sferical, barrel, scale);
		half4 color = tex2D(_MainTex, i.texcoord);
		float lineSize = _ScreenParams.y * 0.005;
		float displacement = ((_Time / 4 * 1000) * speed) % _ScreenParams.y;
		float ps;
		ps = displacement + (coord.x * _ScreenParams.x / i.vertex.w);
		float sc = i.texcoord.y;
		float4 result;
		result = ((uint)(ps / floor(_ScanLines * lineSize)) % 2 == 0) ? color : _ScanLinesColor;
		result += color * sc;
		return lerp(color,result,fade);
	}

		float4 FragVD(VaryingsDefault i) : SV_Target
	{
		float2 coord = FisheyeDistortion(i.texcoord, sferical, barrel, scale);
		half4 color = tex2D(_MainTex, i.texcoord);
		float lineSize = _ScreenParams.y * 0.005;
		float displacement = ((_Time / 4 * 1000) * speed) % _ScreenParams.y;
		float ps;
		i.texcoord.x = frac(i.texcoord.x + cos((coord.y + (_Time / 4)) * 100) / _OffsetDistortion);
		ps = displacement + (i.texcoord.x * _ScreenParams.x / i.vertex.w);
		float sc = i.texcoord.y;
		float4 result;
		result = ((uint)(ps / floor(_ScanLines * lineSize)) % 2 == 0) ? color : _ScanLinesColor;
		result += color * sc;
		return lerp(color,result,fade);
	}

		ENDHLSL

		SubShader
	{
		Cull Off ZWrite Off ZTest Always

			Pass
		{
			HLSLPROGRAM

				#pragma vertex VertDefault
				#pragma fragment FragH

			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM

				#pragma vertex VertDefault
				#pragma fragment FragHD

			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM

				#pragma vertex VertDefault
				#pragma fragment FragV

			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM

				#pragma vertex VertDefault
				#pragma fragment FragVD

			ENDHLSL
		}
	}
}