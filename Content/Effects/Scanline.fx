sampler SourceSampler : register(s0);

#define LINES_ON      1.0 
#define LINES_OFF     1.0 



uniform float PincushionAmount = 0.02f;
uniform float CurvatureAmount = 0.03f;

float width;
float height;

float targetwidth;
float targetheight=1024;

float redShift=1.0f;
float greenShift=0.0f;

float OFF_INTENSITY=0.7f;


float4 PixelShaderFunction(float4 tpos : SV_POSITION, float4 color:COLOR, float2 pos : TEXCOORD0) : COLOR0
{
	float pixelRatio = 1.0; 
       
	float2 Ratios = float2(1, 1);
	float2 PinViewpointOffset = float2(0.0f, 0.0f);
	float2 PinUnitCoord = (pos + PinViewpointOffset) * Ratios * 2.0f - 1.0f;
	float PincushionR2 = pow(length(PinUnitCoord), 2.0f) / pow(length(Ratios), 2.0f);
	float2 PincushionCurve = PinUnitCoord * CurvatureAmount * PincushionR2;

	float2 BaseCoord = pos;
	float2 ScanCoord = BaseCoord - 0.5f / float2(width, height);

	float2 RedCoord = ScanCoord;
	RedCoord.x=RedCoord.x+(redShift/width);

	float2 GreenCoord = ScanCoord;
	GreenCoord.x=GreenCoord.x+(greenShift/width);
	
	RedCoord -= 0.5f / Ratios;
	RedCoord *= 1.0f - PincushionAmount * Ratios * 0.2f; // Warning: Magic constant
	RedCoord += 0.5f / Ratios;
	RedCoord += PincushionCurve;

	GreenCoord -= 0.5f / Ratios;
	GreenCoord *= 1.0f - PincushionAmount * Ratios * 0.2f; // Warning: Magic constant
	GreenCoord += 0.5f / Ratios;
	GreenCoord += PincushionCurve;

	BaseCoord -= 0.5f / Ratios;
	BaseCoord *= 1.0f - PincushionAmount * Ratios * 0.2f; // Warning: Magic constant
	BaseCoord += 0.5f / Ratios;
	BaseCoord += PincushionCurve;

	ScanCoord -= 0.5f / Ratios;
	ScanCoord *= 1.0f - PincushionAmount * Ratios * 0.2f; // Warning: Magic constant
	ScanCoord += 0.5f / Ratios;
	ScanCoord += PincushionCurve;

	
	float4 c=tex2D(SourceSampler,ScanCoord);

	float red=tex2D(SourceSampler,RedCoord).r;
	float green=tex2D(SourceSampler,GreenCoord).g;
	
	clip((BaseCoord.x < 1.0f / width) ? -1 : 1);
	clip((BaseCoord.y < 1.0f / height) ? -1 : 1);
	clip((BaseCoord.x >  1.0f-(1.0f / width)) ? -1 : 1);
	clip((BaseCoord.y >  1.0f-(1.0f/height)) ? -1 : 1);

	float4 result;
	result.r=red;
	result.g=green;//c.g;
	result.b=c.b;
	result.a=c.a;
	


    float intensity = (pos.x * targetwidth * pixelRatio) % (LINES_ON + LINES_OFF); 
    intensity = intensity < LINES_ON ? 1.0f : OFF_INTENSITY; 

	

    return result * intensity; 

}


float4 PixelShaderFunction2(float4 color:COLOR, float2 pos : TEXCOORD0) : COLOR0
{
	
}


technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_4_0 PixelShaderFunction();
    }
	
}
