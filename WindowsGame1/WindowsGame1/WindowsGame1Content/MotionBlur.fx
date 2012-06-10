float2 vel;
sampler texSampler;
float4 PS_COLOR(float2 texCoord: TEXCOORD0) : COLOR
{
	float4 color = 0;

	color += tex2D(texSampler, texCoord + vel * 0.001f);
	color += tex2D(texSampler, texCoord - vel * 0.001f);
	color += tex2D(texSampler, texCoord + vel * 0.002f);
	color += tex2D(texSampler, texCoord - vel * 0.002f);
	color += tex2D(texSampler, texCoord + vel * 0.003f);
	color += tex2D(texSampler, texCoord - vel * 0.003f);
	color += tex2D(texSampler, texCoord + vel * 0.004f);
	color += tex2D(texSampler, texCoord - vel * 0.004f); 

	color /= 8;
   
   return color;
}

technique MotionBlur
{
   pass pass0
   {
	  PixelShader = compile ps_2_0 PS_COLOR();
   }
}