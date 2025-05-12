//#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
//#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
//#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
//#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
//#include "Packages/com.unity.shadergraph/Editor/Generation/Targets/Fullscreen/Includes/FullscreenShaderPass.cs.hlsl"
//#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
//#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
//#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
//#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/SpaceTransforms.hlsl"
//#include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"
#include "ColorUtils.hlsl"

#ifndef SHADERGRAPH_PREVIEW
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#endif

#ifndef SHADERGRAPH_PREVIEW
TEXTURE2D_X(_BlitTexture);
#endif
float4 Unity_Universal_SampleBuffer_BlitSource_float(uint2 pixelCoords)
{
    #ifndef SHADERGRAPH_PREVIEW
    return LOAD_TEXTURE2D_X_LOD(_BlitTexture, pixelCoords, 0);
    #else
    return 0;
    #endif
}

#define SECTOR_SIZE 7

void Kuwahara_float(in float2 uv, in float intensity, out float3 color_out)
{
    const uint2 pixelUV = uv * _ScreenSize.xy;
    
    const float3 startColor = Unity_Universal_SampleBuffer_BlitSource_float(pixelUV);
    
    float3 kernel[SECTOR_SIZE][SECTOR_SIZE];

    uint2 sectorUV;
    
    float3 avgColor[4];
    float avgValue[4];
    float std[4];

    [unroll]
    for (int i = 0; i < 4; i++)
    {
        avgColor[i] = 0;
        avgValue[i] = 0;
        std[i] = 0;
    }
    
    // UL Sector
    for (int y = 0; y < SECTOR_SIZE; y++)
        for (int x = 0; x < SECTOR_SIZE; x++)
        {
            sectorUV = pixelUV + uint2(-x, -y);
            float3 color = Unity_Universal_SampleBuffer_BlitSource_float(sectorUV);
            kernel[x][y] = color;
            avgColor[0] += color;
        }
    avgColor[0] /= SECTOR_SIZE * SECTOR_SIZE;
    avgValue[0] = RGBtoHSV(avgColor[0]);
        
    for (int y = 0; y < SECTOR_SIZE; y++)
        for (int x = 0; x < SECTOR_SIZE; x++)
        {
            float3 hcvColor = RGBtoHSV(kernel[x][y]);
            std[0] += (avgValue[0] - hcvColor.z) * (avgValue[0] - hcvColor.z);
        }
    
    // UR Sector
    for (int y = 0; y < SECTOR_SIZE; y++)
        for (int x = 0; x < SECTOR_SIZE; x++)
        {
            sectorUV = pixelUV + uint2(x, -y);
            float3 color = Unity_Universal_SampleBuffer_BlitSource_float(sectorUV);
            kernel[x][y] = color;
            avgColor[1] += color;
        }
    avgColor[1] /= SECTOR_SIZE * SECTOR_SIZE;
    avgValue[1] = RGBtoHSV(avgColor[1]).z;
    
    for (int y = 0; y < SECTOR_SIZE; y++)
        for (int x = 0; x < SECTOR_SIZE; x++)
        {
            float3 hcvColor = RGBtoHSV(kernel[x][y]);
            std[1] += (avgValue[1] - hcvColor.z) * (avgValue[1] - hcvColor.z);
        }
    
    // DL Sector
    for (int y = 0; y < SECTOR_SIZE; y++)
        for (int x = 0; x < SECTOR_SIZE; x++)
        {
            sectorUV = pixelUV + uint2(-x, y);
            float3 color = Unity_Universal_SampleBuffer_BlitSource_float(sectorUV);
            kernel[x][y] = color;
            avgColor[2] += color;
        }
    avgColor[2] /= SECTOR_SIZE * SECTOR_SIZE;
    avgValue[2] = RGBtoHSV(avgColor[2]).z;
    
    for (int y = 0; y < SECTOR_SIZE; y++)
        for (int x = 0; x < SECTOR_SIZE; x++)
        {
            float3 hcvColor = RGBtoHSV(kernel[x][y]);
            std[2] += (avgValue[2] - hcvColor.z) * (avgValue[2] - hcvColor.z);
        }
    
    // DR Sector
    for (int y = 0; y < SECTOR_SIZE; y++)
        for (int x = 0; x < SECTOR_SIZE; x++)
        {
            sectorUV = pixelUV + uint2(x, y);
            float3 color = Unity_Universal_SampleBuffer_BlitSource_float(sectorUV);
            kernel[x][y] = color;
            avgColor[3] += color;
        }
    avgColor[3] /= SECTOR_SIZE * SECTOR_SIZE;
    avgValue[3] = RGBtoHSV(avgColor[3]).z;
    
    for (int y = 0; y < SECTOR_SIZE; y++)
        for (int x = 0; x < SECTOR_SIZE; x++)
        {
            float3 hcvColor = RGBtoHSV(kernel[x][y]);
            std[3] += (avgValue[3] - hcvColor.z) * (avgValue[3] - hcvColor.z);
        }

    const float minStd = min(std[3], min(std[2], min(std[1], std[0])));

    float3 outColor;
    
    if (minStd == std[0])
        outColor = avgColor[0];
    else if (minStd == std[1])
        outColor = avgColor[1];
    else if (minStd == std[2])
        outColor = avgColor[2];
    else
        outColor = avgColor[3];

    color_out = lerp(startColor, outColor, intensity);
}