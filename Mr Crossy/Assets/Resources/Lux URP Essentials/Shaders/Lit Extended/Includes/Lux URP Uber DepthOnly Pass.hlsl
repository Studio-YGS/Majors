//  ///////////////////////////////////////////////
//  Lux
//  We use a different keyword but want to keep as much of the original code, so:
#if defined(_PARALLAX)
    #define _PARALLAXMAP
#endif
//  ///////////////////////////////////////////////


// GLES2 has limited amount of interpolators
#if defined(_PARALLAXMAP) && !defined(SHADER_API_GLES)
    #define REQUIRES_TANGENT_SPACE_VIEW_DIR_INTERPOLATOR
#endif

#if (defined(_NORMALMAP) || (defined(_PARALLAXMAP) && !defined(REQUIRES_TANGENT_SPACE_VIEW_DIR_INTERPOLATOR))) || defined(_DETAIL)
#define REQUIRES_WORLD_SPACE_TANGENT_INTERPOLATOR
#endif

struct Attributes
{
    float4 positionOS       : POSITION;
    float4 tangentOS        : TANGENT;
    float2 texcoord         : TEXCOORD0;
    float3 normalOS         : NORMAL;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
    float4 positionCS       : SV_POSITION;
    float2 uv               : TEXCOORD1;
    half3 normalWS          : TEXCOORD2;

    #if defined(REQUIRES_WORLD_SPACE_TANGENT_INTERPOLATOR)
        half4 tangentWS     : TEXCOORD3;
    #endif

    #if defined(REQUIRES_TANGENT_SPACE_VIEW_DIR_INTERPOLATOR)
        half3 viewDirTS     : TEXCOORD4;
    #endif

    //UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};


Varyings DepthOnlyVertex(Attributes input)
{
    Varyings output = (Varyings)0;
    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

    float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
    output.positionCS = TransformWorldToHClip(positionWS);

    #if defined(_ALPHATEST_ON)
        output.uv = TRANSFORM_TEX(input.texcoord, _BaseMap);
        VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);
        #if defined(REQUIRES_TANGENT_SPACE_VIEW_DIR_INTERPOLATOR)
            half3x3 tangentSpaceRotation =  half3x3(normalInput.tangentWS, normalInput.bitangentWS, normalInput.normalWS);
            half3 viewDirWS = GetWorldSpaceNormalizeViewDir(positionWS);
            output.viewDirTS = SafeNormalize( mul(tangentSpaceRotation, viewDirWS) );
        #endif
            output.normalWS = normalInput.normalWS;
            float sign = input.tangentOS.w * float(GetOddNegativeScale());
            output.tangentWS = half4(normalInput.tangentWS.xyz, sign);
        #else
    #endif

    return output;
}


half4 DepthOnlyFragment(Varyings input, half facing : VFACE) : SV_TARGET
{
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

//  LOD crossfading
    #if defined(LOD_FADE_CROSSFADE) && !defined(SHADER_API_GLES)
        clip (unity_LODFade.x - Dither32(input.positionCS.xy, 1));
    #endif

    #if defined(_ALPHATEST_ON)
        #if defined(_FADING_ON)
            clip ( input.positionCS.w - _CameraFadeDist - Dither32(input.positionCS.xy, 1));
        #endif

        float2 uv = input.uv;
        
    //  Parallax
        #if defined(REQUIRES_TANGENT_SPACE_VIEW_DIR_INTERPOLATOR)
            half3 viewDirTS = input.viewDirTS;
            viewDirTS.z *= facing;
        #else
            half3 viewDirWS = GetWorldSpaceNormalizeViewDir(input.positionWS);
            half3 viewDirTS = GetViewDirectionTangentSpace(input.tangentWS, input.normalWS, viewDirWS);
            viewDirTS.z *= facing;
        #endif
        float3 v = viewDirTS;
        v.z += 0.42;
        v.xy /= v.z;
        float halfParallax = _Parallax * 0.5f;
        float parallax = SAMPLE_TEXTURE2D(_HeightMap, sampler_HeightMap, uv).g * _Parallax - halfParallax;
        float2 offset1 = parallax * v.xy;
    //  Calculate 2nd height
        parallax = SAMPLE_TEXTURE2D(_HeightMap, sampler_HeightMap, uv + offset1).g * _Parallax - halfParallax;
        float2 offset2 = parallax * v.xy;
    //  Final UVs
        uv += (offset1 + offset2) * 0.5f;

        half alpha = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uv).a * _BaseColor.a;
        clip (alpha - _Cutoff);
    #endif

    return 0;
}