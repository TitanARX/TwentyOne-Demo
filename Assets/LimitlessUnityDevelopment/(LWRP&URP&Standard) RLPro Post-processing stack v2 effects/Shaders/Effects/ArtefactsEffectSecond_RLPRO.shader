Shader "RetroLookPro/ArtefactsEffectSecond" 
{
    HLSLINCLUDE

       // #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
		
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
#if STEREO_INSTANCING_ENABLED
    uint stereoTargetEyeIndex : SV_RenderTargetArrayIndex;
#endif
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
VaryingsDefault VertDefault(AttributesDefault v)
{
    VaryingsDefault o;
    o.vertex = float4(v.vertex.xy, 0.0, 1.0);
    o.texcoord = TransformTriangleVertexToUV(v.vertex.xy);

//#if UNITY_UV_STARTS_AT_TOP
    o.texcoord = o.texcoord * float2(1.0, -1.0) + float2(0.0, 1.0);
//#endif

    o.texcoordStereo = TransformStereoScreenSpaceTex(o.texcoord, 1.0);

    return o;
}		
		sampler2D _MainTex;
		sampler2D _FeedbackTex;
		float feedbackAmp = 1.0;
		half3 bm_screen(half3 a, half3 b){ 	return 1.0- (1.0-a)*(1.0-b); }

        float4 Frag(VaryingsDefault i) : SV_Target
        {
			float2 p = i.texcoordStereo;
			half4 col = tex2D( _MainTex, i.texcoordStereo); 		
			half3 fbb = tex2D( _FeedbackTex, i.texcoordStereo).rgb; 
			col.rgb = bm_screen(col.rgb, fbb*feedbackAmp);
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
    }
}