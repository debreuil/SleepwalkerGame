sampler screen_Sampler : register(s0);

float4 PixelSampler(float2 texCoord: TEXCOORD0) : COLOR
{
    const float3 sunColor = float3(255.0 / 255.0, 236.0 / 255.0, 178.0 / 255.0) * 0.0;
    
    float4 color = tex2D(screen_Sampler, texCoord); 
    float4 light = tex2D(screen_Sampler, texCoord - 0.001F);  
    
    const float shadowLength = 1.5F;
    
    float shadow = 
		(
			tex2D(screen_Sampler, texCoord - 0.001F * shadowLength).a - light.a +  
			tex2D(screen_Sampler, texCoord - 0.002F * shadowLength).a - light.a +  
			tex2D(screen_Sampler, texCoord - 0.003F * shadowLength).a - light.a +  
			tex2D(screen_Sampler, texCoord - 0.004F * shadowLength).a - light.a +  
			tex2D(screen_Sampler, texCoord - 0.005F * shadowLength).a - light.a +  
			tex2D(screen_Sampler, texCoord - 0.006F * shadowLength).a - light.a +  
			tex2D(screen_Sampler, texCoord - 0.007F * shadowLength).a - light.a +  
			tex2D(screen_Sampler, texCoord - 0.008F * shadowLength).a - light.a
		) / 8.0; 
    
    
    light.a = max(0, (light.a - 0.1) * 1.111111);
    
    return 
    float4
    (
		//Base Color
		color.rgb + 
		
		//Light
		max(0, (color.a - light.a)) * sunColor -
    
		//Shadow
		shadow * (1 - color.a) * 0.3,
		1
    );
}

technique Technique1
{
    pass PassTest
    {
        PixelShader = compile ps_2_0 PixelSampler();
    }
}
