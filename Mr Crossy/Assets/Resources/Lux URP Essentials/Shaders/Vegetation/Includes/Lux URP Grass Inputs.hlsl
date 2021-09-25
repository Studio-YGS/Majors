#ifndef INPUT_LUXLWRP_BASE_INCLUDED
#define INPUT_LUXLWRP_BASE_INCLUDED

    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
//  defines a bunch of helper functions (like lerpwhiteto)
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"  
//  defines SurfaceData, textures and the functions Alpha, SampleAlbedoAlpha, SampleNormal, SampleEmission
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
//  defines e.g. "DECLARE_LIGHTMAP_OR_SH"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"

//  Material Inputs
    CBUFFER_START(UnityPerMaterial)
        half4   _BaseColor;
        half    _AlphaClip;
        float4  _BaseMap_ST;
        half    _Cutoff;
        half    _Smoothness;
        half4   _SpecColor;
        half    _Occlusion;
        half4   _WindMultiplier;
        float2  _DistanceFade;
        half    _BumpScale;
    CBUFFER_END

//  Additional textures
    TEXTURE2D(_LuxLWRPWindRT); SAMPLER(sampler_LuxLWRPWindRT);

//  Global Inputs
    half4 _LuxLWRPWindDirSize;
    half4 _LuxLWRPWindStrengthMultipliers;
    float4 _LuxLWRPSinTime;

//  Structs
    struct VertexInput
    {
        float3 positionOS                   : POSITION;
        float3 normalOS                     : NORMAL;
        float4 tangentOS                    : TANGENT;
        float2 texcoord                     : TEXCOORD0;
        float2 lightmapUV                   : TEXCOORD1;
        half4 color                         : COLOR;
        UNITY_VERTEX_INPUT_INSTANCE_ID
    };
    
    struct VertexOutput
    {
        float4 positionCS                   : SV_POSITION;
        float2 uv                           : TEXCOORD0;

        half2 fadeOcclusion                 : TEXCOORD9;

        #if !defined(UNITY_PASS_SHADOWCASTER) && !defined(DEPTHONLYPASS)
            half3 normalWS                 : TEXCOORD3;
        #endif

        #if !defined(UNITY_PASS_SHADOWCASTER) && !defined(DEPTHONLYPASS)
            DECLARE_LIGHTMAP_OR_SH(lightmapUV, vertexSH, 1);
            #if defined(REQUIRES_WORLD_SPACE_POS_INTERPOLATOR)
                float3 positionWS           : TEXCOORD2;
            #endif
            
            #ifdef _NORMALMAP
                half4 tangentWS             : TEXCOORD4;
            #endif
            //float3 viewDirWS              : TEXCOORD5;

            half4 fogFactorAndVertexLight   : TEXCOORD5;
            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
                float4 shadowCoord          : TEXCOORD6;
            #endif
        #endif
        UNITY_VERTEX_INPUT_INSTANCE_ID
        UNITY_VERTEX_OUTPUT_STEREO
    };

    struct SurfaceDescription
    {
        float3 albedo;
        float alpha;
        float3 normalTS;
        float3 emission;
        float metallic;
        float3 specular;
        float smoothness;
        float occlusion;
        //float translucency;
    };

    void BendGrass(float3 positionOS, half3 normalOS, half4 vertexColors, out float3 positionWS, out half3 normalWS, out half2 fadeOcclusion) {
        #if defined (_BENDINGMODE_ALPHA)
            #define bendAmount vertexColors.a
        #else
            #define bendAmount vertexColors.b
        #endif
        #define phase vertexColors.gg
        
        fadeOcclusion = half2(1,1);

        #if !defined(_ALPHATEST_ON)
            float3 worldInstancePos = UNITY_MATRIX_M._m03_m13_m23;
            float3 diff = (_WorldSpaceCameraPos - worldInstancePos);
            float dist = dot(diff, diff);
            half fade = saturate( (_DistanceFade.x - dist) * _DistanceFade.y );
        //  Shrink mesh
            positionOS.xyz *= fade;
        #endif

        positionWS = TransformObjectToWorld(positionOS.xyz);
        half4 wind = SAMPLE_TEXTURE2D_LOD(_LuxLWRPWindRT, sampler_LuxLWRPWindRT, positionWS.xz * _LuxLWRPWindDirSize.w + phase * _WindMultiplier.z, _WindMultiplier.w);

    //  Calculate fade
        #if defined(_ALPHATEST_ON)
            float3 worldInstancePos = UNITY_MATRIX_M._m03_m13_m23;
            float3 diff = (_WorldSpaceCameraPos - worldInstancePos);
            float dist = dot(diff, diff);
            fadeOcclusion.x = saturate( (_DistanceFade.x - dist) * _DistanceFade.y );
        #endif

        half windStrength = bendAmount * _LuxLWRPWindStrengthMultipliers.x * _WindMultiplier.x;
        half3 windDir = _LuxLWRPWindDirSize.xyz;

        wind.r = wind.r * (wind.g * 2.0h - 0.243h);  // not a "real" normal as we want to keep the base direction
        windStrength *= wind.r;

    //  Displace vertices
        positionWS.xz += windDir.xz * windStrength;

    //  Do something to the normal as well
        normalWS = TransformObjectToWorldNormal(normalOS);
        half2 normalWindDir = windDir.xz * _WindMultiplier.y;
        normalWS.xz += normalWindDir * windStrength;
        normalWS = NormalizeNormalPerVertex(normalWS);

    }


#endif