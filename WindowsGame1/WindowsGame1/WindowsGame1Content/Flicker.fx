sampler texSampler;
float size;

float4 PS_COLOR(float2 texCoord: TEXCOORD0) : COLOR
{
   texCoord.y += sin(texCoord.x + size * 100.0f) * 0.001f;
   texCoord.x += cos(texCoord.y + size * 100.0f) * 0.001f;
   float4 color = tex2D(texSampler, texCoord);
   
   return color;
}

technique FlickerShader
{
   pass pass0
   {
      PixelShader = compile ps_2_0 PS_COLOR();
   }
} 