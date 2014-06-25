texture2D DisplayTexture;
sampler2D DisplaySampler = sampler_state
{
    Texture = <DisplayTexture>;
};

float4 TextureShader( float2 texCoord : TEXCOORD0 ) : COLOR
{
    return tex2D( DisplaySampler, texCoord );
}

technique
{
    pass Main
    {
        PixelShader = compile ps_2_0 TextureShader();
    }
}