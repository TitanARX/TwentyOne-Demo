Shader "RetroLookPro/VHS_RetroLook"
{
	HLSLINCLUDE
		half iterations;
	half smoothSize;
	float _StandardDeviation;
	half _OffsetNoiseX;
	half _OffsetNoiseY;
	sampler2D _MainTex;
	sampler2D _SecondaryTex;
	half _Stripes;
	float4 _MainTex_ST;
	float4 _SecondaryTex_ST;
#define PI 3.14159265359
#define E 2.71828182846
	float _Intensity;
	float _TexCut;
	float _OffsetColor;
	float2 _OffsetColorAngle;
	half _OffsetPosY;
	half _OffsetDistortion;
	half tileX = 0;
	half tileY = 0;
	half smooth = 0;
#define unity_ColorSpaceLuminance half4(0.0396819152, 0.458021790, 0.00609653955, 1.0) 

	struct AttributesDefault
	{
		float3 vertex : POSITION;
	};
	float4 _ScreenParams;             // x: width,          y: height,   z: 1+1/width, w: 1+1/height
	struct VaryingsDefault
	{
		float4 vertex : SV_POSITION;
		float2 texcoord : TEXCOORD0;
		float2 texcoordStereo : TEXCOORD1;
	};
	float Time;
	float2 TransformTriangleVertexToUV(float2 vertex)
	{
		float2 uv = (vertex + 1.0) * 0.5;
		return uv;
	}
	inline half luminance(half3 rgb)
	{
		return dot(rgb, unity_ColorSpaceLuminance.rgb);
	}

#define half4_one half4(1.0, 1.0, 1.0, 1.0)
	
	VaryingsDefault VertDef(AttributesDefault v)
	{
		VaryingsDefault o;
		o.vertex = float4(v.vertex.xy, 0.0, 1.0);
		o.texcoord = TransformTriangleVertexToUV(v.vertex.xy);
		o.texcoord = o.texcoord * float2(1.0, -1.0) + float2(0.0, 1.0);
		o.texcoordStereo = o.texcoord + float2(_OffsetNoiseX - 0.2f, _OffsetNoiseY);
		o.texcoordStereo *= float2(tileY, tileX);
		return o;
	}
	float4 box(sampler2D tex, float2 uv, float4 size)
	{
		float4 col = 0;
		float sum = 0;

		//iterate over blur samples
		for (float index = 0; index < iterations; index++) {
			//get the offset of the sample
			float offset = (index / (10 - 1) - 0.5) * size.x;
			//get uv coordinate of sample
			float2 uv1 = uv + float2(0, offset);

			//calculate the result of the gaussian function
			float stDevSquared = _StandardDeviation * _StandardDeviation;
			float gauss = (1 / sqrt(2 * PI * stDevSquared)) * pow(E, -((offset * offset) / (2 * stDevSquared)));
			//add result to sum
			sum += gauss;
			//multiply color with influence from gaussian function and add it to sum color
			col += tex2D(tex, uv1) * gauss;

		}
		//divide the sum of values by the amount of samples
		col = col / sum;

		return col;
	}
	float smoothCut(float colR) {
		if (smooth)
			return saturate(colR - _TexCut);
		else
			return ceil(colR - _TexCut);
	}

	float4 FragDivide(VaryingsDefault i) : SV_Target
	{
		i.texcoord = float2(frac(i.texcoord.x), frac(i.texcoord.y + _OffsetPosY));
		i.texcoord.x = _OffsetDistortion < 3498 ? frac(i.texcoord.x + cos((i.texcoord.y + Time) * 100) / _OffsetDistortion) : frac(i.texcoord.x);
		float4 col = tex2D(_MainTex, i.texcoord);
		half  amount = _OffsetColor * (distance(i.texcoord, half2(0.5, 0.5))) * 2;
		col.r = tex2D(_MainTex, i.texcoord + (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).r;
		col.b = tex2D(_MainTex, i.texcoord - (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).b;
		float4 col2 = tex2D(_SecondaryTex, i.texcoordStereo);
		if(smooth)
		col2 = box(_SecondaryTex, i.texcoordStereo, smoothSize);
		col2 = col2 / col;
		col2 = lerp(col,col2,_Intensity);
		return lerp(col, col2, smoothCut(col2.r)) * (1 - ceil(saturate(abs(i.texcoord.y - 0.5) - _Stripes)));
	}
		float4 FragSubtract(VaryingsDefault i) : SV_Target
	{
		i.texcoord = float2(frac(i.texcoord.x), frac(i.texcoord.y + _OffsetPosY));
		i.texcoord.x = _OffsetDistortion < 3498 ? frac(i.texcoord.x + cos((i.texcoord.y + Time) * 100) / _OffsetDistortion) : frac(i.texcoord.x);
		float4 col = tex2D(_MainTex, i.texcoord);
		half  amount = _OffsetColor * (distance(i.texcoord, half2(0.5, 0.5))) * 2;
		col.r = tex2D(_MainTex, i.texcoord + (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).r;
		col.b = tex2D(_MainTex, i.texcoord - (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).b;
		float4 col2 = tex2D(_SecondaryTex, i.texcoordStereo);
		if (smooth)
			col2 = box(_SecondaryTex, i.texcoordStereo, smoothSize);
		col2 = saturate(col - col2);
		col2 = lerp(col,col2,_Intensity);
		return lerp(col, col2, smoothCut(col2.r)) * (1 - ceil(saturate(abs(i.texcoord.y - 0.5) - _Stripes)));
	}
		float4 FragMultiply(VaryingsDefault i) : SV_Target
	{
		i.texcoord = float2(frac(i.texcoord.x), frac(i.texcoord.y + _OffsetPosY));
		i.texcoord.x = _OffsetDistortion < 3498 ? frac(i.texcoord.x + cos((i.texcoord.y + Time) * 100) / _OffsetDistortion) : frac(i.texcoord.x);
		float4 col = tex2D(_MainTex, i.texcoord);
		half  amount = _OffsetColor * (distance(i.texcoord, half2(0.5, 0.5))) * 2;
		col.r = tex2D(_MainTex, i.texcoord + (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).r;
		col.b = tex2D(_MainTex, i.texcoord - (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).b;
		float4 col2 = tex2D(_SecondaryTex, i.texcoordStereo);
		if (smooth)
			col2 = box(_SecondaryTex, i.texcoordStereo, smoothSize);
		col2 = saturate(col * col2);
		col2 = lerp(col,col2,_Intensity);
		return lerp(col, col2, smoothCut(col2.r)) * (1 - ceil(saturate(abs(i.texcoord.y - 0.5) - _Stripes)));
	}
		float4 FragColorBurn(VaryingsDefault i) : SV_Target
	{
		i.texcoord = float2(frac(i.texcoord.x), frac(i.texcoord.y + _OffsetPosY));
		i.texcoord.x = _OffsetDistortion < 3498 ? frac(i.texcoord.x + cos((i.texcoord.y + Time) * 100) / _OffsetDistortion) : frac(i.texcoord.x);
		float4 col = tex2D(_MainTex, i.texcoord);
		half  amount = _OffsetColor * (distance(i.texcoord, half2(0.5, 0.5))) * 2;
		col.r = tex2D(_MainTex, i.texcoord + (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).r;
		col.b = tex2D(_MainTex, i.texcoord - (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).b;
		float4 col2 = tex2D(_SecondaryTex, i.texcoordStereo);
		if (smooth)
			col2 = box(_SecondaryTex, i.texcoordStereo, smoothSize);
		col2 = half4_one - (half4_one - col) / col2;
		col2 = lerp(col,col2,_Intensity);
		return lerp(col, col2, smoothCut(col2.r)) * (1 - ceil(saturate(abs(i.texcoord.y - 0.5) - _Stripes)));
	}
		float4 FragLinearBurn(VaryingsDefault i) : SV_Target
	{
		i.texcoord = float2(frac(i.texcoord.x), frac(i.texcoord.y + _OffsetPosY));
		i.texcoord.x = _OffsetDistortion < 3498 ? frac(i.texcoord.x + cos((i.texcoord.y + Time) * 100) / _OffsetDistortion) : frac(i.texcoord.x);
		float4 col = tex2D(_MainTex, i.texcoord);
		half  amount = _OffsetColor * (distance(i.texcoord, half2(0.5, 0.5))) * 2;
		col.r = tex2D(_MainTex, i.texcoord + (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).r;
		col.b = tex2D(_MainTex, i.texcoord - (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).b;
		float4 col2 = tex2D(_SecondaryTex, i.texcoordStereo);
		if (smooth)
			col2 = box(_SecondaryTex, i.texcoordStereo, smoothSize);
		col2 = col2 + col - half4_one;
		col2 = lerp(col,col2,_Intensity);
		return lerp(col, col2, smoothCut(col2.r)) * (1 - ceil(saturate(abs(i.texcoord.y - 0.5) - _Stripes)));
	}
		float4 FragDarkerColor(VaryingsDefault i) : SV_Target
	{
		i.texcoord = float2(frac(i.texcoord.x), frac(i.texcoord.y + _OffsetPosY));
		i.texcoord.x = _OffsetDistortion < 3498 ? frac(i.texcoord.x + cos((i.texcoord.y + Time) * 100) / _OffsetDistortion) : frac(i.texcoord.x);
		float4 col = tex2D(_MainTex, i.texcoord);
		half  amount = _OffsetColor * (distance(i.texcoord, half2(0.5, 0.5))) * 2;
		col.r = tex2D(_MainTex, i.texcoord + (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).r;
		col.b = tex2D(_MainTex, i.texcoord - (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).b;
		float4 col2 = tex2D(_SecondaryTex, i.texcoordStereo);
		if (smooth)
			col2 = box(_SecondaryTex, i.texcoordStereo, smoothSize);
		col2 = luminance(col.rgb) < luminance(col2.rgb) ? col : col2;
		col2 = lerp(col,col2,_Intensity);
		return lerp(col, col2, smoothCut(col2.r)) * (1 - ceil(saturate(abs(i.texcoord.y - 0.5) - _Stripes)));
	}
		float4 FragLighten(VaryingsDefault i) : SV_Target
	{
		i.texcoord = float2(frac(i.texcoord.x), frac(i.texcoord.y + _OffsetPosY));
		i.texcoord.x = _OffsetDistortion < 3498 ? frac(i.texcoord.x + cos((i.texcoord.y + Time) * 100) / _OffsetDistortion) : frac(i.texcoord.x);
		float4 col = tex2D(_MainTex, i.texcoord);
		half  amount = _OffsetColor * (distance(i.texcoord, half2(0.5, 0.5))) * 2;
		col.r = tex2D(_MainTex, i.texcoord + (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).r;
		col.b = tex2D(_MainTex, i.texcoord - (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).b;
		float4 col2 = tex2D(_SecondaryTex, i.texcoordStereo);
		if (smooth)
			col2 = box(_SecondaryTex, i.texcoordStereo, smoothSize);
		col2 = max(col, col2);
		col2 = lerp(col,col2,_Intensity);
		return lerp(col, col2, smoothCut(col2.r)) * (1 - ceil(saturate(abs(i.texcoord.y - 0.5) - _Stripes)));
	}
		float4 FragScreen(VaryingsDefault i) : SV_Target
	{
		i.texcoord = float2(frac(i.texcoord.x), frac(i.texcoord.y + _OffsetPosY));
		i.texcoord.x = _OffsetDistortion < 3498 ? frac(i.texcoord.x + cos((i.texcoord.y + Time) * 100) / _OffsetDistortion) : frac(i.texcoord.x);
		float4 col = tex2D(_MainTex, i.texcoord);
		half  amount = _OffsetColor * (distance(i.texcoord, half2(0.5, 0.5))) * 2;
		col.r = tex2D(_MainTex, i.texcoord + (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).r;
		col.b = tex2D(_MainTex, i.texcoord - (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).b;
		float4 col2 = tex2D(_SecondaryTex, i.texcoordStereo);
		if (smooth)
			col2 = box(_SecondaryTex, i.texcoordStereo, smoothSize);
		col2 = half4_one - ((half4_one - col2) * (half4_one - col));
		col2 = lerp(col,col2,_Intensity);
		return lerp(col, col2, smoothCut(col2.r)) * (1 - ceil(saturate(abs(i.texcoord.y - 0.5) - _Stripes)));
	}
		float4 FragColorDodge(VaryingsDefault i) : SV_Target
	{
		i.texcoord = float2(frac(i.texcoord.x), frac(i.texcoord.y + _OffsetPosY));
		i.texcoord.x = _OffsetDistortion < 3498 ? frac(i.texcoord.x + cos((i.texcoord.y + Time) * 100) / _OffsetDistortion) : frac(i.texcoord.x);
		float4 col = tex2D(_MainTex, i.texcoord);
		half  amount = _OffsetColor * (distance(i.texcoord, half2(0.5, 0.5))) * 2;
		col.r = tex2D(_MainTex, i.texcoord + (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).r;
		col.b = tex2D(_MainTex, i.texcoord - (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).b;
		float4 col2 = tex2D(_SecondaryTex, i.texcoordStereo);
		if (smooth)
			col2 = box(_SecondaryTex, i.texcoordStereo, smoothSize);
		col2 = col / (half4_one - col2);
		col2 = lerp(col,col2,_Intensity);
		return lerp(col, col2, smoothCut(col2.r)) * (1 - ceil(saturate(abs(i.texcoord.y - 0.5) - _Stripes)));
	}
		float4 FragLinearDodge(VaryingsDefault i) : SV_Target
	{
		i.texcoord = float2(frac(i.texcoord.x), frac(i.texcoord.y + _OffsetPosY));
		i.texcoord.x = _OffsetDistortion < 3498 ? frac(i.texcoord.x + cos((i.texcoord.y + Time) * 100) / _OffsetDistortion) : frac(i.texcoord.x);
		float4 col = tex2D(_MainTex, i.texcoord);
		half  amount = _OffsetColor * (distance(i.texcoord, half2(0.5, 0.5))) * 2;
		col.r = tex2D(_MainTex, i.texcoord + (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).r;
		col.b = tex2D(_MainTex, i.texcoord - (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).b;
		float4 col2 = tex2D(_SecondaryTex, i.texcoordStereo);
		if (smooth)
			col2 = box(_SecondaryTex, i.texcoordStereo, smoothSize);
		col2 = col + col2;
		col2 = lerp(col,col2,_Intensity);
		return lerp(col, col2, smoothCut(col2.r)) * (1 - ceil(saturate(abs(i.texcoord.y - 0.5) - _Stripes)));
	}
		float4 FragLighterColor(VaryingsDefault i) : SV_Target
	{
		i.texcoord = float2(frac(i.texcoord.x), frac(i.texcoord.y + _OffsetPosY));
		i.texcoord.x = _OffsetDistortion < 3498 ? frac(i.texcoord.x + cos((i.texcoord.y + Time) * 100) / _OffsetDistortion) : frac(i.texcoord.x);
		float4 col = tex2D(_MainTex, i.texcoord);
		half  amount = _OffsetColor * (distance(i.texcoord, half2(0.5, 0.5))) * 2;
		col.r = tex2D(_MainTex, i.texcoord + (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).r;
		col.b = tex2D(_MainTex, i.texcoord - (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).b;
		float4 col2 = tex2D(_SecondaryTex, i.texcoordStereo);
		if (smooth)
			col2 = box(_SecondaryTex, i.texcoordStereo, smoothSize);
		col2 = luminance(col.rgb) > luminance(col2.rgb) ? col : col2;
		col2 = lerp(col,col2,_Intensity);
		return lerp(col, col2, smoothCut(col2.r)) * (1 - ceil(saturate(abs(i.texcoord.y - 0.5) - _Stripes)));
	}
		float4 FragOverlay(VaryingsDefault i) : SV_Target
	{
		i.texcoord = float2(frac(i.texcoord.x), frac(i.texcoord.y + _OffsetPosY));
		i.texcoord.x = _OffsetDistortion < 3498 ? frac(i.texcoord.x + cos((i.texcoord.y + Time) * 100) / _OffsetDistortion) : frac(i.texcoord.x);
		float4 col = tex2D(_MainTex, i.texcoord);
		half  amount = _OffsetColor * (distance(i.texcoord, half2(0.5, 0.5))) * 2;
		col.r = tex2D(_MainTex, i.texcoord + (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).r;
		col.b = tex2D(_MainTex, i.texcoord - (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).b;
		float4 col2 = tex2D(_SecondaryTex, i.texcoordStereo);
		if (smooth)
			col2 = box(_SecondaryTex, i.texcoordStereo, smoothSize);
		float4 check = step(0.5, col2);
		float4 ress = check * (half4_one - ((half4_one - 2.0 * (col - 0.5)) * (half4_one - col2)));
		ress += (half4_one - check) * (2.0 * col * col2);
		col2 = ress;
		col2 = lerp(col,col2,_Intensity);
		return lerp(col, col2, smoothCut(col2.r)) * (1 - ceil(saturate(abs(i.texcoord.y - 0.5) - _Stripes)));
	}
		float4 FragSoftLight(VaryingsDefault i) : SV_Target
	{
		i.texcoord = float2(frac(i.texcoord.x), frac(i.texcoord.y + _OffsetPosY));
		i.texcoord.x = _OffsetDistortion < 3498 ? frac(i.texcoord.x + cos((i.texcoord.y + Time) * 100) / _OffsetDistortion) : frac(i.texcoord.x);
		float4 col = tex2D(_MainTex, i.texcoord);
		half  amount = _OffsetColor * (distance(i.texcoord, half2(0.5, 0.5))) * 2;
		col.r = tex2D(_MainTex, i.texcoord + (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).r;
		col.b = tex2D(_MainTex, i.texcoord - (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).b;
		float4 col2 = tex2D(_SecondaryTex, i.texcoordStereo);
		if (smooth)
			col2 = box(_SecondaryTex, i.texcoordStereo, smoothSize);
		float4 check = step(0.5, col2);
		float4 result = check * (2.0 * col * col2 + col * col - 2.0 * col * col * col2);
		result += (half4_one - check) * (2.0 * sqrt(col) * col2 - sqrt(col) + 2.0 * col - 2.0 * col * col2);
		col2 = result;
		col2 = lerp(col,col2,_Intensity);
		return lerp(col, col2, smoothCut(col2.r)) * (1 - ceil(saturate(abs(i.texcoord.y - 0.5) - _Stripes)));
	}
		float4 FragHardLight(VaryingsDefault i) : SV_Target
	{
		i.texcoord = float2(frac(i.texcoord.x), frac(i.texcoord.y + _OffsetPosY));
		i.texcoord.x = _OffsetDistortion < 3498 ? frac(i.texcoord.x + cos((i.texcoord.y + Time) * 100) / _OffsetDistortion) : frac(i.texcoord.x);
		float4 col = tex2D(_MainTex, i.texcoord);
		half  amount = _OffsetColor * (distance(i.texcoord, half2(0.5, 0.5))) * 2;
		col.r = tex2D(_MainTex, i.texcoord + (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).r;
		col.b = tex2D(_MainTex, i.texcoord - (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).b;
		float4 col2 = tex2D(_SecondaryTex, i.texcoordStereo);
		if (smooth)
			col2 = box(_SecondaryTex, i.texcoordStereo, smoothSize);
		float4 check = step(0.5, col2);
		float4 result = check * (half4_one - ((half4_one - 2.0 * (col - 0.5)) * (half4_one - col2)));
		result += (half4_one - check) * (2.0 * col * col2);
		col2 = result;
		col2 = lerp(col,col2,_Intensity);
		return lerp(col, col2, smoothCut(col2.r)) * (1 - ceil(saturate(abs(i.texcoord.y - 0.5) - _Stripes)));
	}
		float4 FragVividLight(VaryingsDefault i) : SV_Target
	{
		i.texcoord = float2(frac(i.texcoord.x), frac(i.texcoord.y + _OffsetPosY));
		i.texcoord.x = _OffsetDistortion < 3498 ? frac(i.texcoord.x + cos((i.texcoord.y + Time) * 100) / _OffsetDistortion) : frac(i.texcoord.x);
		float4 col = tex2D(_MainTex, i.texcoord);
		half  amount = _OffsetColor * (distance(i.texcoord, half2(0.5, 0.5))) * 2;
		col.r = tex2D(_MainTex, i.texcoord + (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).r;
		col.b = tex2D(_MainTex, i.texcoord - (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).b;
		float4 col2 = tex2D(_SecondaryTex, i.texcoordStereo);
		if (smooth)
			col2 = box(_SecondaryTex, i.texcoordStereo, smoothSize);
		float4 check = step(0.5, col2);
		float4 result = check * (col / (half4_one - 2.0 * (col2 - 0.5)));
		result += (half4_one - check) * (half4_one - (half4_one - col) / (2.0 * col2));
		col2 = result;
		col2 = lerp(col,col2,_Intensity);
		return lerp(col, col2, smoothCut(col2.r)) * (1 - ceil(saturate(abs(i.texcoord.y - 0.5) - _Stripes)));
	}
		float4 FragLinearLight(VaryingsDefault i) : SV_Target
	{
		i.texcoord = float2(frac(i.texcoord.x), frac(i.texcoord.y + _OffsetPosY));
		i.texcoord.x = _OffsetDistortion < 3498 ? frac(i.texcoord.x + cos((i.texcoord.y + Time) * 100) / _OffsetDistortion) : frac(i.texcoord.x);
		float4 col = tex2D(_MainTex, i.texcoord);
		half  amount = _OffsetColor * (distance(i.texcoord, half2(0.5, 0.5))) * 2;
		col.r = tex2D(_MainTex, i.texcoord + (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).r;
		col.b = tex2D(_MainTex, i.texcoord - (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).b;
		float4 col2 = tex2D(_SecondaryTex, i.texcoordStereo);
		if (smooth)
			col2 = box(_SecondaryTex, i.texcoordStereo, smoothSize);
		float4 check = step(0.5, col2);
		float4 result = check * (col + (2.0 * (col2 - 0.5)));
		result += (half4_one - check) * (col + 2.0 * col2 - half4_one);
		col2 = result;
		col2 = lerp(col,col2,_Intensity);
		return lerp(col, col2, smoothCut(col2.r)) * (1 - ceil(saturate(abs(i.texcoord.y - 0.5) - _Stripes)));
	}
		float4 FragPinLight(VaryingsDefault i) : SV_Target
	{
		i.texcoord = float2(frac(i.texcoord.x), frac(i.texcoord.y + _OffsetPosY));
		i.texcoord.x = _OffsetDistortion < 3498 ? frac(i.texcoord.x + cos((i.texcoord.y + Time) * 100) / _OffsetDistortion) : frac(i.texcoord.x);
		float4 col = tex2D(_MainTex, i.texcoord);
		half  amount = _OffsetColor * (distance(i.texcoord, half2(0.5, 0.5))) * 2;
		col.r = tex2D(_MainTex, i.texcoord + (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).r;
		col.b = tex2D(_MainTex, i.texcoord - (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).b;
		float4 col2 = tex2D(_SecondaryTex, i.texcoordStereo);
		if (smooth)
			col2 = box(_SecondaryTex, i.texcoordStereo, smoothSize);
		float4 check = step(0.5, col2);
		float4 result = check * max(2.0 * (col2 - 0.5), col);
		result += (half4_one - check) * min(2 * col2, col);
		col2 = result;
		col2 = lerp(col,col2,_Intensity);
		return lerp(col, col2, smoothCut(col2.r)) * (1 - ceil(saturate(abs(i.texcoord.y - 0.5) - _Stripes)));
	}
		float4 FragHardMix(VaryingsDefault i) : SV_Target
	{
		i.texcoord = float2(frac(i.texcoord.x), frac(i.texcoord.y + _OffsetPosY));
		i.texcoord.x = _OffsetDistortion < 3498 ? frac(i.texcoord.x + cos((i.texcoord.y + Time) * 100) / _OffsetDistortion) : frac(i.texcoord.x);
		float4 col = tex2D(_MainTex, i.texcoord);
		half  amount = _OffsetColor * (distance(i.texcoord, half2(0.5, 0.5))) * 2;
		col.r = tex2D(_MainTex, i.texcoord + (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).r;
		col.b = tex2D(_MainTex, i.texcoord - (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).b;
		float4 col2 = tex2D(_SecondaryTex, i.texcoordStereo);
		if (smooth)
			col2 = box(_SecondaryTex, i.texcoordStereo, smoothSize);
		float4 result = float4(0.0, 0.0, 0.0, 0.0);
		result.r = col2.r > 1.0 - col.r ? 1.0 : 0.0;
		result.g = col2.g > 1.0 - col.g ? 1.0 : 0.0;
		result.b = col2.b > 1.0 - col.b ? 1.0 : 0.0;
		result.a = col2.a > 1.0 - col.a ? 1.0 : 0.0;
		col2 = result;
		col2 = lerp(col,col2,_Intensity);
		return lerp(col, col2, smoothCut(col2.r)) * (1 - ceil(saturate(abs(i.texcoord.y - 0.5) - _Stripes)));
	}
		float4 FragDifference(VaryingsDefault i) : SV_Target
	{
		i.texcoord = float2(frac(i.texcoord.x), frac(i.texcoord.y + _OffsetPosY));
		i.texcoord.x = _OffsetDistortion < 3498 ? frac(i.texcoord.x + cos((i.texcoord.y + Time) * 100) / _OffsetDistortion) : frac(i.texcoord.x);
		float4 col = tex2D(_MainTex, i.texcoord);
		half  amount = _OffsetColor * (distance(i.texcoord, half2(0.5, 0.5))) * 2;
		col.r = tex2D(_MainTex, i.texcoord + (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).r;
		col.b = tex2D(_MainTex, i.texcoord - (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).b;
		float4 col2 = tex2D(_SecondaryTex, i.texcoordStereo);
		if (smooth)
			col2 = box(_SecondaryTex, i.texcoordStereo, smoothSize);
		col2 = abs(col - col2);
		col2 = lerp(col,col2,_Intensity);
		return lerp(col, col2, smoothCut(col2.r)) * (1 - ceil(saturate(abs(i.texcoord.y - 0.5) - _Stripes)));
	}
		float4 FragExclusion(VaryingsDefault i) : SV_Target
	{
		i.texcoord = float2(frac(i.texcoord.x), frac(i.texcoord.y + _OffsetPosY));
		i.texcoord.x = _OffsetDistortion < 3498 ? frac(i.texcoord.x + cos((i.texcoord.y + Time) * 100) / _OffsetDistortion) : frac(i.texcoord.x);
		float4 col = tex2D(_MainTex, i.texcoord);
		half  amount = _OffsetColor * (distance(i.texcoord, half2(0.5, 0.5))) * 2;
		col.r = tex2D(_MainTex, i.texcoord + (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).r;
		col.b = tex2D(_MainTex, i.texcoord - (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).b;
		float4 col2 = tex2D(_SecondaryTex, i.texcoordStereo);
		if (smooth)
			col2 = box(_SecondaryTex, i.texcoordStereo, smoothSize);
		col2 = col + col2 - (2.0 * col * col2);
		col2 = lerp(col,col2,_Intensity);
		return lerp(col, col2, smoothCut(col2.r)) * (1 - ceil(saturate(abs(i.texcoord.y - 0.5) - _Stripes)));
	}
		float4 FragDarken(VaryingsDefault i) : SV_Target
	{
		i.texcoord = float2(frac(i.texcoord.x), frac(i.texcoord.y + _OffsetPosY));
		i.texcoord.x = _OffsetDistortion < 3498 ? frac(i.texcoord.x + cos((i.texcoord.y + Time) * 100) / _OffsetDistortion) : frac(i.texcoord.x);
		float4 col = tex2D(_MainTex, i.texcoord);
		half  amount = _OffsetColor * (distance(i.texcoord, half2(0.5, 0.5))) * 2;
		col.r = tex2D(_MainTex, i.texcoord + (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).r;
		col.b = tex2D(_MainTex, i.texcoord - (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).b;
		float4 col2 = tex2D(_SecondaryTex, i.texcoordStereo);
		if (smooth)
			col2 = box(_SecondaryTex, i.texcoordStereo, smoothSize);
		col2 = min(col, col2);
		col2 = lerp(col,col2,_Intensity);
		return lerp(col, col2, smoothCut(col2.r)) * (1 - ceil(saturate(abs(i.texcoord.y - 0.5) - _Stripes)));
	}


	float4 Frag(VaryingsDefault i) : SV_Target
	{
		i.texcoord = float2(frac(i.texcoord.x), frac(i.texcoord.y + _OffsetPosY));
		i.texcoord.x = _OffsetDistortion < 3498 ? frac(i.texcoord.x + cos((i.texcoord.y + Time) * 100) / _OffsetDistortion) : frac(i.texcoord.x);
		float4 col = tex2D(_MainTex, i.texcoord);
		half  amount = _OffsetColor * (distance(i.texcoord, half2(0.5, 0.5))) * 2;
		col.r = tex2D(_MainTex, i.texcoord + (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).r;
		col.b = tex2D(_MainTex, i.texcoord - (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).b;
		float4 col2 = tex2D(_SecondaryTex, i.texcoordStereo);
		if (smooth)
			col2 = box(_SecondaryTex, i.texcoordStereo, smoothSize);
		col2 = lerp(col,col2,_Intensity);
		return lerp(col, col2, smoothCut(col2.r)) * (1 - ceil(saturate(abs(i.texcoord.y - 0.5) - _Stripes)));
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
			Pass
		{
			HLSLPROGRAM
				#pragma vertex VertDef
				#pragma fragment FragDarken
			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM
				#pragma vertex VertDef
				#pragma fragment FragMultiply
			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM
				#pragma vertex VertDef
				#pragma fragment FragColorBurn
			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM
				#pragma vertex VertDef
				#pragma fragment FragLinearBurn
			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM
				#pragma vertex VertDef
				#pragma fragment FragDarkerColor
			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM
				#pragma vertex VertDef
				#pragma fragment FragLighten
			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM
				#pragma vertex VertDef
				#pragma fragment FragScreen
			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM
				#pragma vertex VertDef
				#pragma fragment FragColorDodge
			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM
				#pragma vertex VertDef
				#pragma fragment FragLinearDodge
			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM
				#pragma vertex VertDef
				#pragma fragment FragLighterColor
			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM
				#pragma vertex VertDef
				#pragma fragment FragOverlay
			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM
				#pragma vertex VertDef
				#pragma fragment FragSoftLight
			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM
				#pragma vertex VertDef
				#pragma fragment FragHardLight
			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM
				#pragma vertex VertDef
				#pragma fragment FragVividLight
			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM
				#pragma vertex VertDef
				#pragma fragment FragLinearLight
			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM
				#pragma vertex VertDef
				#pragma fragment FragPinLight
			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM
				#pragma vertex VertDef
				#pragma fragment FragHardMix
			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM
				#pragma vertex VertDef
				#pragma fragment FragDifference
			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM
				#pragma vertex VertDef
				#pragma fragment FragExclusion
			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM
				#pragma vertex VertDef
				#pragma fragment FragSubtract
			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM
				#pragma vertex VertDef
				#pragma fragment FragDivide
			ENDHLSL
		}
	}
}