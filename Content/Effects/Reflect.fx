sampler TextureSampler : register(s0);
sampler back : register(s1);

float x;
float y;

float scale=0.16f;

float4 PixelShaderFunction(float4 tpos : SV_POSITION, float4 color:COLOR, float2 pos : TEXCOORD0) : COLOR0
{    
	float x1=x/820.0f;
	float y1=y/1024.0f;
	

	float2 pos1;
	pos1.x=x1+((pos.x-0.5)/2.4*scale);
	pos1.y=y1+((pos.y-0.5)/3.2*scale);

	float4 front1 = tex2D(TextureSampler, pos);
	

	float2 center=float2(0.5,0.5);
	float dist=distance(float2(pos.x,pos.y),center);
	

	float2 vec=(float2(pos.x-0.5,pos.y-0.5));

	//vec is distance from pos to center
	
	pos1.x=pos1.x+(vec.x*(0.4-dist)*2); 
	pos1.y=pos1.y+(vec.y*(0.4-dist)*2);
	
	//return float4(abs(vec.x),abs(vec.y),0,1);

	float4 back1 = tex2D(back, pos1);
	back1.a=1;

	float4 reflection;
	
	
	if(dist>0.2 && dist<0.4)
	{
		//return back1;
		reflection.r=back1.r*(dist-0.2)*3;
		reflection.g=back1.g*(dist-0.2)*3;
		reflection.b=back1.b*(dist-0.2)*3;
		reflection.a=1;
		if(dist>0.39) reflection.a=0.5;
		back1.a=1;

		float4 c=lerp(front1,reflection,(dist-0.2)/0.2);
		//return float4(1,0,0,1);
		
		return c;
	}
	if(dist<=0.2 )
	{
		//return back1;
		return front1;
	}
	return float4(0,0,0,0);
	


}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_4_0 PixelShaderFunction();
    }
}
