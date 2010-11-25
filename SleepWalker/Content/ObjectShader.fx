sampler screen_Sampler : register(s0);

float4 PixelSampler(float2 texCoord: TEXCOORD0) : COLOR
{
	return tex2D(screen_Sampler, texCoord);
}

technique Technique1
{
    pass PassTest
    {
		ColorWriteEnable = RED|GREEN|BLUE|ALPHA;

        PixelShader = compile ps_2_0 PixelSampler();
    }
}
