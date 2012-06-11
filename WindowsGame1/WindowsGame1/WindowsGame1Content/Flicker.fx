sampler texSampler;
float size;

float4 PS_COLOR(float2 texCoord: TEXCOORD0) : COLOR
{
   texCoord.y += sin(texCoord.x + size * 12.0f) * 0.01f;
   //texCoord.x += cos(texCoord.y + size * 50.0f) * 0.005f;
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