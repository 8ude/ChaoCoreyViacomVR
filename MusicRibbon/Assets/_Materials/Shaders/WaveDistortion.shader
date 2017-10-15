Shader "Custom/Waves 2"
{
	Properties
	{
		_Color("", Color) = (1,1,1,1)
		_MainTex("Color", 2D) = "white" {}
		_OffsetScale("Offset Scale",Range(0,10)) = 1
		_Smoothing("Smoothing",Range(0,1)) = 0.5 //in case we want to recalc. normals
		_AudioInput("AudioInput", Float) = 0.0
	
		A ("Amplitude", Float) = 0.5 //amplitude
		L ("Wavelength", Float) = 1 //wavelength
		S ("Speed", Float) = 0.1 //speed
		Q ("Steepness", Range(0,1)) = 0.5 //steepness
		i ("Number of Waves",Range(1,10)) = 1 //number of waves
		D ("Wave Direction", Vector) = (0.5,0.0,0.5,0.0) //wave direction

	}
		SubShader
	{
		Tags{ "RenderType" = "Opaque" }


		CGPROGRAM

#pragma surface surf Standard vertex:vert nolightmap  addshadow alpha:fade 
#pragma target 3.0

//#include "Common.cginc"

	struct Input { 
		float2 uv_MainTex : TEXCOORD0;
		float dummy; 
	};

	half _Glossiness;
	half _Metallic;
	fixed4 _Color;

	sampler2D _MainTex;

	float _OffsetScale = 1;
	float _Smoothing; //in case we want to calculate normals and smooth them

	float3 _DisplaceTarget;

	float _AudioInput;

	float A = 0.5; //amplitude
	float L = 1; //wavelength
	float S = 0.1; //speed
	float Q = 0.5; //steepness
	float i = 1; //number of waves
	float2 D = float2(0.5,0.5); //wave direction
	float w;

	//w = 2/L
	float freq(float wavelength){
		return 2/L;
	}

	float QA(float w){
		i = ceil(i);
		//return A;
		return A*Q/(w*A*i);
	}

	float phaseConstant(float speed){
		return speed * w;
	}

	float gInner(float3 pos){
		return dot(D,pos.xy) * w + _Time.y * phaseConstant(S);
		
		//return sin(posXY.x)*A;
	}

	float3 gerstnerWave(float3 pos){
		w = freq(L);
		D = normalize(D);
		float ampCalc = QA(w);
		float waveCalc = gInner(pos);
		pos.x += ampCalc * D.x * cos(waveCalc);
		pos.z += ampCalc * D.y * cos(waveCalc);
		pos.y += A * sin(waveCalc);

		return pos;
	}

	void vert(inout appdata_full v)
	{	
		float3 wsVertex = v.vertex.xyz;//
		//wsVertex = mul(unity_ObjectToWorld, v.vertex);
		float3 wsVertexOut = gerstnerWave(wsVertex);

		

		//get a vector to the target, and the magnitude
		//float3 vecToTarget = _DisplaceTarget - wsVertex;
		//float lenToTarget = length(vecToTarget);
		
		//move each vertex along vector toward target, 
		//scaled by the distance of this vertex from the target
		//and the Offset scale (parameter set in material inspector)
		//wsVertex += normalize(vecToTarget) / lenToTarget *_OffsetScale;

		v.vertex.xyz = wsVertexOut;
		v.vertex.xyz *= (0.5 + _AudioInput);


		

	}

	void surf(Input IN, inout SurfaceOutputStandard o)
	{
		o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * _Color;//_Color.rgb;
		//o.Albedo = _MainTex.rgb * _Color;//_Color.rgb;

		o.Metallic = 0;// _Metallic;
		o.Smoothness = 0;// _Glossiness;
		float colVal = saturate((1 - tex2D(_MainTex, IN.uv_MainTex).b) * 10); //make blue alpha .. but wrong blue for now
		o.Alpha = 1;
	}

	ENDCG
	}
		FallBack "Diffuse"
}