
Shader "RetroLookPro/ColorPalette"
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
	sampler3D _Colormap;
	sampler2D _Palette;
	sampler2D _BlueNoise;
	float4 _BlueNoise_TexelSize;
	float _Opacity;
	float _Dither;
	
	half CalcLuminance(float3 color)
	{
		return dot(color, float3(0.299f, 0.587f, 0.114f));
	}

	float4 Frag(VaryingsDefault i) : SV_Target
	{
		float4 inputColor = tex2D(_MainTex, i.texcoord);
		inputColor = saturate(inputColor);
		float4 colorInColormap = tex3D(_Colormap, inputColor.rgb);

		float random = tex2D(_BlueNoise, i.vertex.xy / _BlueNoise_TexelSize.z).r;
		random = saturate(random);

		if (CalcLuminance(colorInColormap.r) > CalcLuminance(colorInColormap.g))
		{
			random = 1 - random;
		}

		float paletteIndex;
		float blend = colorInColormap.b;
		float threshold = saturate((1 / _Dither) * (blend - 0.5 + (_Dither / 2)));

		if (random < threshold)
		{
			paletteIndex = colorInColormap.g;
		}
		else
		{
			paletteIndex = colorInColormap.r;
		}

		float4 result = tex2D(_Palette, float2(paletteIndex, 0));
		result.a = inputColor.a;
		result = lerp(inputColor, result, _Opacity);

		return result;
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