Shader "Warrior/Water"
{
    Properties
    {
        [Header(Water Color)]
        _ShallowColor("Shallow Color", Color) = (0.3, 0.8, 0.9, 0.6)
        _DeepColor("Deep Color", Color) = (0.05, 0.15, 0.4, 0.9)
        _DepthFade("Depth Fade Distance", Range(0.1, 10)) = 2.0
        _FresnelPower("Fresnel Power", Range(0.1, 10)) = 3.0

        [Header(Emission)]
        [HDR] _EmissionColor("Emission Color", Color) = (0, 0.2, 0.3, 1)
        _EmissionStrength("Emission Strength", Range(0, 5)) = 0.5

        [Header(Flow)]
        _FlowDirection("Flow Direction (XY)", Vector) = (1, 0, 0, 0)
        _FlowSpeed("Flow Speed", Range(0, 5)) = 0.5

        [Header(Normal Maps)]
        [Normal] _NormalMapA("Normal Map A", 2D) = "bump" {}
        [Normal] _NormalMapB("Normal Map B", 2D) = "bump" {}
        _NormalScale("Normal Strength", Range(0, 2)) = 0.5
        _NormalTiling("Normal Tiling", Range(0.1, 10)) = 1.0

        [Header(Surface)]
        _Smoothness("Smoothness", Range(0, 1)) = 0.95
        _Specular("Specular", Range(0, 1)) = 0.5
        _Distortion("Refraction Distortion", Range(0, 0.5)) = 0.05

        [Header(Reflections)]
        [HDR] _ReflectionTint("Reflection Tint (HDR)", Color) = (1, 1, 1, 1)
        _ReflectionStrength("Reflection Strength", Range(0, 5)) = 1.5
        _ReflectionFresnelBoost("Reflection Fresnel Boost", Range(0, 3)) = 1.0
        _ReflectionNormalStrength("Reflection Normal Distortion", Range(0, 3)) = 1.0
        _ReflectionRoughness("Reflection Roughness", Range(0, 1)) = 0.05
        _ReflectionBaseAmount("Reflection Base Amount", Range(0, 1)) = 0.2

        [Header(Foam)]
        _FoamColor("Foam Color", Color) = (1, 1, 1, 1)
        _FoamDistance("Foam Distance", Range(0, 2)) = 0.4
        _FoamCutoff("Foam Cutoff", Range(0, 1)) = 0.5

        [Header(Caustics)]
        _CausticsScale("Caustics Scale", Range(0.1, 10)) = 2.0
        _CausticsStrength("Caustics Strength", Range(0, 3)) = 0.5
        _CausticsSpeed("Caustics Speed", Range(0, 2)) = 0.5

        [Header(Waves)]
        _WaveAmplitude("Wave Amplitude", Range(0, 0.5)) = 0.05
        _WaveFrequency("Wave Frequency", Range(0, 10)) = 2.0
        _WaveSpeed("Wave Speed", Range(0, 5)) = 1.0
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Back

        Pass
        {
            Name "PoolWater"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareOpaqueTexture.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float4 tangentOS  : TANGENT;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS   : SV_POSITION;
                float2 uv           : TEXCOORD0;
                float3 positionWS   : TEXCOORD1;
                float3 normalWS     : TEXCOORD2;
                float3 tangentWS    : TEXCOORD3;
                float3 bitangentWS  : TEXCOORD4;
                float4 screenPos    : TEXCOORD5;
                float  fogFactor    : TEXCOORD6;
                float3 viewDirWS    : TEXCOORD7;
            };

            TEXTURE2D(_NormalMapA);  SAMPLER(sampler_NormalMapA);
            TEXTURE2D(_NormalMapB);  SAMPLER(sampler_NormalMapB);

            CBUFFER_START(UnityPerMaterial)
                half4  _ShallowColor;
                half4  _DeepColor;
                float  _DepthFade;
                float  _FresnelPower;

                half4  _EmissionColor;
                float  _EmissionStrength;

                float4 _FlowDirection;
                float  _FlowSpeed;

                float  _NormalScale;
                float  _NormalTiling;

                float  _Smoothness;
                float  _Specular;
                float  _Distortion;

                half4  _ReflectionTint;
                float  _ReflectionStrength;
                float  _ReflectionFresnelBoost;
                float  _ReflectionNormalStrength;
                float  _ReflectionRoughness;
                float  _ReflectionBaseAmount;

                half4  _FoamColor;
                float  _FoamDistance;
                float  _FoamCutoff;

                float  _CausticsScale;
                float  _CausticsStrength;
                float  _CausticsSpeed;

                float  _WaveAmplitude;
                float  _WaveFrequency;
                float  _WaveSpeed;
            CBUFFER_END

            // Vertex Shader
            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                // Vertex wave displacement
                float3 posOS = IN.positionOS.xyz;
                float2 flowDir = normalize(_FlowDirection.xy + float2(0.001, 0.001));
                float flowDot = dot(posOS.xz, flowDir);

                posOS.y += sin(flowDot * _WaveFrequency + _Time.y * _WaveSpeed) * _WaveAmplitude;
                posOS.y += sin(flowDot * _WaveFrequency * 0.7 + _Time.y * _WaveSpeed * 1.3 + 1.5) * _WaveAmplitude * 0.5;

                VertexPositionInputs posInputs = GetVertexPositionInputs(posOS);
                VertexNormalInputs normInputs = GetVertexNormalInputs(IN.normalOS, IN.tangentOS);

                OUT.positionCS  = posInputs.positionCS;
                OUT.positionWS  = posInputs.positionWS;
                OUT.normalWS    = normInputs.normalWS;
                OUT.tangentWS   = normInputs.tangentWS;
                OUT.bitangentWS = normInputs.bitangentWS;
                OUT.uv          = IN.uv;
                OUT.screenPos   = ComputeScreenPos(posInputs.positionCS);
                OUT.fogFactor   = ComputeFogFactor(posInputs.positionCS.z);
                OUT.viewDirWS   = GetWorldSpaceNormalizeViewDir(posInputs.positionWS);

                return OUT;
            }

            // Procedural Caustics
            float3 Caustics(float2 uv, float time)
            {
                // Two layers of animated voronoi-like caustics
                float2 p = uv * _CausticsScale;
                float t = time * _CausticsSpeed;

                float v1 = 0;
                float v2 = 0;

                // Layer 1
                for (int i = 0; i < 3; i++)
                {
                    float fi = float(i);
                    float2 offset = float2(
                        sin(t + fi * 1.3) * 0.5,
                        cos(t + fi * 1.7) * 0.5
                    );
                    float d = length(frac(p + offset + fi * 0.33) - 0.5);
                    v1 += smoothstep(0.4, 0.0, d);
                }

                // Layer 2 (shifted)
                for (int j = 0; j < 3; j++)
                {
                    float fj = float(j);
                    float2 offset2 = float2(
                        cos(t * 0.7 + fj * 2.1) * 0.5,
                        sin(t * 0.7 + fj * 1.9) * 0.5
                    );
                    float d2 = length(frac(p * 1.3 + offset2 + fj * 0.33) - 0.5);
                    v2 += smoothstep(0.4, 0.0, d2);
                }

                float caustic = min(v1, v2) * 0.33;
                return caustic * _CausticsStrength;
            }

            // Fragment Shader
            half4 frag(Varyings IN) : SV_Target
            {
                // Flow
                float2 flowDir = normalize(_FlowDirection.xy + float2(0.001, 0.001));
                float flowTime = _Time.y * _FlowSpeed;

                // Phase cycling to avoid texture stretching
                float phase0 = frac(flowTime * 0.5);
                float phase1 = frac(flowTime * 0.5 + 0.5);
                float blendPhase = abs(phase0 * 2.0 - 1.0);

                float2 uvTiled = IN.positionWS.xz * _NormalTiling;
                float2 uv0 = uvTiled - flowDir * phase0;
                float2 uv1 = uvTiled - flowDir * phase1;

                // Normal Mapping
                half3 normalA0 = UnpackNormalScale(SAMPLE_TEXTURE2D(_NormalMapA, sampler_NormalMapA, uv0), _NormalScale);
                half3 normalA1 = UnpackNormalScale(SAMPLE_TEXTURE2D(_NormalMapA, sampler_NormalMapA, uv1), _NormalScale);

                // Second normal layer at different scale/angle for variety
                float2 uv0b = uvTiled * 0.7 - flowDir * phase0 * 0.8 + float2(0.5, 0.3);
                float2 uv1b = uvTiled * 0.7 - flowDir * phase1 * 0.8 + float2(0.5, 0.3);
                half3 normalB0 = UnpackNormalScale(SAMPLE_TEXTURE2D(_NormalMapB, sampler_NormalMapB, uv0b), _NormalScale * 0.5);
                half3 normalB1 = UnpackNormalScale(SAMPLE_TEXTURE2D(_NormalMapB, sampler_NormalMapB, uv1b), _NormalScale * 0.5);

                half3 normalTS_A = lerp(normalA0, normalA1, blendPhase);
                half3 normalTS_B = lerp(normalB0, normalB1, blendPhase);

                // Blend normals (RNM)
                half3 normalTS;
                normalTS.xy = normalTS_A.xy + normalTS_B.xy;
                normalTS.z  = normalTS_A.z * normalTS_B.z;
                normalTS = normalize(normalTS);

                // Transform to world space
                float3x3 TBN = float3x3(
                    normalize(IN.tangentWS),
                    normalize(IN.bitangentWS),
                    normalize(IN.normalWS)
                );
                float3 normalWS = normalize(mul(normalTS, TBN));

                // Punchier Reflection Normal
                // Boost the tangent-space normal specifically for reflections
                half3 reflNormalTS = normalTS;
                reflNormalTS.xy *= _ReflectionNormalStrength;
                reflNormalTS = normalize(reflNormalTS);
                float3 reflNormalWS = normalize(mul(reflNormalTS, TBN));

                // Depth
                float2 screenUV = IN.screenPos.xy / IN.screenPos.w;

                float rawDepth = SampleSceneDepth(screenUV);
                float sceneEyeDepth = LinearEyeDepth(rawDepth, _ZBufferParams);
                float surfaceEyeDepth = IN.screenPos.w;
                float depthDiff = sceneEyeDepth - surfaceEyeDepth;
                float depthFactor = saturate(depthDiff / _DepthFade);

                // Water Color
                half4 waterColor = lerp(_ShallowColor, _DeepColor, depthFactor);

                // Refraction
                float2 refractionOffset = normalTS.xy * _Distortion;
                float2 refractedUV = screenUV + refractionOffset;

                // Prevent sampling above water surface
                float refractedDepth = SampleSceneDepth(refractedUV);
                float refractedEyeDepth = LinearEyeDepth(refractedDepth, _ZBufferParams);
                if (refractedEyeDepth < surfaceEyeDepth)
                {
                    refractedUV = screenUV;
                }

                half3 sceneColor = SampleSceneColor(refractedUV);

                // Fresnel
                float fresnel = pow(1.0 - saturate(dot(IN.viewDirWS, normalWS)), _FresnelPower);
                fresnel = saturate(fresnel);

                // Caustics
                float2 causticsUV = IN.positionWS.xz - flowDir * flowTime * 0.3;
                float3 causticColor = Caustics(causticsUV, _Time.y);

                // Apply caustics more in shallow areas
                causticColor *= (1.0 - depthFactor);

                // Foam
                float foamMask = 0;
                if (_FoamDistance > 0.001)
                {
                    float foamDepth = saturate(depthDiff / _FoamDistance);

                    // Animated foam pattern
                    float2 foamUV = IN.positionWS.xz * 3.0 - flowDir * flowTime;
                    float foamNoise = SAMPLE_TEXTURE2D(_NormalMapA, sampler_NormalMapA, foamUV).r;
                    float foamNoise2 = SAMPLE_TEXTURE2D(_NormalMapB, sampler_NormalMapB, foamUV * 1.3 + 0.5).r;
                    float combinedNoise = (foamNoise + foamNoise2) * 0.5;

                    foamMask = step(_FoamCutoff, combinedNoise) * (1.0 - foamDepth);
                }

                // Lighting
                float4 shadowCoord = TransformWorldToShadowCoord(IN.positionWS);
                Light mainLight = GetMainLight(shadowCoord);

                // Diffuse (subtle since water is mostly specular)
                float NdotL = saturate(dot(normalWS, mainLight.direction));
                half3 diffuse = mainLight.color * NdotL * mainLight.shadowAttenuation * 0.3;

                // Specular (Blinn-Phong for performance)
                float3 halfDir = normalize(mainLight.direction + IN.viewDirWS);
                float NdotH = saturate(dot(normalWS, halfDir));
                float specPow = exp2(_Smoothness * 10.0 + 1.0);
                float spec = pow(NdotH, specPow) * _Specular;
                half3 specular = mainLight.color * spec * mainLight.shadowAttenuation;

                // Additional lights
                half3 additionalDiffuse = 0;
                half3 additionalSpecular = 0;

                #ifdef _ADDITIONAL_LIGHTS
                uint pixelLightCount = GetAdditionalLightsCount();
                for (uint lightIndex = 0; lightIndex < pixelLightCount; ++lightIndex)
                {
                    Light addLight = GetAdditionalLight(lightIndex, IN.positionWS);
                    float addNdotL = saturate(dot(normalWS, addLight.direction));
                    additionalDiffuse += addLight.color * addNdotL * addLight.distanceAttenuation * addLight.shadowAttenuation * 0.3;

                    float3 addHalf = normalize(addLight.direction + IN.viewDirWS);
                    float addNdotH = saturate(dot(normalWS, addHalf));
                    additionalSpecular += addLight.color * pow(addNdotH, specPow) * _Specular * addLight.distanceAttenuation * addLight.shadowAttenuation;
                }
                #endif

                // Composite
                half3 totalLight = diffuse + additionalDiffuse;

                // Blend refracted scene with water color based on depth
                half3 finalColor = lerp(sceneColor, waterColor.rgb, waterColor.a * depthFactor);

                // Apply lighting
                finalColor = finalColor * (totalLight + 0.7); // 0.7 = ambient approximation
                finalColor += specular + additionalSpecular;

                // Add caustics
                finalColor += causticColor * waterColor.rgb;

                // Apply emission
                finalColor += _EmissionColor.rgb * _EmissionStrength * waterColor.rgb;

                // Apply foam
                finalColor = lerp(finalColor, _FoamColor.rgb, foamMask * _FoamColor.a);

                // Enhanced Reflections
                // Use boosted reflection normal for stronger ripple effect on reflections
                half3 reflectDir = reflect(-IN.viewDirWS, reflNormalWS);

                // Sample reflection probe with controllable roughness
                half3 reflColor = GlossyEnvironmentReflection(reflectDir, _ReflectionRoughness, 1.0);

                // Apply HDR tint and strength boost
                reflColor *= _ReflectionTint.rgb * _ReflectionStrength;

                // Reflection amount = base amount + fresnel-boosted amount
                float reflectionAmount = saturate(_ReflectionBaseAmount + fresnel * _ReflectionFresnelBoost);

                finalColor = lerp(finalColor, reflColor, reflectionAmount);

                // Alpha
                float alpha = lerp(_ShallowColor.a, _DeepColor.a, depthFactor);
                alpha = saturate(alpha + reflectionAmount * 0.4 + foamMask);

                // Fog
                finalColor = MixFog(finalColor, IN.fogFactor);

                return half4(finalColor, alpha);
            }
            ENDHLSL
        }

        // Shadow caster pass (optional, for casting shadows)
        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }

            ZWrite On
            ZTest LEqual
            ColorMask 0
            Cull Back

            HLSLPROGRAM
            #pragma vertex ShadowVert
            #pragma fragment ShadowFrag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
            };

            float3 _LightDirection;

            Varyings ShadowVert(Attributes IN)
            {
                Varyings OUT;
                float3 posWS = TransformObjectToWorld(IN.positionOS.xyz);
                float3 normWS = TransformObjectToWorldNormal(IN.normalOS);
                posWS = ApplyShadowBias(posWS, normWS, _LightDirection);
                OUT.positionCS = TransformWorldToHClip(posWS);

                #if UNITY_REVERSED_Z
                    OUT.positionCS.z = min(OUT.positionCS.z, UNITY_NEAR_CLIP_VALUE);
                #else
                    OUT.positionCS.z = max(OUT.positionCS.z, UNITY_NEAR_CLIP_VALUE);
                #endif

                return OUT;
            }

            half4 ShadowFrag(Varyings IN) : SV_Target
            {
                return 0;
            }
            ENDHLSL
        }

        // Depth pass
        Pass
        {
            Name "DepthOnly"
            Tags { "LightMode" = "DepthOnly" }

            ZWrite On
            ColorMask R
            Cull Back

            HLSLPROGRAM
            #pragma vertex DepthVert
            #pragma fragment DepthFrag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
            };

            Varyings DepthVert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                return OUT;
            }

            half4 DepthFrag(Varyings IN) : SV_Target
            {
                return 0;
            }
            ENDHLSL
        }
    }

    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}
