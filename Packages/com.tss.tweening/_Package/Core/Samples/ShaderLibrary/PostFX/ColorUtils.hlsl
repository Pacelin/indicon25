const float epsilon = 1e-10;

float3 HUEtoRGB(const in float h)
{
    const float r = abs(h * 6 - 3) - 1;
    const float g = 2 - abs(h * 6 - 2);
    const float b = 2 - abs(h * 6 - 4);
    return saturate(float3(r, g, b));
}

float3 RGBtoHCV(in float3 rgb)
{
    const float4 p = (rgb.g < rgb.b) ? float4(rgb.bg, -1.0, 2.0/3.0) : float4(rgb.gb, 0.0, -1.0/3.0);
    const float4 q = (rgb.r < p.x) ? float4(p.xyw, rgb.r) : float4(rgb.r, p.yzx);
    const float c = q.x - min(q.w, q.y);
    const float h = abs((q.w - q.y) / (6 * c + epsilon) + q.z);
    return float3(h, c, q.x);
}

float3 RGBtoHSV(const in float3 rgb)
{
    const float3 hcv = RGBtoHCV(rgb);
    const float s = hcv.y / (hcv.z + epsilon);
    return float3(hcv.x, s, hcv.z);
}

float3 HSVtoRGB(in float3 hsv)
{
    const float3 rgb = HUEtoRGB(hsv.x);
    return ((rgb - 1) * hsv.y + 1) * hsv.z;
}